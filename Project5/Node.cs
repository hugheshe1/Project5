//	Project:		Project 5 - BTree
//	File Name:		Node.cs
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

namespace Project5
{
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

        public Node()
        {
            NodeSize = 3;
            Items = new List<int>(3);
        }

        public Node(int nodeSize)
        {
            NodeSize = nodeSize;
            Items = new List<int>(NodeSize);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            string result = "";
            result += $"Node Size: {Items.Count} out of {NodeSize}\n" + 
                      $"Item List:\n\t";
            for (int i = 0; i < Items.Count; i++)
            {
                result += $"{Items[i]} ";
            }
            return result;
        }

        #endregion
    }
}
