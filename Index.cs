//	Project:		Project 5 - BTree
//	File Name:		Index.cs
//	Description:	An Index class (subclass of Node) that stores index information
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

namespace Project5
{
    /// <summary>
    /// An Index class (subclass of Node) that stores index information
    /// </summary>
    class Index : Node
    {
        #region Properties

        public List<Index> IndexList { get; set; }

        public List<Leaf> LeafList { get; set; }

        public int IndexLevel { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor for the creation of an Index object
        /// </summary>
        public Index()
        {
            IndexList = new List<Index>(base.NodeSize);
            LeafList = new List<Leaf>(base.NodeSize);
            Items.Add(-1);
        }

        /// <summary>
        /// Parameterized constructor for the creation of an Index object
        /// </summary>
        /// <param name="nodeSize">Represents the desired node size</param>
        public Index(int nodeSize)
        {
            NodeSize = nodeSize;
            IndexList = new List<Index>(NodeSize);
            LeafList = new List<Leaf>(NodeSize);
            Items.Add(-1);
        }

        /// <summary>
        /// Parameterized constructor for the creation of an Index object
        /// </summary>
        /// <param name="CopyIndex">Represents the desired Index</param>
        public Index(Index CopyIndex)
        {
            NodeSize = CopyIndex.NodeSize;
            IndexList = CopyIndex.IndexList;
            LeafList = CopyIndex.LeafList;
            Items = new List<int>(CopyIndex.Items);
        }

        #endregion

        #region Insertion Methods

        /// <summary>
        /// Method for inserting a value
        /// </summary>
        /// <param name="value">Represents value to be added</param>
        /// <param name="LeftIndex">Represents left index</param>
        /// <param name="RightIndex">Represents right index</param>
        public void Insert(int value, Index LeftIndex, Index RightIndex)
        {
            //Initial Values
            int i;
            Items.Add(value);

            //Position temp to the smallest place it 
            //can go
            for (i = Items.Count - 1; (i > 0 && value <= Items[i - 1]); i--)
            {
                Items[i] = Items[i - 1];
            }

            //Insert the value to the selected position
            Items[i] = value;

            //Set Index References
            IndexList.RemoveAt(i - 1);
            IndexList.RemoveAt(i);
            IndexList.Insert(i - 1, LeftIndex);
            IndexList.Insert(i, RightIndex);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Overridden ToString method for displaying Index information
        /// </summary>
        /// <returns>String representing index information</returns>
        public override string ToString()
        {
            string result = base.ToString();
            result += $"\nIndex Level: {IndexLevel}";
            if (IndexList.Count > 0)
            {
                result += $"\nIndexes Pointed to:\n\t";
                for (int i = 0; i < IndexList.Count; i++)
                {
                    result += $"{IndexList[i] }";
                }
            }
            if (LeafList.Count > 0)
            {
                result += $"\nLeaves Pointed to:\n\t";
                for (int i = 0; i < LeafList.Count; i++)
                {
                    result += $"{LeafList[i] }";
                }
            }
            return result;
        }

        #endregion
    }
}
