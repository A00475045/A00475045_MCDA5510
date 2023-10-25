using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Assignment1
{
    internal class DriverClass
    {

        public static void Main(string[] args)
        {
            string outputPath = "~\\..\\..\\..\\..\\Output\\Final Output.csv";
            string logsPath = "~\\..\\..\\..\\..\\logs\\Logs.txt";
            if (File.Exists(logsPath)) File.Delete(logsPath);
            using (StreamWriter logger = new StreamWriter(logsPath, true))
            {
                try
                {

                    if (File.Exists(outputPath)) File.Delete(outputPath);

                    Console.WriteLine("Execution started.. \nLOADING...");
                    RecursivePathSearch rps = new RecursivePathSearch();

                    var watch = Stopwatch.StartNew();
                    var csvPath = Path.Combine(outputPath);
                    var streamWriter = new StreamWriter(csvPath, true);

                    using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteHeader<Format>();
                        csvWriter.NextRecord();
                    }

                    rps.pathFinder(@"F:\SMU academics\5510\Assignments\MCDA5510_Assignments\Sample Data", logger);

                    watch.Stop();

                    TimeSpan elapsed = watch.Elapsed;
                    string formattedTime = $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";

                    Console.Clear();
                    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine($"The Execution of the Program was successful and in total it took {formattedTime}minutes to complete.");
                    Console.WriteLine($"There are a total of {DLL.validColumnCount} valid entries");
                    Console.WriteLine($"There are a total of {DLL.rowSkipped} inValid entries");
                    Console.WriteLine($"Total Entries(rows) processed Count is {DLL.rowSkipped + DLL.validColumnCount}");



                    logger.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    logger.WriteLine($"The Execution of the Program was successful and in total it took {formattedTime}minutes to complete.");
                    logger.WriteLine($"There are a total of {DLL.validColumnCount} valid entries");
                    logger.WriteLine($"There are a total of {DLL.rowSkipped} inValid entries");
                    logger.WriteLine($"Total Entries(rows) processed Count is {DLL.rowSkipped + DLL.validColumnCount}");
                }

                
                catch (Exception ex)
                {
                    logger.WriteLine(ex.Message);
                }
            }
        }
    }
}
