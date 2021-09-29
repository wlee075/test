using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment1Controller
    {
        string Directory = MainController.GetMainDirectory() + "data.tsv";
        const int blockSize = 100;
        const int blockAddress = 8;


        int availableSpace = blockSize - blockAddress;

        int tconstSize = 0;
        int avgratingSize = 0;
        int maxAverageRating = 0;
        int numvoteSize = 0;
        public string GetDirectory()
        {
            return Directory;
        }
        public void StoreData()
        {
            //Read from original dataset
            AccessFileController afController = new AccessFileController(Directory);
            List<Record> records = afController.ReadAndConvertToRecords();

            //Get the minimum and maximum length of each fields
            int minTconst = records.Select(z => z.Tconst).Min().Count();
            int maxTconst = records.Select(z => z.Tconst).Max().Count();
            int minAverageRating = records.Select(z => z.AverageRating.ToString()).Min().Count();
            int maxAverageRating = records.Select(z => z.AverageRating.ToString()).Max().Count();
            int minNumVotes = records.Select(z => z.NumVotes.ToString()).Min().Count();
            int maxNumVotes = records.Select(z => z.NumVotes.ToString()).Max().Count();
            RecordController.TotalRecord = records.Count();
            //Save the size of each field
            tconstSize = maxTconst;
            avgratingSize = maxAverageRating - 1; //Because of float
            numvoteSize = 4; //Integer size
            RecordController.SetRecordSize(tconstSize, avgratingSize, numvoteSize);
            Console.WriteLine("Min length of tconst: {0}", minTconst);
            Console.WriteLine("Max length of tconst: {0}", maxTconst);
            Console.WriteLine("Max length of averageRating: {0}", minAverageRating);
            Console.WriteLine("Max length of averageRating: {0}", maxAverageRating);
            Console.WriteLine("Max length of numVotes: {0}", minNumVotes);
            Console.WriteLine("Max length of numVotes: {0}", maxNumVotes);
            //Sort records by numVotes       
            records = records.OrderBy(z => z.NumVotes).ToList();
            //Saving records
            Console.WriteLine("Storing records...");
            SaveRecords(maxTconst, maxAverageRating - 1, 4, records);
            Console.WriteLine("Records stored");
            //Print memoryaddresses into textfile
            PrintMemoryAddresses();
            Console.WriteLine("Total number of records: {0}", RecordController.TotalRecord);

        }
        public void PrintMemoryAddresses()
        {
            List<string> lines = new List<string>();
            foreach (var memoryaddress in MemoryAddressController.GetAddresses())
            {
                lines.Add(String.Format("{0}: {1}", BitConverter.ToString(memoryaddress.Key), memoryaddress.Value));
            }
            //var memoryaddress = MemoryAddressController.GetAddressesForRecords().ElementAt(1);
            //lines.Add(String.Format("{0}: {1}", BitConverter.ToString(memoryaddress.Key), memoryaddress.Value.NumVotes));
            AccessFileController afcontroller = new AccessFileController(MainController.GetMainDirectory() + "experiment1_stored_data.txt");
            afcontroller.Write(lines);
        }
        private void SaveRecords(int tconst, int avgrating, int numvotes, List<Record> records)
        {
    
            foreach (Record r in records)
            {
                 MemoryAddressController.InsertRecordIntoMemory(r);
            }

            //uncomment later
           // BlockController.InsertBlockIntoMemory();
        }


        public void ShowStatistics()
        {
            int recordSize = (int)RecordController.GetRecordSize();
            if (recordSize != 0)
            {
                decimal blockOffsetSize = Convert.ToDecimal(BlockController.GetBlockOffsetSize());
                decimal recordsPerBlock = Convert.ToDecimal(availableSpace) / (recordSize + blockOffsetSize);
                recordsPerBlock = Math.Floor(recordsPerBlock);
                decimal blockHeaderSize = blockAddress + blockOffsetSize * recordsPerBlock;
                decimal totalBlocks = Convert.ToDecimal(RecordController.TotalRecord) / recordsPerBlock;
                totalBlocks = Math.Ceiling(totalBlocks);
                decimal sizeOfDatabase = totalBlocks * blockSize;
                Console.WriteLine("Record size: {0} bytes", recordSize);
                Console.WriteLine("Number of records per block: {0}", recordsPerBlock);
                Console.WriteLine("Block address size: {0} bytes", blockAddress);
                Console.WriteLine("Block offset size: {0} bytes", blockOffsetSize);
                Console.WriteLine("Block header size: {0} bytes", blockHeaderSize);
                Console.WriteLine("Total Number of blocks: {0}", totalBlocks);
                Console.WriteLine("Size of Database: {0} bytes", sizeOfDatabase);
            }
            else
            {
                Console.WriteLine("Please store data first.");
            }
        }
    }
}
