using CZ4031_Project1.Controllers;
using CZ4031_Project1.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CZ4031_Project1.Views
{
    public static class Experiment2View
    {
        static BPlusTree tree { get; set; }

        static Experiment2Controller controller2 = new Experiment2Controller();

        public static void Display()
        {
            Console.WriteLine("Please key in your selection: ");
            Console.WriteLine("1. Build the B+ Tree by numVotes");
            Console.WriteLine("2. Show statistics");
            Console.WriteLine("3. Back to main page");
            Console.WriteLine("4. Exit");

            tree = new BPlusTree();

            string input = Console.ReadLine();
            switch (input)
            { 
                case "1":
                    controller2.BuildTree(tree);
                    BPlusTreeController.PrintTree(tree);
                    break;
                case "2":
                    Console.WriteLine("Parameter n of the B+ tree    : {0} ", BPlusTreeController.GetMaxKeys());
                    Console.WriteLine("Height of the B+ tree    : {0} ", BPlusTreeController.Levels);
                    Console.WriteLine("Number nodes of the B+ tree    : {0} ", BPlusTreeController.numNodes);
                    break;
                case "3":
                    Views.MainView.Display();
                    break;
                case "4":
                    Environment.Exit(0);
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
