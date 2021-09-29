using CZ4031_Project1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZ4031_Project1.Controllers
{
    public static class BPlusTreeController 
    {
        public static Block CurrentRecordBlock { get; set; }
        public static int numNodes { get; set; }
        public static int Levels { get; set; }

        public const int NodeAddressSize = BlockController.BlockAddressSize;
        public static Dictionary<byte[], Block> Blocks = BlockController.Blocks;

        public static void insert(BPlusTree tree, int key, byte[] address)
        {
            int m = GetMaxKeys();
            // Case 1: Inserting to an Empty B Plus Tree
            
            if (null == tree.rootBlock)
            {
                // For an empty B Tree, root is created and its key is set as the
                // newly inserted key
                CurrentRecordBlock = BlockController.CreateBlock();
                Block newBlock = CurrentRecordBlock;
                Node newNode = new Node();
                newNode.Key = key;
                newNode.Pointer = address;
                newNode.KeyPointers = new List<byte[]>();
                newNode.KeyPointers = null;
                newBlock.Nodes.Add(newNode);
                newBlock.children = new List<Block>();
                newBlock.children = null;
                tree.rootBlock = newBlock;
                // Since the root has no parent, parent set to null
                tree.rootBlock.Parent = null;
                
            }

            // Case 2: Only one node that is not full
            else if (tree.rootBlock.children == null && tree.rootBlock.Nodes.Count < (m - 1))
            {

                    // For all insertions until the root gets overfull for the first
                    // time, we just update the root node, adding the new keys
                    insertWithinExternalNode(key, address, tree.rootBlock);
                
            }

            // Case 3: Normal insert
            else
            {
                Block curr = tree.rootBlock;
                // Since we insert the element only at the external node, we
                // traverse to the last level
                while (curr.children != null)
                {
                    curr = curr.children[binarySearchWithinInternalNode(key, curr)];
                }
                insertWithinExternalNode(key, address, curr);
                if (curr.Nodes.Count == m)
                {
                    // If the external node becomes full, we split it
                    splitExternalNode(tree, curr, m);
                }
            }

        }

        private static void splitExternalNode(BPlusTree tree, Block curr, int m)
        {

            // Find the middle index
            int midIndex = m / 2;

            Block middle = BlockController.CreateBlock();

            Block rightPart = BlockController.CreateBlock();

            // Set the right part to have middle element and the elements right to
            // the middle element
            rightPart.Nodes = curr.Nodes.GetRange(midIndex, curr.Nodes.Count);
            rightPart.Parent = middle;

            // While making middle as the internal node, we add only the key since
            // internal nodes of bplus tree do not contain values
            Node newNode = new Node();
            newNode.Key = curr.Nodes[midIndex].Key;
            newNode.Pointer = curr.Nodes[midIndex].Pointer;
            newNode.KeyPointers = new List<byte[]>();
            newNode.KeyPointers.Add(curr.Nodes[midIndex].Pointer);
            middle.Nodes.Add(newNode);
            middle.children.Add(rightPart);

            // Curr holds the left part, so update the split node to contain just
            // the left part
            curr.Nodes.RemoveRange(midIndex, curr.Nodes.Count);

            bool firstSplit = true;
            // propogate the middle element up the tree and merge with parent of
            // previously overfull node
            splitInternalNode(tree, curr.Parent, curr, m, middle, firstSplit);

        }

        public static int binarySearchWithinInternalNode(int key, Block block)
        {
            int st = 0;
            int end = block.Nodes.Count() - 1;
            int mid;
            int index = -1;
            // Return first index if key is less than the first element
            if (key < block.Nodes[0].Key)
            {
                return 0;
            }
            // Return array size + 1 as the new positin of the key if greater than
            // last element
            if (key >= block.Nodes[end].Key)
            {
                return block.Nodes.Count();
            }
            while (st <= end)
            {
                mid = (st + end) / 2;
                // Following condition ensures that we find a location s.t. key is
                // smaller than element at that index and is greater than or equal
                // to the element at the previous index. This location is where the
                // key would be inserted
                if (key < block.Nodes[mid].Key && key >= block.Nodes[mid-1].Key)
                {
                    index = mid;
                    break;
                } // Following conditions follow normal Binary Search
                else if (key >= block.Nodes[mid].Key)
                {
                    st = mid + 1;
                }
                else
                {
                    end = mid - 1;
                }
            }
            return index;
        }

        private static void insertWithinExternalNode(int key, byte[] keyAddress, Block block)
        {
            //new block deets
            Node newNode = new Node();
            Block cursor = block;
            newNode.KeyPointers = new List<byte[]>();
            newNode.Key = key;
            newNode.Pointer = keyAddress;
            newNode.KeyPointers = null;
            
            int indexOfKey = binarySearchWithinInternalNode(key, block);

            if (indexOfKey != 0 && cursor.Nodes[indexOfKey - 1].Key == key)
                {
                // Key already exists. Add the new value to the list
                if(cursor.Nodes[indexOfKey - 1].KeyPointers == null)
                {
                    cursor.Nodes[indexOfKey - 1].KeyPointers = new List<byte[]>();
                }
                cursor.Nodes[indexOfKey - 1].KeyPointers.Add(keyAddress);
                }
                else
                {
                    cursor.Nodes.Add(newNode);
                }
       
        }

        public static List<T> Swap<T>(this List<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

        private static void splitInternalNode(BPlusTree tree, Block curr, Block prev, int m, Block toBeInserted, bool firstSplit)
        {
            if (null == curr)
            {
                // if we split the root before, then a new root has to be created
                tree.rootBlock = toBeInserted;
                // we find where the child has to be inserted by doing a binary
                // search on keys
                int indexForPrev = binarySearchWithinInternalNode(prev.Nodes[0].Key, toBeInserted);
                prev.Parent = toBeInserted;
                toBeInserted.children.Insert(indexForPrev, prev);
                if (firstSplit)
                {
                    // update the linked list only for first split (for external
                    // node)
                    if (indexForPrev == 0)
                    {
                        toBeInserted.children[0].Next = toBeInserted.children[1];
                        toBeInserted.children[1].Previous = toBeInserted.children[0];
                    }
                    else
                    {
                        toBeInserted.children[indexForPrev+1].Next = toBeInserted.children[indexForPrev];
                        toBeInserted.children[indexForPrev - 1].Previous = toBeInserted.children[indexForPrev];
                    }
                }
            }
            else
            {
                // merge the internal node with the mid + right of previous split
                mergeInternalNodes(toBeInserted, curr);
                if (curr.Nodes.Count == m)
                {
                    // do a split again if the internal node becomes full
                    int midIndex = (int)Math.Ceiling(m / 2.0) - 1;
                    Block middle = BlockController.CreateBlock();
                    Block rightPart = BlockController.CreateBlock();
                    // since internal nodes follow a split like the b tree, right
                    // part contains elements right of the mid element, and the
                    // middle becomes parent of right part
                    rightPart.Nodes = curr.Nodes.GetRange(midIndex + 1, curr.Nodes.Count);
                    rightPart.Parent = middle;

                    middle.Nodes.Add(curr.Nodes[midIndex]);
                    middle.children.Add(rightPart);

                    List<Block> childrenOfCurr = curr.children;
                    List<Block> childrenOfRight = new List<Block>();

                    int lastChildOfLeft = childrenOfCurr.Count - 1;

                    // update the children that have to be sent to the right part
                    // from the split node
                    for (int i = childrenOfCurr.Count - 1; i >= 0; i--)
                    {
                        List<Node> currKeysList = childrenOfCurr[i].Nodes;
                        if (middle.Nodes[0].Key <= currKeysList[0].Key)
                        {
                            childrenOfCurr[i].Parent = rightPart;
                            childrenOfRight.Insert(0, childrenOfCurr[i]);
                            lastChildOfLeft--;
                        }
                        else
                        {
                            break;
                        }
                    }

                    rightPart.children = childrenOfRight;

                    // update the overfull node to contain just the left part and
                    // update its children
                    curr.children.RemoveRange(lastChildOfLeft + 1, childrenOfCurr.Count);
                    curr.Nodes.RemoveRange(midIndex, curr.Nodes.Count);

                    // propogate split one level up
                    splitInternalNode(tree, curr.Parent, curr, m, middle, false);
                }
            }
        }

        private static void mergeInternalNodes(Block mergeFrom, Block mergeInto)
        {
            Node keyToBeInserted = mergeFrom.Nodes[0];
            Block childToBeInserted = mergeFrom.children[0];
            // Find the index where the key has to be inserted to by doing a binary
            // search
            int indexToBeInsertedAt = binarySearchWithinInternalNode(keyToBeInserted.Key, mergeInto);
            int childInsertPos = indexToBeInsertedAt;
            if (keyToBeInserted.Key <= childToBeInserted.Nodes[0].Key)
            {
                childInsertPos = indexToBeInsertedAt + 1;
            }
            childToBeInserted.Parent = mergeInto;
            mergeInto.children.Insert(childInsertPos, childToBeInserted);
            mergeInto.Nodes.Insert(indexToBeInsertedAt, keyToBeInserted);

            // Update Linked List of external nodes
            if (mergeInto.children != null && mergeInto.children[0].children == null)
            {

                // If merge is happening at the last element, then only pointer
                // between new node and previously last element
                // needs to be updated
                if (mergeInto.children.Count - 1 != childInsertPos
                        && mergeInto.children[childInsertPos + 1].Previous == null)
                {
                    mergeInto.children[childInsertPos + 1].Previous = mergeInto.children[childInsertPos];
                    mergeInto.children[childInsertPos].Next = mergeInto.children[childInsertPos + 1];
                }
                // If merge is happening at the last element, then only pointer
                // between new node and previously last element
                // needs to be updated
                else if (0 != childInsertPos && mergeInto.children[childInsertPos - 1].Next == null)
                {
                    mergeInto.children[childInsertPos].Previous = mergeInto.children[childInsertPos - 1];
                    mergeInto.children[childInsertPos - 1].Next = mergeInto.children[childInsertPos];
                }
                // If merge is happening in between, then the next element and the
                // previous element's prev and next pointers have to be updated
                else
                {
                    mergeInto.children[childInsertPos].Next = 
                           mergeInto.children[childInsertPos - 1].Next;
                    mergeInto.children[childInsertPos].Previous =
                          mergeInto.children[childInsertPos];

                    mergeInto.children[childInsertPos - 1].Next = mergeInto.children[childInsertPos];
                    mergeInto.children[childInsertPos].Previous = mergeInto.children[childInsertPos - 1];
                }
            }

        }

        public static int PrintTree(BPlusTree tree)
        {
            List<Block> queue = new List<Block>();
            queue.Add(tree.rootBlock);
            queue.Add(null);
            Block curr = null;
            int levelNumber =2;
            Console.WriteLine("Printing level 1");
            numNodes = 0;
            while (queue != null)
            {
                curr = poll(ref queue, 0);
                if (null == curr)
                {
                    queue.Add(null);
                    if (queue[0] == null)
                    {
                        break;
                    }
                    Console.WriteLine("\n" + "Printing level {0}",levelNumber++);
                    continue;
                }

                printNode(curr);
                numNodes++;
                if (curr.children == null)
                {
                    break;
                }
                for (int i = 0; i < curr.children.Count(); i++)
                {
                    queue.Add(curr.children[i]);
                }
            }

            curr = curr.Next;
            while (null != curr)
            {
                printNode(curr);
                curr = curr.Next;
            }
            Levels = levelNumber;
            return levelNumber;

        }

        public static Block poll(ref List<Block> queue, int index)
        {
            var result = queue[index];
            queue.RemoveAt(index);
            return result;
        }

        private static void printNode(Block curr)
        {
            for (int i = 0; i < curr.Nodes.Count; i++)
            {
                Console.Write( "{0} :(", curr.Nodes[i].Key );
                string values = "";
                for (int j = 0; j < curr.Nodes[i].KeyPointers.Count; j++)
                {
                    values = values + curr.Nodes[i].KeyPointers[j].ToString() + ",";
                }
                if(values != null)
                    Console.Write("{0} );", values.Substring(0, values.Length - 1));
            }
            Console.Write("||");
        }

        public static int GetMaxKeys()
        {
            decimal maxKeys;
            int blockSize = BlockController.BlockSize;
            int blockAddressSize = BlockController.BlockAddressSize;
            // Get size left for keys and pointers in a node after accounting for node's isLeaf 
            Dictionary<byte[], string> memory = MemoryAddressController.GetAddresses();
            byte[] pointerSize = memory.Keys.Last();
            int keySize = sizeof(int);
            maxKeys = Math.Floor(Convert.ToDecimal(blockSize - blockAddressSize - keySize) / (pointerSize.Length + keySize));
            return (int)maxKeys;
        }


    }
}
