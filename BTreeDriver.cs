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
    /// <summary>
    /// A driver class for displaying BTree information and handling functions
    /// </summary>
    class BTreeDriver
    {

        #region Properties
        private static Random rand = new Random();
        private static string Menu = string.Empty;
        private static int selection = 0;
        private static int response = 0;
        private static BTree tree;
        private static int totalAdded = 0;
        private static int totalAddAttempts = 0;
        #endregion

        #region Main Method
        /// <summary>
        /// Main method for the BTreeDriver class which is used for handling method calls for menu and BTree functionality
        /// </summary>
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
                            Write("What is the arity of the tree to be created? ");
                            response = Convert.ToInt16(ReadLine());
                            tree = new BTree(response);
                            FillTree();
                            WriteLine($"The tree has been built; {totalAdded} values were added in {totalAddAttempts} loops.");
                            WriteLine("\n\n\n\nPress any key to continue...");
                            ReadKey();
                            break;
                        case 2:
                            Clear();
                            if (tree != null)
                            {
                                tree.DisplayTree();
                                WriteLine(tree.Stats());
                                ReadKey();
                            }                          
                            break;
                        case 3:
                            WriteLine("What value do you want to add to the tree? ");
                            response = Convert.ToInt16(ReadLine());

                            tree.AddedValue(response);

                            WriteLine($"{response} was added to the tree.");
                            WriteLine("\n\n\n\nPress any key to continue...");
                            ReadKey();
                            break;
                        case 4:
                            WriteLine("What value do you want to find? ");
                            response = Convert.ToInt16(ReadLine());
                            Leaf leaf = null;
                            bool match = tree.FindValue(response, out leaf);

                            Clear();

                            if (match)
                                WriteLine($"{response} was found in the tree.");
                            else
                                WriteLine($"{response} was not found in the tree.");

                            WriteLine("\n\n\n\nPress any key to continue...");
                            ReadKey();
                            break;
                        case 5:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Clear();
                    WriteLine(e.Message);
                }
            }
        } 
        #endregion

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

            WriteLine(Menu);
            response = ReadLine();

            IsValid = int.TryParse(response, out selection);

            if (!IsValid || selection > 5 || selection <= 0)
            {
                WriteLine("Invalid Entry");
                MenuDialog();
            }
           
        }
        #endregion

        #region Fill Tree Method
        /// <summary>
        /// Method for filling a BTree with values
        /// </summary>
        private static void FillTree()
        {
            bool success;

            if (tree == null)
                WriteLine("A tree has not yet been created.");
            else
            {
                while (totalAdded < 20)
                {
                    success = tree.AddedValue(rand.Next(100));

                    if (success)
                    {
                        totalAdded++;
                        totalAddAttempts++;
                    }
                    else
                        totalAddAttempts++;
                }
            }
        }
        #endregion
    }
}
