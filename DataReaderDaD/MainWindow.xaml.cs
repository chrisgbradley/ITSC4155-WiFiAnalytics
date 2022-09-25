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

namespace DataReaderDaD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow1_DragEnter(object sender, DragEventArgs e)
        {
            // Basically used to prime the window for dragging and dropping.
            e.Effects = DragDropEffects.All;
        }

        private void MainWindow1_Drop(object sender, DragEventArgs e)
        {
            // Creates a string array to collect lines from a document.
            string[] getItems = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
            
            //  Creats a string list to store each line from the string array.
            List<string> lines = File.ReadAllLines(getItems[0]).ToList();

            /*
            This was the source of my problem.  Is there a way to break a Regex pattern into separate lines?
            Regex regex = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+
                                      (?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s(?<HOST>\S+)\s+
                                      (?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+
                                     <(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?<(?<TYPE>[a-zA-Z]+)>");
            */
            Regex regex = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+(?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s(?<HOST>\S+)\s+(?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+<(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?<(?<TYPE>[a-zA-Z]+)>");

            // Foreach loop that will read every line from the file that is now stored in a list.
            foreach (string line in lines) 
            {
                // Used to match each line with the regex pattern from above.
                Match match = regex.Match(line);
                
                // Can't get this to only print matched variables.
                // Problem solved.
                // If the match is successful, everyting within the if statement will be performed.
                if (match.Success)
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
                    string logLine = "Date: " + date + " Time: " + time + 
                                     " Host: " + host + " Proc. Name: " + procname + 
                                     " PID: " + pid +" Log Code: " + logcode + " Type: " + type;
                    
                    // The string with the variables in it being sent to the list box for display.
                    DragDrobListbox.Items.Add(logLine);
                }
                
                // This was used to make sure that the drag and drop was working.
                //DragDrobListbox.Items.Add(line);
            }
        }
    }
}
