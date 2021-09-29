using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class RecordController
    {
        private static double RecordSize { get; set; }
        public static double TotalRecord { get; set; }
        public static void SetRecordSize(int tconst, int avgrating, int numvotes)
        {
            RecordSize = tconst + avgrating + numvotes;
        }
        public static double GetRecordSize()
        {
            return RecordSize;
        }
    }
}
