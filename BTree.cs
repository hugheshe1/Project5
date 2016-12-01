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

        public int TreeIndexes { get; set; }

        public int TreeLeaves { get; set; }

        public int DeepestDepth { get; set; }

        public int NodeSize { get; set; }

        public Index Root { get; set; }

        public Stack<Index> MainStack { get; set; }

        public Stack<Index> PreOrderStack { get; set; }

        public List<string> PreOrder { get; set; }

        public int PreOrderCount { get; set; }

        public List<string> GetNodesTraveled { get; set; }

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
            MainStack = new Stack<Index>();
            PreOrder = new List<string>();
            GetNodesTraveled = new List<string>();
            DeepestDepth = 0;

            //Initialize counts
            NodeCount = 0;
            TreeIndexes = 0;
            TreeLeaves = 0;
            PreOrderCount = 0;
        }

        #endregion

        #region Add a value to the Tree

        /// <summary>
        /// Method for adding a value to the BTree
        /// </summary>
        /// <param name="value">Represents the value to be added</param>
        /// <returns>Boolean representing if the value was added successfully or not</returns>
        public bool AddValue(int value)
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
                

                //Increment Counts
                NodeCount += 3;
                TreeIndexes++;
                TreeLeaves += 2;

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
            GetNodesTraveled.Add(SearchIndex.ToString());
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
                    GetNodesTraveled.Add(SearchIndex.ToString());
                }
            }

            //Find Leaf needed to insert into
            for (int i = 1; i < SearchIndex.Items.Count; i++)
            {
                GetNodesTraveled.Add(SearchIndex.LeafList[i].ToString());
                if (value < SearchIndex.Items[i])
                {
                    return SearchIndex.LeafList[i - 1];
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
            return SearchIndex.IndexList[SearchIndex.Items.Count - 1];
        }

        #endregion

        #region Find Value Method

        /// <summary>
        /// Method to determine if a specified value exists in the BTree
        /// </summary>
        /// <param name="value">The specified value to be searched for</param>
        /// <returns>A boolean that represents if the value was located within the BTree</returns>
        public bool FindValue(int value, out Leaf match)
        {
            GetNodesTraveled.Clear();
            Leaf leaf = FindLeaf(value);

            if (leaf.Items.Contains(value))
            {
                match = leaf;
                return true;
            }
            else
            {
                match = null;
                return false;
            }
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
            
            //Insert new index item and leaf
            MainStack.Peek().Insert(newIndexValue, NewLeaf);

            //Reset Root if it has changed
            if (MainStack.Count == 1)
            {
                Root = new Index(MainStack.Peek());
            }

            //Increment Nodes
            TreeLeaves++;
            NodeCount++;

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
                int newIndexItem = CurrentIndex.Items[half];

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

                #region If split at Root

                if (MainStack.Count == 1)
                {
                    //Root = CenterIndex

                    #region Set Right LeafList

                    //Set start to half if the Root doesn't
                    //reference any indexes
                    if (isFirstRootSplit)
                    {
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

                    //Dispose values
                    int disposeCount = CurrentIndex.Items.Count;
                    for (int i = half; i < disposeCount; i++)
                    {
                        CurrentIndex.Items.RemoveAt(half);
                        if (isFirstRootSplit)
                        {
                            CurrentIndex.LeafList.RemoveAt(half);
                        }
                        else
                        {
                            CurrentIndex.IndexList.RemoveAt(half);
                        }
                    }

                    //Set Left Index
                    LeftIndex = new Index(CurrentIndex);

                    //Set Right Index Level
                    LeftIndex.IndexLevel = CurrentIndex.IndexLevel;
                    RightIndex.IndexLevel = CurrentIndex.IndexLevel;

                    //Set and Reference CenterIndex
                    CenterIndex.IndexList.Add(LeftIndex);
                    CenterIndex.Insert(newIndexItem, RightIndex);
                    CenterIndex.IndexLevel = 0;
                    Root = new Index(CenterIndex);

                    //Set Index Levels
                    IncrementAllTreeLevels(Root);

                    //Increment Count
                    TreeIndexes += 2;
                    NodeCount += 2;

                    //Pop stack and move up tree
                    MainStack.Pop();
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
                        MainStack.Peek().Items.RemoveAt(half);
                        if (needToGetLeaves)
                        {
                            MainStack.Peek().LeafList.RemoveAt(half);
                        }
                        else
                        {
                            MainStack.Peek().IndexList.RemoveAt(half);
                        }
                    }

                    //Pop stack and move up tree
                    MainStack.Pop();

                    //Add RightIndex to the Index up the tree
                    //with references
                    MainStack.Peek().Insert(newIndexItem, RightIndex);

                    //Reset Root if needed
                    if (MainStack.Count == 1)
                    {
                        Root = new Index(MainStack.Peek()); 
                    }

                    //Increment Count
                    TreeIndexes++;
                    NodeCount++;

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
        public void IncrementAllTreeLevels(Index SearchIndex)
        {
            for (int i = 0; i < SearchIndex.IndexList.Count; i++)
            {
                //Increment Next Index
                SearchIndex.IndexList[i].IndexLevel++;

                //Step down if needed
                if (SearchIndex.IndexList.Count > 0)
                {
                    IncrementAllTreeLevels(SearchIndex.IndexList[i]);
                }
            }
        }

        #endregion

        #region Find Depth and Number of Values

        /// <summary>
        /// Method for determining the depth of the BTree
        /// </summary>
        /// <returns>Int representing the depth of the BTree</returns>
        public void FindDepth(Index SearchIndex)
        {
            if (SearchIndex.IndexList.Count > 0)
            {
                //Step down to sub tree
                FindDepth(SearchIndex.IndexList[0]);
            }
            else
            {
                DeepestDepth = SearchIndex.IndexLevel + 1;
            }
        }


        #endregion

        #region Displaying Methods

        /// <summary>
        /// Method for traversing the BTree by a given index and putting
        /// the BTree PreOrder in a list of strings
        /// </summary>
        /// <param name="SearchIndex">Represents the Index</param>
        public void PreOrderTraversal(Index SearchIndex)
        {
            for (int i = 0; i < SearchIndex.IndexList.Count; i++)
            {
                //Add Next String
                PreOrder.Add(SearchIndex.IndexList[i].ToString());

                if (SearchIndex.IndexList.Count > 0)
                {
                    //Step down to sub tree
                    PreOrderTraversal(SearchIndex.IndexList[i]);
                }
                else
                {
                    //Add all Leaf strings
                    for (int j = 0; j < SearchIndex.LeafList.Count; j++)
                        PreOrder.Add(SearchIndex.LeafList[j].ToString());
                }
            }

            //For a Root that has no indexes
            if (SearchIndex.LeafList != null)
            {
                for (int k = 0; k < SearchIndex.LeafList.Count; k++)
                    PreOrder.Add(SearchIndex.LeafList[k].ToString());
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
            FindDepth(Root);
            string stats = "";
            stats += $"Number of Index Nodes: {TreeIndexes}";
            stats += $"\nNumber of Leaf Nodes: {TreeLeaves}";
            stats += $"\nTotal number of nodes in the tree: {NodeCount}";
            stats += $"\nThe depth of the tree is {DeepestDepth}";
            //ToDo in driver: stats += $"\nTotal number of values in the tree: {}";
            return stats;
        }

        #endregion 
    }
}
