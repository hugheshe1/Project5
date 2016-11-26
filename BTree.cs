//	Project:		Project 5 - BTree
//	File Name:		BTree.cs
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
using static System.Console;

namespace Project5
{
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

            MainStack.Peek().Items.Add(newIndexValue);
            MainStack.Peek().LeafList.Add(NewLeaf);

            //Split Index if needed
            if (MainStack.Peek().Items.Count > NodeSize)
            {
                SplitIndex();
            }
        }

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

        public void IncrementAllTreeLevels()
        {
            for (int i = 0; i < TreeIndexs.Count; i++)
            {
                TreeIndexs[i].IndexLevel++;
            }
        }

        #endregion

        #region Displaying Methods

        public void PreOrderTraversal(Index SearchIndex)
        {
            for (int i = 0; i < SearchIndex.IndexList.Count; i++)
            {
                //Add Next String
                PreOrder.Add(SearchIndex.IndexList[i].ToString());

                //Add to Index Stack
                PreOrderStack.Push(SearchIndex.IndexList[i]);

                if (SearchIndex.IndexList[i] != null)
                {
                    //Step down to sub tree
                    PreOrderTraversal(SearchIndex.IndexList[i]); 
                }
            }
        }

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

        public string Stats()
        {
            string stats = "";
            return stats;
        }

        #endregion

        //Need Display Method
        //...Search For Node method(Base off FindLeaf Method)
        //...Need Stats Method
        //...Need to implement counts

        //Idea for display: put tree sideways
        //       Root
        //Level: 0          1           2           3
        //              
        //                              *, 150, 300
        //                  *, 150
        //       *, 100
        //                  *, 75
        //                              
        //Or Put top down   
    }
}
