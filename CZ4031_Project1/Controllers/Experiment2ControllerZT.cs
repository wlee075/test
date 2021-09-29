using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    class Experiment2ControllerZT
    {
        List<string> lines { get; set; }
        string lineTabs { get; set; }
        int minKey = 5;
        int maxKey = 10;
        int index = 0;
        int level = 1;
        int increment = 1;
        Dictionary<byte[], Record>addresses = MemoryAddressController.GetAddressesForRecords();
        int count = 1070318;
        public void BuildBPlusTree()
        {
            
            lines = new List<string>();         

            while (increment  <= count)
            {
                PrintBlock();
                level += 1;

                //Level 2 onwards
                if (level == 2)
                {
                   increment = minKey;
                }

                //Level 3 onwards
                if (level >= 3)
                {                  
                    increment = increment * (minKey + 1);
                }
               
            }

        }
        public void PrintBlock()
        {
       
            string line = lineTabs;
            List<int> indexes = new List<int>();
            for (int i = 0; i < minKey; i++)
            {
                if (index < count)
                {
                    line += index + " ";
                    indexes.Add(index);
                    index += increment;
                }
            }
            lines.Add(line);
            
            line = lineTabs;
            foreach (var x in indexes)
            {
                line += addresses.ElementAt(x).Value.NumVotes + " ";
            }
            lines.Add(line);

            lineTabs += "\t";
            
        }
        public void Print()
        {
            var arrLines = lines.ToArray();
            Array.Reverse(arrLines);
            //List<string> lines = new List<string>();
            //foreach(var x in BlockController.BPlussTreeBlocks)
            //{
            //    foreach(var y in x.Value.Nodes)
            //    {
            //        lines.Add(String.Format("{0}: {1}", BitConverter.ToString(x.Key), BitConverter.ToString(y.Pointer)));
            //    }
               
            //}
            AccessFileController afcontroller = new AccessFileController(MainController.GetMainDirectory() + "experiment2_stored_data.txt");
            afcontroller.Write(arrLines.ToList());
        }
    }
}
