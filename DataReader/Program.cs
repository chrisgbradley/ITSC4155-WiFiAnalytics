using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace DataReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string FILENAME = "wireless_01-01-2021.log";
           // const string FILENAME = "test-file.log";
            //const string FILENAME2 = "filtered4.log";
            const string FILE_CSV = "testf.csv";
            const string FIRST_LINE = "Date,Time,Log Code,Access Point,Client MAC,AP MAC,Error Description";
            

            if (File.Exists(FILENAME))
            {
                FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
                FileStream outFile = new FileStream(FILE_CSV, FileMode.Create, FileAccess.Write);

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
                                          + @"\|AP\s(?<AP>[EXT-]+[a-zA-z0-9]+|[a-zA-Z0-9]+).+"
                                          + @"?(?<CMAC>([a-fA-F0-9]{2}:){5}([a-fA-F0-9]{2}))(.*)?(?<HMAC>([a-fA-F0-9]{2}:){5}([a-fA-F0-9]{2})).+"
                                          + @"(-(.*)-1\s?(?<ERRDSC>(.*)))");


                    Regex dateReg = new Regex(@"(?<DATE>[a-zA-Z]+\s+(0?[1-9]|[12][0-9]|3[01]))\s+");
                    Regex timeReg = new Regex(@"(?<TIME>[0-9]{2}:[0-9]{2}:[0-9]{2})\s");
		            Regex macReg = new Regex(@"(?<MAC>([a-fA-F0-9]{2}:){5}([a-fA-F0-9]{2}))");
		

                    String logline;
                    String csvLine;

                    if ((line = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(FIRST_LINE);
                    }

                    while ((line = reader.ReadLine()) != null)
                    {    
                        Match match = regex2.Match(line);
                        Match match2 = regex3.Match(line);
                        Match match3 = regex4.Match(line);

                        Match matchReg = macReg.Match(line);

                        if (match3.Success)
                        {
                            
                            
                            //Used for storing groups into variables.
                            string date = match3.Groups["DATE"].Value;
                            string time = match3.Groups["TIME"].Value;                            
                            int logcode = Convert.ToInt32(match3.Groups["LOGCODE"].Value);
                            string access_point = match3.Groups["AP"].Value;
                            string host_mac = match3.Groups["HMAC"].Value;
                            string client_mac = match3.Groups["CMAC"].Value;
                            string error_desc = match3.Groups["ERRDSC"].Value;

                            //logline = "date: " + date + " time: " + time +
                            //                        " log code: " + logcode + " access point: " + access_point +
                            //                        " client mac:  " + client_mac + " ap mac: " + host_mac;

                            csvLine = date+ "," + time + "," + logcode + "," + access_point + "," + client_mac + "," + host_mac + "," + error_desc;

                            writer.WriteLine(csvLine);
                            
                            //writer.WriteLine(line);
                        }

                        

                        //Console.WriteLine(match.Success.ToString());
                        //Console.WriteLine(line);
                    }

                    Console.WriteLine("Written to file successfully!!!");
                    //Console.ReadLine();
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


        //microsoft docs copypaste, haven't done anything with this yet
        /*
        public void uploadCSV(string connectionString, string insertSQL)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                // The insertSQL string contains a SQL statement that
                // inserts a new row in the source table.
                OleDbCommand command = new OleDbCommand(insertSQL);

                // Set the Connection to the new OleDbConnection.
                command.Connection = connection;

                // Open the connection and execute the insert command.
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // The connection is automatically closed when the
                // c
            }
        }
        */
    }
}
