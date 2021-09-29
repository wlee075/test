using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CZ4031_Project1.Entities;

namespace CZ4031_Project1.Controllers
{
    public class AccessFileController
    {
        string Directory { get; set; }
        public AccessFileController(string d)
        {
            Directory = d;
        }
        public string Read()
        {
            string line = "";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Directory);
                //Read the second line of text
                sr.ReadLine();
                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            return line;
        }
        public List<Record> ReadAndConvertToRecords()
        {
            string line = "";
            List<Record> records = new List<Record>();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Directory);
                //Read the second line of text
                sr.ReadLine();
                line = sr.ReadLine();
                Console.WriteLine("Reading from {0} ..." , Directory);
                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the line to console window
                    string[] data = line.Split('\t');
                    Record r = new Record();
                    r.Tconst = data[0];
                    r.AverageRating = decimal.Parse(data[1]);
                    r.NumVotes = int.Parse(data[2]);
                    records.Add(r);
                    //Read the next line
                    line = sr.ReadLine();
                }
                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Reading from {0} complete", Directory);
            }
            return records;
        }
        public void Write(List<string> lines)
        {
            Console.WriteLine("Writing to {0}...", Directory);
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Directory);

                foreach(var line in lines)
                {
                    //Write a line of text
                    sw.WriteLine(line);
                }


                //Close the file
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Writing to {0} complete", Directory);
            }
        }
    }
}
