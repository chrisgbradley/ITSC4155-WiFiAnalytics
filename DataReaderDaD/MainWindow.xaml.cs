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
using System.Data.OleDb;
using System.Windows.Markup;
using System.Globalization;
using System.Collections.Concurrent;
using FastMember;
using System.Data.SqlClient;

namespace DataReaderDaD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Regex lineRegex = new Regex(@"(?<DATE>(?<MONTH>[a-zA-Z]+)\s+(?<DAY>0?[1-9]|[12][0-9]|3[01]))\s+"
                                                   + @"(?<TIME>(?<HOUR>[0-9]{2}):(?<MINUTE>[0-9]{2}):(?<SECOND>[0-9]{2}))\s(?<HOST>\S+)\s+"
                                                   + @"(?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+"
                                                   + @"<(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?"
                                                   + @"<(?<TYPE>[a-zA-Z]+)>"
                                                   + @"\s+(.*?)\s+((?<HOSTNAME>[a-zA-Z0-9\-]+)[\@\s])?(?<IPADDRESS>(?:[0-9]{1,3}.){3}[0-9]{1,3})");

        private readonly Regex yearRegex = new Regex(@".\d{2}-\d{2}-(?<YEAR>\d{4}).");

        private readonly int BULKCOPY_THRESHOLD = 250000;

        /* 
         * Connection String
         * Data Source = WINSVR2019; Initial Catalog = NINERFISTAGING; Integrated Security = True
         */

        //  @"(?<TIME>(?<HOUR>[0-9]{2}):(?<MINUTE>[0-9]{2}):(?<SECOND>[0-9]{2}))\s(?<HOST>\S+)\s+"
        //  @".\d{2}-\d{2}-(?<YEAR>\d{4})."
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
            OutputTextBox.Content = string.Empty;
        }


        private void MainWindow1_Drop(object sender, DragEventArgs e)
        {
            // dont allow drops while parsing
            if (backgroundWorker.IsBusy)
                return;

            // file paths of files dropped in
            string[] filesThatWereDropped = (string[])(e.Data.GetData(DataFormats.FileDrop, true));

            string logExtension = ".log";

            // add all paths to listbox in GUI
            // checks file extension and removes files that are not .log extension
            foreach (string file in filesThatWereDropped)
            {
                FileInfo nFile = new FileInfo(file);

                Debug.WriteLine(file);
                if (!Files.Any(o => o.FullName == file))
                {
                    Files.Add(nFile);
                }


                if (!file.EndsWith(logExtension))
                {
                    Files.Remove(nFile);
                    OutputTextBox.Content = "An invalid filetype was dropped and will not be uploaded";
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
                /*
                string destinationDirectory = "C:\\Users\\willr\\Documents\\CSV";  //  Comment this out when done.

                if (!Directory.Exists(destinationDirectory)) //  Comment this out when done.
                    Directory.CreateDirectory(destinationDirectory); //  Comment this out when done.
                
                //  Comment this out when done.
                using (StreamWriter writer = new StreamWriter(System.IO.Path.ChangeExtension(System.IO.Path.Combine(destinationDirectory, System.IO.Path.GetFileName(file.Name)), ".csv"), true, Encoding.UTF8))*/
                Match yearMatch = yearRegex.Match(file.FullName);
                int year = yearMatch.Success == true ? Convert.ToInt32(yearMatch.Groups["YEAR"].Value) : 9999;

                using (StreamReader reader = new StreamReader(file.FullName, Encoding.UTF8, true, BufferSize))
                {
                    List<LogEntry> lines = new List<LogEntry>();

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

                        Match lineMatch = lineRegex.Match(line);


                        if (lineMatch.Success)
                        {
                            //writer.WriteLine(match.Groups["HOST"].Value); // Comment this out when done.
                            string month = lineMatch.Groups["MONTH"].Value;
                            int day = Convert.ToInt32(lineMatch.Groups["DAY"].Value);
                            string time = lineMatch.Groups["TIME"].Value;
                            int hour = Convert.ToInt32(lineMatch.Groups["HOUR"].Value);
                            int minute = Convert.ToInt32(lineMatch.Groups["MINUTE"].Value);
                            int second = Convert.ToInt32(lineMatch.Groups["SECOND"].Value);
                            string host = lineMatch.Groups["HOST"].Value;
                            string procname = lineMatch.Groups["PROCNAME"].Value;
                            int pid = Convert.ToInt32(lineMatch.Groups["PID"].Value);
                            int logcode = Convert.ToInt32(lineMatch.Groups["LOGCODE"].Value);
                            string hostname = lineMatch.Groups["HOSTNAME"].Value;
                            string ipaddress = lineMatch.Groups["IPADDRESS"].Value;
                            string description = " "; //  Comment out if it doesn't work.
                            string type = lineMatch.Groups["TYPE"].Value;

                            DateTime fullDate = new DateTime(year, DateTime.ParseExact(month, "MMM", CultureInfo.CurrentCulture).Month, day, hour, minute, second);

                            //LogEntry entry = new LogEntry(fullDate, host, procname, pid, logcode, type);
                            LogEntry entry = new LogEntry(fullDate, hostname, ipaddress, procname, pid, logcode, description, type);

                            lines.Add(entry);
                        }

                        // only report progress if reportInterval has been passed
                        if (bytesReadSoFar - bytesLastReportedAt > reportIntervalInBytes)
                        {
                            int percentageComplete = (int)((float)bytesReadSoFar / totalSizeInBytes * 100);
                            string progressMessage = $"Read {bytesReadSoFar / 1024} KB of {totalSizeInBytes / 1024} KB";
                            worker.ReportProgress(percentageComplete, progressMessage);
                            bytesLastReportedAt = bytesReadSoFar; // reset report interval
                        }
                        if (lines.Count >= BULKCOPY_THRESHOLD)
                        {
                            addToDatabase(lines);
                            lines.Clear();
                        }
                    }
                    addToDatabase(lines);
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

        private void addToDatabase(List<LogEntry> lines)
        {
            string connString;

            ///connString = @"Server=localhost; Database=NINERFISTAGING; Integrated Security=True";
            connString = @"Server=localhost; Database=NINERFISTAGINGTEST; Integrated Security=True";

            // SEE BOTTOM FOR BLOCK

            using (SqlConnection destinationConnection =
                   new SqlConnection(connString))
            {
                destinationConnection.Open();



                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connString))
                using (var reader = ObjectReader.Create(lines, "EntryTimestamp", "Hostname", "IPAddress", "ProcessName"
                                                          , "ProcessNumber", "LogCode", "Description"
                                                          , "EntryTypeName"))
                {
                    bulkCopy.DestinationTableName = "dbo.STAGING";

                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("EntryTimestamp", "EntryTimestamp"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Hostname", "Hostname"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("IPAddress", "IPAddress"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProcessName", "ProcessName"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ProcessNumber", "ProcessNumber"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LogCode", "LogCode"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Description", "Description"));
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("EntryTypeName", "EntryTypeName"));

                    bulkCopy.BatchSize = 50000;
                    bulkCopy.BulkCopyTimeout = 0;


                    try
                    {
                        bulkCopy.WriteToServer(reader);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                    finally
                    {
                        reader.Close();
                    }

                }
            }

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
                return;
            Button button = sender as Button;
            FileInfo fileInfo = button.DataContext as FileInfo;

            Files.Remove(fileInfo);
        }

        struct LogEntry
        {
            public DateTime EntryTimestamp;
            public string Hostname;
            public string IPAddress;
            public string ProcessName;
            public int ProcessNumber;
            public int LogCode;
            public string Description; //  Comment out if all goes wrong.
            public string EntryTypeName;

            //public LogEntry(DateTime date, string hostname, string procname, int pid, int log, string entrytype)
            public LogEntry(DateTime date, string hostname, string ipaddress, string procname, int pid, int log, string description, string entrytype)
            {
                this.EntryTimestamp = date;
                this.Hostname = hostname;
                this.IPAddress = ipaddress;
                this.ProcessName = procname;
                this.ProcessNumber = pid;
                this.LogCode = log;
                this.Description = description;
                this.EntryTypeName = entrytype;
            }
        }
    }
}