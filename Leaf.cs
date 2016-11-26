//	Project:		Project 5 - BTree
//	File Name:		Leaf.cs
//	Description:	A Leaf class (subclass of Node) that stores leaf information and its functionality
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
    /// A Leaf class (subclass of Node) that stores leaf information and its functionality
    /// </summary>
    class Leaf : Node
    {
        #region Constructors

        /// <summary>
        /// Default constructor for the creation of a Leaf object
        /// </summary>
        public Leaf() { }

        /// <summary>
        /// Parameterized constructor for the creation of a Leaf object
        /// </summary>
        /// <param name="nodeSize">Represents the desired node size</param>
        public Leaf(int nodeSize)
        {
            NodeSize = nodeSize;
        }

        /// <summary>
        /// Parameterized constructor for the creation of a Leaf object
        /// </summary>
        /// <param name="CopyLeaf">Represents the desired Leaf</param>
        public Leaf(Leaf CopyLeaf)
        {
            NodeSize = CopyLeaf.NodeSize;
            Items = new List<int>(CopyLeaf.Items);
        }

        #endregion

        #region Insertion Methods

        /// <summary>
        /// Method for inserting a value into a leaf
        /// </summary>
        /// <param name="value">Represents the value to be inserted</param>
        /// <returns>Insertion status</returns>
        public INSERT Insert(int value)
        {
            if (Items.Count == 0)
            {
                Items.Add(value);
                return INSERT.SUCCESS;
            }
            else
            {
                //Initial Values
                int i;
                Items.Add(value);

                //Position temp to the smallest place it 
                //can go
                for (i = Items.Count - 1; (i > 0 && value <= Items[i - 1]); i--)
                {
                    //This prevents duplicates from being added
                    if (Items[i - 1] == value)
                    {
                        //Undo changes to Items List
                        for (int j = i; j < Items.Count - 2; j++)
                        {
                            Items[i + 1] = Items[i + 2];
                        }

                        //Remove top value
                        Items.RemoveAt(Items.Count - 1);
                        return INSERT.DUPLICATE;
                    }
                    else
                    {
                        Items[i] = Items[i - 1];
                    }
                }


                //Insert the value to the selected position
                Items[i] = value;

                //Set return message
                if (Items.Count > NodeSize)
                {
                    return INSERT.NEEDSPLIT;
                }
                else
                {
                    return INSERT.SUCCESS;
                }
            }
        }

        #endregion
    }
}
