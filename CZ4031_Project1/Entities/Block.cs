using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CZ4031_Project1.Entities
{
    public class Block
    {
       // public bool IsRecordBlock { get; set; }
        public string Id { get; set; }
        public byte[] Address { get; set; }
        public byte[] Pointer { get; set; }
        
        //public Block Parent { get; set; }
        //public Block Next { get; set; }
       // public Block Previous { get; set; }
        //public List<Block> children { get; set; }
        
        public List<Node> Nodes { get; set; }

    }
}
