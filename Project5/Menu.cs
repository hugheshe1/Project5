//	Project:		Menu Creation Project
//	File Name:		Menu.cs
//	Description:	Class for generating menus in code
//	Course:			CSCI 2210-001 - Data Structures
//	Author:			Reed Jackson, reedejackson@gmail.com, jacksonre@etsu.edu
//	Created:		09/10/2016
//	Copyright:		Reed Jackson, 2016

using System.Collections;
using System.Collections.Generic;

namespace MenuCreation
{
    /// <summary>
    /// This Class is used for generating menus
    /// </summary>
    public class Menu
    {
        private string menuTitle;
        private List<string> MenuOptions = new List<string>();

        #region Constructors

        //Generate Menu
        public Menu(string title)
        {
            menuTitle = title;
        } 
       
        #endregion

        #region Menu Option Manipulation

        /// <summary>
        /// Takes array of strings to add as menu options
        /// </summary>
        /// <param name="strValue"></param>
        public void AddMenuItems(string[] strValue)
        {
            MenuOptions.AddRange(strValue);
        }

        /// <summary>
        /// Takes menu option number and inserts the particular string to go there
        /// </summary>
        /// <param name="optionNumber"></param>
        /// <param name="strValue"></param>
        public void InsertMenuitem(int optionNumber, string strValue)
        {
            optionNumber -= 1;
            MenuOptions.Insert(optionNumber, strValue);
        }

        /// <summary>
        /// Removes menu option based on string value if it exists
        /// </summary>
        /// <param name="strValue"></param>
        public void RemoveMenuItem(string strValue)
        {
            MenuOptions.Remove(strValue);
        }

        /// <summary>
        /// Removes menu option based on it's number in the menu
        /// </summary>
        /// <param name="optionNumber"></param>
        public void RemoveMenuItemNumber(int optionNumber)
        {
            optionNumber -= 1;
            MenuOptions.RemoveAt(optionNumber);
        }

        #endregion

        #region ToString
       
        /// <summary>
        /// Editing how the menu display looks
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //Add Title
            string menuStr = menuTitle + "\n\n";

            //Add Options
            for (int i = 0; i < MenuOptions.Count; i++)
            {
                menuStr = $"{menuStr}{i + 1}.  {MenuOptions[i]}\n";
            }

            //Return menu to display
            return menuStr;
        } 
       
        #endregion
    }
}