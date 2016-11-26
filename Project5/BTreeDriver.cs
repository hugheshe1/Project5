//	Project:		Project 5 - BTree
//	File Name:		BTreeDriver.cs
//	Description:	
//	Course:			CSCI 2210-001 - Data Structures
//	Authors:		Reed Jackson, reedejackson@gmail.com, jacksonre@etsu.edu
//                  Other Author
//                  Other Author
//	Created:		11/23/2016
//	Copyright:		Reed Jackson, Author, Author, 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project5
{
    class BTreeDriver
    {
        private static Random rand = new Random();
        static void Main()
        {
            ConsoleStartUp();
            
            //Welcome message

            //Main Menu

            //Set arity and create B-Tree
            //User set arity
            int arity = 3;
            BTree userTree = new BTree(arity);

            bool getNextValue = false;
            for (int i = 1; i < 20; i++) //20 = 500
            {
                while (getNextValue == false)
                {
                    getNextValue = userTree.AddedValue(rand.Next(100)); //0 - 9999
                }
                getNextValue = false;
            }

            WriteLine("Enter node to display");
            string nodeStr = ReadLine();
            try
            {
                int node = int.Parse(nodeStr);
                userTree.FindNode(node);
                WriteLine  
            }
            catch (Exception)
            {
                //Messed up
            }
        }

        #region Console Edit

        /// <summary>
        /// Used to edit Console Start Up
        /// </summary>
        private static void ConsoleStartUp()
        {
            Title = $"Registering Simulation";
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
            Clear();
        }

        /// <summary>
        /// Used to inform the user the press a key to 
        /// continue the program
        /// </summary>
        public static void BreakLine()
        {
            WriteLine($"\n----------------------------------------------------------------------\n" +
                      $"Press any key to continue...\n" +
                      $"----------------------------------------------------------------------\n\n\n\n");
            ReadKey();
        }

        /// <summary>
        /// Used to add a separation line for easier reading
        /// </summary>
        public static void SeparationLine()
        {
            WriteLine($"----------------------------------------------------------------------\n\n");
        }

        #endregion
    }
}
