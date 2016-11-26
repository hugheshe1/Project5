//	Project:		Project 5 - BTree
//	File Name:		BTreeDriver.cs
//	Description:	A BTreeDriver class for displaying BTree info
//	Course:			CSCI 2210-001 - Data Structures
//	Authors:		Reed Jackson, reedejackson@gmail.com, jacksonre@etsu.edu
//                  Haley Hughes, hugheshe1@etsu.edu
//                  Other Author
//	Created:		11/23/2016
//	Copyright:		Reed Jackson, Haley Hughes, Author, 2016

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

        #region Properties
        private static Random rand = new Random();
        private static string Menu = string.Empty;
        private static int selection = 0;
        private static int response = 0;
        private static BTree tree; 
        #endregion

        static void Main()
        {
            ConsoleStartUp();

            //Main Menu
            while (selection != 5)
            {
                try
                {
                    Clear();
                    MenuDialog();

                    switch (selection)
                    {
                        case 1:
                            Clear();
                            Console.Write("What is the arity of the tree to be created? ");
                            response = Convert.ToInt16(Console.ReadLine());
                            tree = new BTree(response);

                            Console.WriteLine($"The tree has been built; ___ values were added in ___ loops.");
                            Console.WriteLine("\n\n\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case 2:
                            tree.DisplayTree();
                            Console.ReadKey();
                            break;
                        case 3:
                            Console.WriteLine("What value do you want to add to the tree? ");
                            response = Convert.ToInt16(Console.ReadLine());

                            tree.AddedValue(response);

                            Console.WriteLine($"{response} was added to the tree.");
                            Console.WriteLine("\n\n\n\nPress any key to continue...");
                            Console.ReadKey();
                            break;
                        case 4:
                            Console.WriteLine("What value do you want to find? ");
                            response = Convert.ToInt16(Console.ReadLine());

                            //implement find value method

                            break;
                        case 5:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                }
            }

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

        #region Menu
        /// <summary>
        /// Menu method to display menu options to user
        /// </summary>
        private static void MenuDialog()
        {
            string response = string.Empty;
            bool IsValid = true;

            Menu = "\n\nB-Trees Menu"
                + "\n------------"
                + "\n1. Set Size of Node and Create new B-Tree"
                + "\n2. Display the B-Tree"
                + "\n3. Add a Value to the B-Tree"
                + "\n4. Find a Value in the B-Tree"
                + "\n5. End the Program"
                + "\n\n Type the number of your choice from the menu: ";

            Console.WriteLine(Menu);
            response = Console.ReadLine();

            IsValid = int.TryParse(response, out selection);

            if (!IsValid || selection > 5 || selection <= 0)
            {
                Console.WriteLine("Invalid Entry");
                MenuDialog();
            }
           
        }
        #endregion
    }
}
