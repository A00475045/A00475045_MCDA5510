using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;
using System.Threading.Tasks;
using System.IO;
using CsvHelper.Configuration;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Assignment1
{
    internal class RecursivePathSearch
    {

        public void pathFinder(string path, StreamWriter logger)
        {

            
            string[] dirPathsList = Directory.GetDirectories(path);

            if (dirPathsList == null) return;
                logger.WriteLine(path);
                logger.WriteLine();
            
                try
                {
                    foreach (string list in dirPathsList)
                    {
                        if(!((File.GetAttributes(list) & FileAttributes.Hidden) == FileAttributes.Hidden))
                          pathFinder(list,logger);
                    }
                    string[] filePathsList = Directory.GetFiles(path);
                    /* here we are finding the Date component from the path by splitting the path with '\' and then we store each component of date to its corresponding variable
                     * this date finding assumes that the path is always in 
                     * "xyz/abc/yyyy/mm/dd/file.csv"
                     
                     */
                    string[] pathComponents = path.Split('\\');
                    int yyyy = 0;
                    int mm = 0;
                    int dd = 0;

                    foreach (string component in pathComponents)
                        if (int.TryParse(component, out int datePart))
                            {
                                if (yyyy == 0 && datePart <= 2070) yyyy = datePart;
                                else if (mm == 0 && datePart < 13) mm = datePart;
                                else if (dd == 0 && datePart < 32) dd = datePart;
                            }
                    

                    DLL dll = new DLL();
                    foreach (string list in filePathsList)
                    {
                       if (yyyy == 0 || mm == 0 || dd == 0)
                        {
                            throw new Exception("The Path does not contain a correct Date!!");
                        }
                    if ((Path.GetExtension(list).Equals(".csv", StringComparison.OrdinalIgnoreCase) && Path.GetFileNameWithoutExtension(list).StartsWith("CustomerData", StringComparison.OrdinalIgnoreCase)))
                     { 

                    List<Format> validEntries = dll.getDataFromFile(@list,new DateTime(yyyy,mm,dd),logger);
                    
                    var csvPath = Path.Combine(@"~\..\..\..\..\Output\Final Output.csv"); // $$$$$$$

                    var doesFileExists = File.Exists(csvPath);
                    var streamWriter = new StreamWriter(csvPath, true);

                    if (doesFileExists) // $$$$$$$
                    {
                        var configPersons = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HasHeaderRecord = false
                        };
                        using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, configPersons))
                        {
                            csvWriter.WriteRecords(validEntries);
                        }
                    }
                    else
                    {

                        using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                        {
                            csvWriter.WriteHeader<Format>();
                            csvWriter.NextRecord();
                            csvWriter.WriteRecords(validEntries);
                        }
                    }
                    streamWriter.Close();
                    }
                }
            }
                catch (Exception ex)
                {
                    logger.WriteLine(ex.Message);
                }
            
                logger.WriteLine("============================================================================================================================");
        }

       
    }
}
