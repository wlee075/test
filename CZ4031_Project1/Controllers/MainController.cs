using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class MainController
    {
        static string workingDirectory = Environment.CurrentDirectory;
        static string MainDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\";
        public static string GetMainDirectory()
        {
            return MainDirectory;
        }
    }
}
