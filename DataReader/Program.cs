using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DataReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //const string FILENAME = "wifi_data.log";
            const string FILENAME = "wireless_01-10-2021.log";
            const string FILENAME2 = "filtered3.log";
            //const string FILENAME =
            //@"C:\Users\Will Robinson\Documents\UNCC\ITSC 4155\DataCollection\DataReader\DataReader\bin\Debug\net5.0\wifi_data.log";

            if (File.Exists(FILENAME))
            {
                FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
                FileStream outFile = new FileStream(FILENAME2, FileMode.Create, FileAccess.Write);

                StreamReader reader = new StreamReader(inFile);
                StreamWriter writer = new StreamWriter(outFile);

                try 
                {                    
                    string line;

                    Regex regex = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+"
                                          + @"(?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s(?<HOST>\S+)\s+"
                                          + @"(?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+"
                                          + @"<(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?"
                                          + @"<(?<TYPE>[a-zA-Z]+)>");

                    Regex regex2 = new Regex(@"<(?<LOGCODE>5[0-9]{5})>");
                    Regex regex3 = new Regex(@"AP\s(?<AP>[EXT-]+[a-zA-z0-9]+|[a-zA-Z0-9]+)-");

                    Regex regex4 = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+"
                                          + @"(?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s(?<HOST>\S+)\s+"
                                          + @"(?<PROCNAME>[a-zA-Z0-9\-]+)\[(?<PID>[^\]]*)\]:\s+"
                                          + @"<(?<LOGCODE>[0-9]{6})>\s+(?:<(\d*)>\s+)?"
                                          + @"<(?<TYPE>[a-zA-Z]+)>\s+"
                                          + @"\|AP\s(?<AP>[EXT-]+[a-zA-z0-9]+|[a-zA-Z0-9]+)-");

                    while ((line = reader.ReadLine()) != null)
                    {    
                        Match match = regex2.Match(line);
                        Match match2 = regex3.Match(line);
                        Match match3 = regex4.Match(line);

                        if (match3.Success)
                        {
                            /*
                            
                            //Used for storing groups into variables.
                            string date = match3.Groups["DATE"].Value;
                            string time = match3.Groups["TIME"].Value;                            
                            int logcode = Convert.ToInt32(match3.Groups["LOGCODE"].Value);
                            string access_point = match3.Groups["AP"].Value;

                            string logLine = "Date: " + date + " Time: " + time +
                                                     " Log Code: " + logcode + " Access Point: " + access_point;

                            writer.WriteLine(logLine);
                            */
                            writer.WriteLine(line);
                        }
                        //Console.WriteLine(match.Success.ToString());
                        //Console.WriteLine(line);
                    }
                    Console.WriteLine("Written to file successfully!!!");
                }
                catch(System.IO.IOException exc)
                {
                    Console.WriteLine(exc.Message.ToString());
                }
                finally
                {
                    reader.Close();
                    writer.Close();
                    inFile.Close();
                    outFile.Close();
                }
            }
            else
                Console.WriteLine("File does not exist...");
            /*
            FileStream fs = File.Open(FILENAME, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BufferedStream bs = new BufferedStream(fs);
            StreamReader sr = new StreamReader(bs);

            string line;// = sr.ReadLine();

            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
            */
        }
    }
}
