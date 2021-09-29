using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public class Experiment2Controller
    {
        public static int IndexOfRecordToBeInserted { get; set; }
        public static Block CurrentRecordBlock = BPlusTreeController.CurrentRecordBlock;
        public BPlusTree BuildTree(BPlusTree tree)
        {
            
            var addresses = MemoryAddressController.GetAddresses().ToArray();
            IndexOfRecordToBeInserted = 0;
            //while (IndexOfRecordToBeInserted < addresses.Count())
             while (IndexOfRecordToBeInserted < 200)
                {
                int recordToBeInserted = Convert.ToInt32(addresses[IndexOfRecordToBeInserted].Value.Split('-')[1]);
                byte[] addressofRecordToBeInserted = addresses[IndexOfRecordToBeInserted].Key;
                
                BPlusTreeController.insert(tree, recordToBeInserted, addressofRecordToBeInserted);

                IndexOfRecordToBeInserted += 1;
            }

            return tree;
        }

    }
}
