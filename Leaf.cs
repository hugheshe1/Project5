//	Project:		Project 5 - BTree
//	File Name:		Leaf.cs
//	Description:	
//	Course:			CSCI 2210-001 - Data Structures
//	Authors:		Reed Jackson, reedejackson@gmail.com, jacksonre@etsu.edu
//                  Other Author
//                  Other Author
//	Created:		11/23/2016
//	Copyright:		Reed Jackson, Author, Author, 2016

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class Leaf : Node
    {
        #region Constructors

        public Leaf() { }

        public Leaf(int nodeSize)
        {
            NodeSize = nodeSize;
        }

        public Leaf(Leaf CopyLeaf)
        {
            NodeSize = CopyLeaf.NodeSize;
            Items = new List<int>(CopyLeaf.Items);
        }

        #endregion

        #region Insertion Methods

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


        /// <summary>
        /// ToString override for Leaf
        /// </summary>
        /// <returns>info, a string containing the relevant information pertaining to the leaf</returns>
        #region ToString
        public override String ToString()
        {
            String info = "\n\nNode type: Leaf";
            info += ("\nNumber of values: " + Items.Count + " (Node is " + ((Items.Count/NodeSize)*100) +
                    " percent full)");
            info += "\nValues: ";

            foreach (int i in Items)
                info += ("\n" + i + " ");

            return info;
        }
        #endregion
    }
}
