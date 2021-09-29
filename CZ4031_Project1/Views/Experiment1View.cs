using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment1View
    {
        static Experiment1Controller controller = new Experiment1Controller();
        public static void Display()
        {
            string directory = controller.GetDirectory();
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Store the data from {0}", directory);
            Console.WriteLine("2. Show statistics");
            Console.WriteLine("3. Back to main page");
            Console.WriteLine("4. Exit");

           
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    controller.StoreData();
                    break;
                case "2":
                    controller.ShowStatistics();
                    break;
                case "3":
                    Views.MainView.Display();
                    break;
                case "4":
                    //Environment.Exit(0);
                    //Testing
                    foreach (var x in BlockController.Blocks)
                    {
                        Console.WriteLine("{0}: {1}", BitConverter.ToString(x.Key), x.Value.Id);
                    }
                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    Display();
                    break;
            }
            Display();
        }
    }
}
