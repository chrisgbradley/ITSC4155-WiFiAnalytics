using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading;

namespace DataReaderDaD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Regex regex = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+"
                                        + @"(?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s(?<HOST>\S+)\s+"
                                        + @"(?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+"
                                        + @"<(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?"
                                        + @"<(?<TYPE>[a-zA-Z]+)>");

        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();
        private ObservableCollection<FileInfo> Files = new ObservableCollection<FileInfo>(); // will update if changes

        private void InitializeBackgroundWorker()
        {
            // Background Process
            backgroundWorker.DoWork += ParallelizedBackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            // Progress Reporting
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            // Cancellation
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeBackgroundWorker();// set BGworker config
            FilesDataGrid.AutoGenerateColumns = false; // otherwise the datagrid will populate for all fields attached to the fileinfo object
            FilesDataGrid.ItemsSource = Files; // set datacontext/binding for datagrid
        }


        private void MainWindow1_DragEnter(object sender, DragEventArgs e)
        {
            // Used to prime the window for dragging and dropping.
            e.Effects = DragDropEffects.All;
        }


        private void MainWindow1_Drop(object sender, DragEventArgs e)
        {
            // dont allow drops while parsing
            if (backgroundWorker.IsBusy)
                return;

            // file paths of files dropped in
            string[] filesThatWereDropped = (string[])(e.Data.GetData(DataFormats.FileDrop, true));

            // add all paths to listbox in GUI
            foreach(string file in filesThatWereDropped)
            {
                Debug.WriteLine(file);
                if(!Files.Any(o => o.FullName == file))
                {
                    Files.Add(new FileInfo(file));
                }
            }
        }


        // Runs on UI Thread
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // protect start button while parsing
            if (backgroundWorker.IsBusy)
                return;

            // start parsing
            backgroundWorker.RunWorkerAsync(Files);

            StartButton.IsEnabled = !backgroundWorker.IsBusy;//disable start button while parsing
            CancelButton.IsEnabled = backgroundWorker.IsBusy;//enable cancel button while parsing
            OutputTextBox.Content = string.Empty; //prep output label
        }

        // Runs on UI Thread
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            backgroundWorker.CancelAsync(); // send cancellation request to worker
        }


        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker worker = sender as BackgroundWorker;

            // get file paths that were passed
            List<FileInfo> filesThatWereDropped = new List<FileInfo>((IEnumerable<FileInfo>)e.Argument);

            
            ulong totalSizeInBytes = 0;
            foreach (FileInfo file in filesThatWereDropped)
            {
                totalSizeInBytes += (ulong)file.Length;
            }

            ulong bytesReadSoFar = 0;
            ulong bytesLastReportedAt = 0;
            ulong reportIntervalInBytes = 131072;
            int linesCount = 0;

            Stopwatch sw = Stopwatch.StartNew();
            foreach (FileInfo file in filesThatWereDropped)
            {
                const Int32 BufferSize = 1024;
                // provides auto try catch block and inits reader for you
                using (StreamReader reader = new StreamReader(file.FullName, Encoding.UTF8, true, BufferSize))
                {
                    while (reader.Peek() >= 0) // until end of file
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                        string line = reader.ReadLine();

                        // not sure if this is exactly correct because im lazy
                        // taking the character length of the string and converting to bytes
                        bytesReadSoFar += (ulong)Encoding.UTF8.GetByteCount(line + "\n");
                        linesCount++;

                        Match match = regex.Match(line);

                        if (match.Success)
                        {
                            // @TODO: Remove this line. Only for debugging purposes. Replace with file write or database put
                            //Debug.WriteLine(parseMatch(match));

                            // only report progress if reportInterval has been passed
                            if (bytesReadSoFar - bytesLastReportedAt > reportIntervalInBytes)
                            {
                                int percentageComplete = (int)((float)bytesReadSoFar / totalSizeInBytes * 100);
                                string progressMessage = $"Read {bytesReadSoFar / 1024} KB of {totalSizeInBytes / 1024} KB";
                                worker.ReportProgress(percentageComplete, progressMessage);
                                bytesLastReportedAt = bytesReadSoFar; // reset report interval
                            }
                        }
                    }
                }
            }

            sw.Stop();

            TimeSpan ts = sw.Elapsed;

            Debug.WriteLine("\n\n-------------------------------------------------------------------------------------\n");
            Debug.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Debug.WriteLine("\n-------------------------------------------------------------------------------------\n\n");
        }


        private void ParallelizedBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker worker = sender as BackgroundWorker;

            // get file paths that were passed
            List<FileInfo> filesThatWereDropped = new List<FileInfo>((IEnumerable<FileInfo>)e.Argument);


            ulong totalSizeInBytes = 0;
            foreach (FileInfo file in filesThatWereDropped)
            {
                totalSizeInBytes += (ulong)file.Length;
            }

            ulong bytesReadSoFar = 0;
            ulong bytesLastReportedAt = 0;
            ulong reportIntervalInBytes = 131072;
            int linesCount = 0;

            Stopwatch sw = Stopwatch.StartNew();
            Parallel.ForEach(filesThatWereDropped, new ParallelOptions { MaxDegreeOfParallelism = 4 }, file =>
            {
                const Int32 BufferSize = 1024;
                // provides auto try catch block and inits reader for you
                using (StreamReader reader = new StreamReader(file.FullName, Encoding.UTF8, true, BufferSize))
                {
                    while (reader.Peek() >= 0) // until end of file
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }

                        string line = reader.ReadLine();

                        // not sure if this is exactly correct because im lazy
                        // taking the character length of the string and converting to bytes
                        bytesReadSoFar += (ulong)Encoding.UTF8.GetByteCount(line + "\n");
                        linesCount++;

                        Match match = regex.Match(line);

                        if (match.Success)
                        {
                            // @TODO: Remove this line. Only for debugging purposes. Replace with file write or database put
                            //Debug.WriteLine(parseMatch(match));

                            // only report progress if reportInterval has been passed
                            if (bytesReadSoFar - bytesLastReportedAt > reportIntervalInBytes)
                            {
                                int percentageComplete = (int)((float)bytesReadSoFar / totalSizeInBytes * 100);
                                string progressMessage = $"Read {bytesReadSoFar / 1024} KB of {totalSizeInBytes / 1024} KB";
                                worker.ReportProgress(percentageComplete, progressMessage);
                                bytesLastReportedAt = bytesReadSoFar; // reset report interval
                            }
                        }
                    }
                }
            });

            sw.Stop();

            TimeSpan ts = sw.Elapsed;

            Debug.WriteLine("\n\n-------------------------------------------------------------------------------------\n");
            Debug.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            Debug.WriteLine("\n-------------------------------------------------------------------------------------\n\n");
        }


        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                OutputTextBox.Content = e.Error.Message; // @TODO: this almost definitely will not work
            }
            else if (e.Cancelled)
            {
                OutputTextBox.Content = "Canceled";
            }
            else
            {
                OutputTextBox.Content = e.Result?.ToString(); // @TODO: this won't work either
                DataProcessingProgress.Value = 0;
            }
            StartButton.IsEnabled = !backgroundWorker.IsBusy; // enable start button
            CancelButton.IsEnabled = backgroundWorker.IsBusy; // disable cancel button
        }


        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DataProcessingProgress.Value = e.ProgressPercentage;
            OutputTextBox.Content = e.UserState?.ToString(); // pass progressmessage from worker to UI thread
        }


        private string parseMatch(Match match)
        {
            //  Regex group matches being converted to variables that will eventually be sent to a database.
            string date = match.Groups["DATE"].Value;
            string time = match.Groups["TIME"].Value;
            string host = match.Groups["HOST"].Value;
            string procname = match.Groups["PROCNAME"].Value;
            int pid = Convert.ToInt32(match.Groups["PID"].Value);
            int logcode = Convert.ToInt32(match.Groups["LOGCODE"].Value);
            string type = match.Groups["TYPE"].Value;

            // A string that displays all of the variable from the regex group patterns.
            string parsedLogLine = "Date: " + date
                             + " Time: " + time
                             + " Host: " + host
                             + " Proc. Name: " + procname
                             + " PID: " + pid
                             + " Log Code: " + logcode
                             + " Type: " + type;

            return parsedLogLine;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
                return;
            Button button = sender as Button;
            FileInfo fileInfo = button.DataContext as FileInfo;

            Files.Remove(fileInfo);
        }
    }
}
