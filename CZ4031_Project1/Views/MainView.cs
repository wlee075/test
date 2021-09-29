using CZ4031_Project1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class MainView
    {
        public static void Display()
        {
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Experiment 1");
            Console.WriteLine("2. Experiment 2");
            Console.WriteLine("3. Experiment 3");
            Console.WriteLine("4. Experiment 4");
            Console.WriteLine("5. Exit");

            
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Views.Experiment1View.Display();
                    break;
                case "2":
                    //Views.Experiment2View.Display();
                    break;
                case "3":
                    Experiment1Controller exp1 = new Experiment1Controller();
                    Experiment2ControllerZT exp2 = new Experiment2ControllerZT();
                    exp1.StoreData();
                    Console.WriteLine("Building B-Plus Tree...");
                    exp2.BuildBPlusTree();
                    Console.WriteLine("Building B-Plus Tree Complete");
                    exp2.Print();
                    break;
                case "4":
                    
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid selection, please try again.");
                    break;
            }
            Display();
        }
    }
}
