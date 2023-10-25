using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Assignment1
{
    public class Format
    {
        [Name("First Name")]
        public string firstName { get; set; }
        
        [Name("Last Name")]
        public string lastName { get; set; }
        
        [Name("Street Number")]
        public int streetNumber { get; set; }
        
        [Name("Street")]
        public string street { get; set; }
        
        [Name("City")]
        public string city { get; set; }
        [Name("Province")]
        public string province { get; set; }
        [Name("Postal Code")]
        public string postal { get; set; }
        [Name("Country")]
        public string country { get; set; }
        [Name("Phone Number")]
        public int phoneNumber { get; set; }
        [Name("email Address")]
        public string email { get; set; }

        [Name("date data column")]
        [Optional]
        public string date { get; set; }


    }
    internal class DLL
    {
            public static int rowSkipped = 0;
            public static int validColumnCount = 0;
        
        public bool IsValid(string emailaddress)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailaddress);
            if (match.Success)
                return true;
            else
                return false;
        }

        public bool isEntryValid(Format record)
        {
            if (
                        !IsValid(record.email) || String.IsNullOrEmpty(record.firstName) || String.IsNullOrEmpty(record.lastName) || String.IsNullOrEmpty(record.streetNumber.ToString()) ||
                                        String.IsNullOrEmpty(record.street) || String.IsNullOrEmpty(record.city) || String.IsNullOrEmpty(record.province) || String.IsNullOrEmpty(record.postal) || String.IsNullOrEmpty(record.country) || String.IsNullOrEmpty(record.phoneNumber.ToString()) || String.IsNullOrEmpty(record.email)
                                        )
            {
                DLL.rowSkipped++;
                return false;
            }
            else
            {
                DLL.validColumnCount++;
                return true;
            }
        }

        public List<Format> getDataFromFile( string fileName , DateTime date, StreamWriter logger)
        {
                List<Format> validEntries = new List<Format>();
                int __rowSkipped = rowSkipped;
                int __validColumnCount = validColumnCount;
                logger.WriteLine("     "+fileName);
                logger.WriteLine();

                try
                {
                        using (var streamReader = new StreamReader(fileName))
                        {
                            using (var csvReader = new CsvReader(streamReader, System.Globalization.CultureInfo.InvariantCulture))
                            {
                                var records = csvReader.GetRecords<Format>().ToList();
                                foreach (var record in records)
                                {
                                    if(isEntryValid(record))
                            {
                                record.date = date.ToString("yyyy/MM/dd");
                                validEntries.Add(record);
                            }
                                    
                                }

                            }
                        }
                } 
                catch 
                {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                using var reader = new CsvReader(new StreamReader(fileName), config);
                var validRecords = new List<Format>();
                while (reader.Read())
                {
                    try
                    {
                        Format record = reader.GetRecord<Format>();
                        if (isEntryValid(record))
                        {
                            record.date = date.ToString("yyyy/MM/dd");
                            validRecords.Add(record);

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.WriteLine(ex.Message);
                        DLL.rowSkipped++;
                    }


                }
                return validRecords;
            }
            logger.WriteLine("     Number of valid rows: " + (DLL.validColumnCount - __validColumnCount).ToString());
            logger.WriteLine("     Number of inValid rows: " + (DLL.rowSkipped - __rowSkipped).ToString());
            logger.WriteLine("     ------------------------------------------------------------------------------------------------------------");
            return validEntries;
        }
    }
}
