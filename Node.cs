//	Project:		Project 5 - BTree
//	File Name:		Node.cs
//	Description:	Node class for storing and displaying node information
//	Course:			CSCI 2210-001 - Data Structures
//	Authors:		Reed Jackson, reedejackson@gmail.com, jacksonre@etsu.edu
//                  Haley Hughes, hugheshe1@etsu.edu
//                  John Burdette, burdettj@etsu.edu
//	Created:		11/23/2016
//	Copyright:		Reed Jackson, Haley Hughes, John Burdette, 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    /// <summary>
    /// Node class for storing and displaying node information
    /// </summary>
    class Node
    {
        #region Local Values

        private Random rand = new Random();

        #endregion

        #region Properites

        public int NodeSize { get; set; }

        public List<int> Items { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for Node class
        /// </summary>
        public Node()
        {
            NodeSize = 3;
            Items = new List<int>(3);
        }

        /// <summary>
        /// Parameterized constructor for Node class
        /// </summary>
        /// <param name="nodeSize">Represents the size of new node</param>
        public Node(int nodeSize)
        {
            NodeSize = nodeSize;
            Items = new List<int>(NodeSize);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overridden ToString method used to display node information
        /// </summary>
        /// <returns>A string containing values for each node</returns>
        public override string ToString()
        {
            string result = "";
            result += $"\nNumber of items: {Items.Count} out of {NodeSize}";
            result += $"\nNode is {String.Format("{0:0.##}", (((double)Items.Count / NodeSize) * 100))}\u0025 full";
            result += "\nValues:\n";
            return result;
        }

        #endregion
    }
}
