using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using System.Data;
using NUnit.Framework;

namespace TestFramework
{
    using DataRow = Dictionary<string, double>;
    
    public class ModFileData
    {
        private List<DataRow> m_Data;

        public ModFileData()
        {
            m_Data = new List<DataRow>();
        }

        public bool ParseCsv(string path)
        {
            Console.WriteLine("Reading modfile at: " + path);
            
            // read all lines from the file
            string[] lines = System.IO.File.ReadAllLines(path);   

            int index = 0;

            if (lines[0].StartsWith("#"))
            {
                index = 1;
            }

            string headers_line = lines[index];
            string[] headers = headers_line.Split(',');

            foreach(string header in headers)
            {
                // check if the headers found in the mod file match the ones defined in Types.ModFileHeader class
                if (Utils.IsValidHeader(header) == false)
                {
                    Console.Error.WriteLine("Column '{0}' is not defined in Types.ModFileHeaders class.", header);
                    return false;
                }
            }

            index ++;

            for(int line_idx = index ; line_idx < lines.Length ; line_idx++)
            {
                // Read data 
                DataRow row = new DataRow();

                string line = lines[line_idx];
                string[] fields = line.Split(',');

                if(headers.Length != fields.Length)
                {
                    Console.Error.WriteLine("headers.Length != fields.Length");
                    return false;
                }

                var headersAndFields = headers.Zip(fields, (h, f) => new { Header = h, Field = f });
                foreach(var hf in headersAndFields)
                {
                    double field_val = 0;

                    try 
                    {
                        field_val = Convert.ToDouble(hf.Field);
                    }
                    catch (FormatException) 
                    {
                        field_val = 0;
                        // Console.Error.WriteLine("Unable to convert field '{0}' value: '{1}' to a Double. Assigning value '0'.", hf.Header, hf.Field);
                    }
                    catch (OverflowException) 
                    {
                        Console.Error.WriteLine("field '{0}' value: '{1}' is outside the range of a Double.", hf.Header, hf.Field);
                        return false;
                    }
                    row.Add(hf.Header, field_val);
                }
                m_Data.Add(row);
                
            }
            Console.WriteLine("Lines parsed: {0}", m_Data.Count);
            return true;
        }
    }
}