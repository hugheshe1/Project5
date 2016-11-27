//	Project:		Project 5 - BTree
//	File Name:		BTree.cs
//	Description:	A BTree class for creating and handling function for a new BTree
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
    /// A BTree class for creating and handling functions for a new BTree
    /// </summary>
    class BTree
    {
        #region Properties

        public int NodeCount { get; set; }

        public int IndexCount { get; set; }

        public int LeafCount { get; set; }

        public int NodeSize { get; set; }

        public Index Root { get; set; }

        public List<Index> TreeIndexs { get; set; }

        public List<Leaf> TreeLeaves { get; set; }

        public Stack<Index> MainStack { get; set; }

        public Stack<Index> PreOrderStack { get; set; }

        public List<string> PreOrder { get; set; }

        public int PreOrderCount { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterized constructor for BTree class
        /// </summary>
        /// <param name="arity">Represents the arity of the BTree</param>
        public BTree(int arity)
        {
            NodeSize = arity;
            Root = new Index();
            TreeIndexs = new List<Index>();
            MainStack = new Stack<Index>();
            PreOrderStack = new Stack<Index>();

            //Initialize counts
            NodeCount = 0;
            IndexCount = 0;
            LeafCount = 0;
            PreOrderCount = 0;
        }

        #endregion

        #region Add a value to the Tree

        /// <summary>
        /// Method for adding a value to the BTree
        /// </summary>
        /// <param name="value">Represents the value to be added</param>
        /// <returns>Boolean representing if the value was added successfully or not</returns>
        public bool AddedValue(int value)
        {
            #region Initialize first value and Root

            if (NodeCount == 0)
            {
                Index FirstIndex = new Index(NodeSize);
                Leaf FirstLeaf = new Leaf(NodeSize);

                //Add value to the first Index and Leaf
                FirstIndex.Items.Add(value);
                FirstLeaf.Items.Add(value);

                //Reference initial and first Leaf
                FirstIndex.LeafList.Add(new Leaf(NodeSize));
                FirstIndex.LeafList.Add(FirstLeaf);

                //Set IndexLevel
                FirstIndex.IndexLevel = 0;

                //Add index to IndexList and Root
                Root = new Index(FirstIndex);
                TreeIndexs.Add(Root);
                TreeLeaves.Add(FirstIndex.LeafList[0]);
                TreeLeaves.Add(FirstIndex.LeafList[1]);

                //Increment Counts
                NodeCount++;
                IndexCount++;
                LeafCount++;
                return true;
            }

            #endregion

            #region Attempt to put value into a Leaf

            else
            {
                //Find Leaf to insert value
                Leaf LeafToFill = FindLeaf(value);
                INSERT response = LeafToFill.Insert(value);
                if (response == INSERT.DUPLICATE)
                {
                    //Do nothing since you need a unique new value
                    return false;
                }
                else if (response == INSERT.NEEDSPLIT)
                {
                    //Split Leaf and Indexes if needed
                    SplitLeaf(LeafToFill);
                    return true;
                }
                else
                {
                    //Success!
                    return true;
                }
            } 

            #endregion
        }

        #endregion

        #region Finding Nodes on Tree Methods

        /// <summary>
        /// Method for finding the leaf in the BTree where a specified value is stored
        /// </summary>
        /// <param name="value">The specified value to search for</param>
        /// <returns>The Leaf that contains the specified value</returns>
        public Leaf FindLeaf(int value)
        {
            //Initialize starting point
            Index SearchIndex = new Index(Root);
            Leaf LeafToInsert = new Leaf(NodeSize);
            MainStack.Clear();

            //Find Deepest Index
            bool foundLeaf = false;
            while (foundLeaf == false)
            {
                //Mark Index in MainStack
                MainStack.Push(SearchIndex);

                if (SearchIndex.IndexList.Count == 0)
                {
                    //Exit once found
                    foundLeaf = true;
                }
                else
                {
                    //Find the next Index to go to
                    SearchIndex = FindIndex(SearchIndex, value);
                }
            }

            //Find Leaf needed to insert into
            for (int i = 1; i < SearchIndex.Items.Count; i++)
            {
                if (value < SearchIndex.Items[i])
                {
                    return SearchIndex.LeafList[i];
                }
            }

            //Return last leaf in index
            return SearchIndex.LeafList[SearchIndex.LeafList.Count - 1];
        }

        /// <summary>
        /// Method for finding the Index where a specified value is stored
        /// </summary>
        /// <param name="SearchIndex">The specified Index</param>
        /// <param name="value">The specified value</param>
        public Index FindIndex(Index SearchIndex, int value)
        {
            for (int i = 1; i < SearchIndex.IndexList.Count; i++)
            {
                if (value < SearchIndex.Items[i])
                {
                    //Return the previous index
                    return SearchIndex.IndexList[i - 1];
                }
            }

            //If value is larger than all put it
            //in the last Index
            return SearchIndex.IndexList[NodeSize - 1];
        }

        #endregion

        #region Splitting Nodes on Tree Methods

        /// <summary>
        /// Method for splitting a leaf and then adding to correct place in BTree
        /// </summary>
        /// <param name="FullLeaf">Represents the leaf to be split</param>
        public void SplitLeaf(Leaf FullLeaf)
        {
            //Initialize values
            Leaf NewLeaf = new Leaf(NodeSize);
            int half = FullLeaf.Items.Count / 2;
            int newIndexValue = FullLeaf.Items[half];
            int FullLeafCount = FullLeaf.Items.Count;

            for (int i = half; i < FullLeaf.Items.Count; i++)
            {
                //Add the values to another Leaf
                NewLeaf.Items.Add(FullLeaf.Items[i]);
            }

            for (int i = half; i < FullLeafCount; i++)
            {
                //Then remove them from the FullLeaf
                FullLeaf.Items.RemoveAt(half);
            }

            Index Temp = new Index(MainStack.Peek());

            MainStack.Peek().Items.Add(newIndexValue);
            MainStack.Peek().LeafList.Add(NewLeaf);

            //Split Index if needed
            if (MainStack.Peek().Items.Count > NodeSize)
            {
                SplitIndex();
            }
        }

        /// <summary>
        /// Method splitting an index and adding it to the BTree
        /// </summary>
        public void SplitIndex()
        {
            if(MainStack.Peek().Items.Count <= NodeSize)
            {
                //Do nothing since all splitting is done
            }

            //Continue to Split
            else
            {
                #region Initialize values

                //Values to be set and used
                Index CurrentIndex = new Index(MainStack.Peek());
                Index LeftIndex;
                Index CenterIndex = new Index(NodeSize);
                Index RightIndex = new Index(NodeSize);
                List<Leaf> RightLeaves = new List<Leaf>();
                int half = CurrentIndex.Items.Count / 2;

                //Values to handle being the first root or
                //a non-inner index
                bool isFirstRootSplit = false;
                if (CurrentIndex.IndexList.Count == 0)
                {
                    isFirstRootSplit = true;
                }

                bool needToGetLeaves = false;
                if (CurrentIndex.LeafList != null)
                {
                    needToGetLeaves = true;
                }

                #endregion

                MainStack.Pop();

                #region If split at Root

                if (MainStack.Count == 0)
                {
                    //Root = CenterIndex

                    #region Set Right LeafList

                    //Set start to half if the Root doesn't
                    //reference any indexes
                    int rightCount = half + 1;
                    if (isFirstRootSplit)
                    {
                        rightCount = half;
                    }

                    //Enter Leaves into RightIndex
                    for (; rightCount < CurrentIndex.LeafList.Count; rightCount++)
                    {
                        RightLeaves.Add(CurrentIndex.LeafList[rightCount]);
                    }

                    #endregion

                    #region Set and Reference RightIndex's Indexes/Leaves

                    for (int i = half + 1; i < CurrentIndex.Items.Count; i++)
                    {
                        RightIndex.Items.Add(CurrentIndex.Items[i]);
                    }

                    if (isFirstRootSplit)
                    {
                        for (int i = 0; i < RightIndex.Items.Count; i++)
                        {
                            RightIndex.LeafList.Add(RightLeaves[i]);
                        }
                    }
                    else
                    {
                        for (int i = half; i < CurrentIndex.Items.Count; i++)
                        {
                            RightIndex.IndexList.Add(CurrentIndex.IndexList[i]);
                        }
                    }

                    #endregion

                    //Set Right Index Level
                    RightIndex.IndexLevel = CurrentIndex.IndexLevel;

                    //Dispose values
                    int disposeCount = CurrentIndex.Items.Count;
                    for (int i = half; i < disposeCount; i++)
                    {
                        CurrentIndex.Items.RemoveAt(i);
                        CurrentIndex.IndexList.RemoveAt(i);
                        CurrentIndex.LeafList.RemoveAt(i);
                    }

                    //Set Left Index and reference to CenterIndex
                    LeftIndex = new Index(CurrentIndex);
                    CenterIndex.IndexList.Add(LeftIndex);
                    CenterIndex.IndexList.Add(RightIndex);

                    //Set Index Levels and add CenterIndex
                    IncrementAllTreeLevels();
                    CenterIndex.IndexLevel = 0;
                    Root = new Index(CenterIndex);
                }

                #endregion

                #region If split elsewhere

                else
                {
                    #region Set Right LeafList

                    if (needToGetLeaves)
                    {
                        //Enter Leaves into RightIndex
                        for (int rightCount = half; rightCount < CurrentIndex.LeafList.Count; rightCount++)
                        {
                            RightLeaves.Add(CurrentIndex.LeafList[rightCount]);
                        } 
                    }

                    #endregion

                    #region Set and Reference RightIndex's Indexes/Leaves

                    for (int i = half + 1; i < CurrentIndex.Items.Count; i++)
                    {
                        RightIndex.Items.Add(CurrentIndex.Items[i]);
                    }

                    if (needToGetLeaves)
                    {
                        for (int i = 0; i < RightIndex.Items.Count; i++)
                        {
                            RightIndex.LeafList.Add(RightLeaves[i]);
                        }
                    }
                    else
                    {
                        for (int i = half; i < CurrentIndex.Items.Count; i++)
                        {
                            RightIndex.IndexList.Add(CurrentIndex.IndexList[i]);
                        }
                    }

                    #endregion

                    //Set Right Index Level
                    RightIndex.IndexLevel = CurrentIndex.IndexLevel;

                    //Dispose values
                    int disposeCount = CurrentIndex.Items.Count;
                    for (int i = half; i < disposeCount; i++)
                    {
                        CurrentIndex.Items.RemoveAt(i);
                        CurrentIndex.IndexList.RemoveAt(i);
                        CurrentIndex.LeafList.RemoveAt(i);
                    }

                    //Add CurrentIndex to the Index up the tree
                    //with references
                    MainStack.Peek().Insert(CurrentIndex.Items[half], CurrentIndex, RightIndex);

                    //Recursively call SplitIndex until all
                    //Indexes are split that need it
                    SplitIndex();
                }
                  
                #endregion
            }
        }

        /// <summary>
        /// Method for increasing the levels of the BTree
        /// </summary>
        public void IncrementAllTreeLevels()
        {
            for (int i = 0; i < TreeIndexs.Count; i++)
            {
                TreeIndexs[i].IndexLevel++;
            }
        }

        #endregion

        #region Find Depth and Number of Values
        /// <summary>
        /// Method for determining the depth of the BTree
        /// </summary>
        /// <returns>Int representing the depth of the BTree</returns>
        public int FindDepth()
        {
            int depth = 0;

            foreach (Index i in TreeIndexs)
            {
                if (i.IndexLevel > depth)
                    depth = i.IndexLevel;
            }
        
            return depth;
        }

        public int TotalNumValues ( )
        {
            int values = 0;

            // number of indexes
            values += IndexCount;

            // adding number of values present in each leaf
            foreach (Leaf l in TreeLeaves)
            {
                values += l.Items.Count;
            }

            return values;
        }
        #endregion

        #region Displaying Methods
        /// <summary>
        /// Method for traversing the BTree by a given index
        /// </summary>
        /// <param name="SearchIndex">Represents the Index</param>


        /* Commenting this out since the preorder traversal seems to be a display method in his program, so I wrote one that does that
         * 
         * 
        public void PreOrderTraversal(Index SearchIndex)
        {
            for (int i = 0; i < SearchIndex.IndexList.Count; i++)
            {
                //Add Next String
                PreOrder.Add(SearchIndex.IndexList[i].ToString());

                if (SearchIndex.IndexList.Count == 0)
                {
                    //Step down to sub tree
                    PreOrderTraversal(SearchIndex.IndexList[i]);
                }
                else
                {
                    for (int j = 0; j < SearchIndex.LeafList.Count; j++)
                        PreOrder.Add(SearchIndex.LeafList[j].ToString());
                }
            }
        }

        */

        /// <summary>
        /// Recursive method to look over the tree in pre-order and to display each node reached
        /// </summary>
        /// <param name="node">The node to start at, should be the root</param>
        public void PreorderDisplay (Node node)
        {
            if (node == null)
                // Nothing to do when there are no nodes in the B tree
                return;

            if (node is Leaf)
            {
                //show the values in the leaf
                Console.WriteLine ((Leaf)node);
                // end this call
                return;
            }

            if (node is Index)
            {
                Console.WriteLine ((Index)node);

                // checking if the index has a list of indexes (that is, if it doesn't point to leaves)
                if (((Index)node).IndexList != null)
                {
                    //recursively calling for each element of the index list
                    foreach (Index i in ((Index)node).IndexList)
                        PreorderDisplay (node);
                }
                else
                {
                    //recursively calling the method for each element of the leaf list
                    foreach (Leaf l in ((Index)node).LeafList)
                        PreorderDisplay (node);
                }
            }
        }

        /// <summary>
        /// Method for retrieving a List of contents within BTree
        /// </summary>
        /// <returns>List of strings representing information for each item in BTree</returns>
        public List<string> DisplayTree()
        {
            //Clear PreOrder
            PreOrder.Clear();
            
            //Start PreOrder at Root
            PreOrder.Add(Root.ToString());
            PreOrderStack.Push(Root);
            PreOrderTraversal(Root);

            //Return List of index strings
            return PreOrder;
        }

        /// <summary>
        /// Method for displaying BTree statistics
        /// </summary>
        /// <returns>String representing BTree statistics</returns>
        public string Stats()
        {
            string stats = "";
            stats += $"Number of Index Nodes: {IndexCount}";
            stats += $"\nNumber of Lead Nodes: {LeafCount}";
            stats += $"\nThe depth of the tree is {FindDepth()}";
            stats += $"\nTotal number of values in the tree: {NodeCount}";
            return stats;
        }

        /// <summary>
        /// Method for displaying each node in the BTree
        /// </summary>
        /// <param name="node">Represents the node in the BTree</param>
        public void display(Node node)
        {
            if (node == null)
                return;

            if (node is Leaf)
            {
                //display the values of the leaf
                Console.WriteLine((Leaf)node);
                // end this call
                return;
            }

            if (node is Index)
            {
                Console.WriteLine((Index)node);

                // checking if the index has a list of indexes (that is, it doesn't point to leaves)
                if (((Index)node).IndexList != null)
                {
                    //iterate over each index in the node
                    foreach (Index i in ((Index)node).IndexList)
                        display(node);
                }
                else
                {
                    //display the leaves
                    foreach (Leaf l in ((Index)node).LeafList)
                        display(node);
                }
            }
        }

        #endregion 
    }
}
