public class Day_20
{
    public const long PART_TWO_PRIME = 811589153;
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_20_Input.txt");

        Day_20_02(lines);
        //PART TWO INCORRECT:
        //3033720253914
        //9404695104964
    }

    void Day_20_01(string[] input)
    {
        CircularLinkedList data = new();
        Queue<CircularLinkedListNode> nodes = new();

        for (int i = 0; i < input.Length; i++)
        {
            data.InsertTail(Convert.ToInt64(input[i]));
            nodes.Enqueue(data.Tail);
        }

        while (nodes.Count > 0)
        {
            data.PrintList();
            data.ShiftList(nodes.Dequeue());
        }
        data.PrintList();


        var tpl = data.FindNodeWithVal(0);
        int ind = tpl.Item1;
        CircularLinkedListNode zeroNode = tpl.Item2;
        CircularLinkedListNode oneNode = data.GetNodeAhead(zeroNode, 1000);
        CircularLinkedListNode twoNode = data.GetNodeAhead(oneNode, 1000);
        CircularLinkedListNode threeNode = data.GetNodeAhead(twoNode, 1000);

        Console.WriteLine((oneNode.val + twoNode.val + threeNode.val));
    }

    void Day_20_02(string[] input)
    {
        CircularLinkedList data = new();
        List<CircularLinkedListNode> nodeQueue = new();

        for (int i = 0; i < input.Length; i++)
        {
            data.InsertTail((Convert.ToInt64(input[i]) * PART_TWO_PRIME));
            nodeQueue.Add(data.Tail);
        }


        data.PrintList();
        //Mixer
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < nodeQueue.Count; j++)
            {
                CircularLinkedListNode curNode = nodeQueue[j];
                data.ShiftList(curNode);
                //data.PrintList();
            }

            data.PrintList();
            //Console.WriteLine("-------------------------");
        }


        var tpl = data.FindNodeWithVal(0);
        CircularLinkedListNode zeroNode = tpl.Item2;
        CircularLinkedListNode oneNode = data.GetNodeAhead(zeroNode, 1000);
        CircularLinkedListNode twoNode = data.GetNodeAhead(oneNode, 1000);
        CircularLinkedListNode threeNode = data.GetNodeAhead(twoNode, 1000);

        Console.WriteLine((oneNode.val + twoNode.val + threeNode.val));
    }

    public class CircularLinkedList
    {
        public const bool PART_TWO_ACTIVE = false;

        public CircularLinkedListNode Head = null;
        public CircularLinkedListNode Tail = null;
        public int Count = 0;

        public CircularLinkedList() { }

        public CircularLinkedList(CircularLinkedList other)
        {
            CircularLinkedListNode otherNode = other.Head;
            for(int i = 0; i < other.Count; i++)
            {
                InsertTail(otherNode.val);
                otherNode = otherNode.Next;
            }
        }

        public void InsertTail(long v)
        {
            CircularLinkedListNode newNode = new CircularLinkedListNode(v);
            if(Head == null)
            {
                Head = newNode;
                Tail = newNode;

                newNode.Next = newNode;
                newNode.Prev = newNode;
            }
            else
            {
                Tail.Next = newNode;
                newNode.Prev = Tail;
                newNode.Next = Head;
                Tail = newNode;

                Head.Prev = newNode;
            }

            Count++;
        }

        public CircularLinkedListNode GetNodeAtIndex(int i)
        {
            if(Head == null)
            {
                return null;
            }

            CircularLinkedListNode curNode = Head;
            while(i > 0)
            {
                curNode = curNode.Next;
                i--;
            }

            return curNode;
        }

        public void RemoveNodeAtIndex(int i)
        {
            CircularLinkedListNode toRemove = GetNodeAtIndex(i);
            if(toRemove == null)
            {
                return;
            }

            toRemove.Prev.Next = toRemove.Next;
            toRemove.Next.Prev = toRemove.Prev;

            if(toRemove == Head)
            {
                if(toRemove.Next == toRemove)
                {
                    Head = null;
                }
                else
                {
                    Head = toRemove.Next;
                }
            }
            if(toRemove == Tail)
            {
                if (toRemove.Prev == toRemove)
                {
                    Tail = null;
                }
                else
                {
                   Tail = toRemove.Prev;
                }
            }

            Count--;
        }

        public Tuple<int, CircularLinkedListNode> FindNodeWithVal(long v)
        {
            if(Head == null)
            {
                return new(-1, null);
            }

            CircularLinkedListNode curNode = Head;

            int c = 0;
            while(c < Count)
            {
                if(curNode.val == v)
                {
                    return new(c, curNode);
                }

                curNode = curNode.Next;

                c++;
            }

            return new(-1, null);
        }

        public void PrintList()
        {
            CircularLinkedListNode curNode = Head;

            int c = 0;
            while (c < Count)
            {
                long curVal = curNode.val;
                if (PART_TWO_ACTIVE)
                {
                    curVal *= PART_TWO_PRIME;
                }

                if (c == Count - 1)
                {
                    Console.Write(curVal);
                }
                else
                {
                    Console.Write(curVal + ", ");
                }

                curNode = curNode.Next;

                c++;
            }

            Console.WriteLine();
        }

        public int GetIndexOfNode(CircularLinkedListNode targetNode)
        {
            int i = 0;
            CircularLinkedListNode curNode = Head;
            while(i < Count)
            {
                if(curNode == targetNode)
                {
                    return i;
                }

                curNode = curNode.Next;

                i++;
            }

            throw new Exception("Target not found!");
        }

        public void ShiftList(CircularLinkedListNode curNode)
        {
            bool isNeg = curNode.val < 0;
            long storedVal = curNode.val % (Count - 1);

            //Only one (or none!) of these whiles will ever run :P
            //The right-shift while
            while (storedVal > 0)
            {
                ShiftRight(curNode);

                storedVal--;
            }

            //The left-shift while
            while (storedVal < 0)
            {
                ShiftRight(curNode.Prev);

                storedVal++;
            }
        }

        private void ShiftRight(CircularLinkedListNode curNode)
        {
            if (Head != Tail)
            {
                if (curNode == Head)
                {
                    Head = curNode.Next;
                }
                else if (curNode == Tail)
                {
                    Head = curNode;
                    Tail = curNode.Next;
                }
                else if (curNode.Next == Tail)
                {
                    Tail = curNode;
                }
            }

            CircularLinkedListNode nextNext = curNode.Next.Next;

            curNode.Next.Prev = curNode.Prev;
            curNode.Next.Next = curNode;
            curNode.Prev.Next = curNode.Next;

            curNode.Prev = curNode.Next;

            curNode.Next = nextNext;
            nextNext.Prev = curNode;
        }

        public CircularLinkedListNode GetNodeAhead(CircularLinkedListNode startingNode, int travelDist)
        {
            while(travelDist > 0)
            {
                startingNode = startingNode.Next;

                travelDist--;
            }

            return startingNode;
        }
    }

    public class CircularLinkedListNode
    {
        public long val;
        public CircularLinkedListNode Next;
        public CircularLinkedListNode Prev;

        public CircularLinkedListNode(long v, CircularLinkedListNode n = null, CircularLinkedListNode p = null)
        {
            val = v;
            Next = n;
            Prev = p;
        }
    }
}