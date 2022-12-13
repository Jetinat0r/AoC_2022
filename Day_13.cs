using System.Linq;

public class Day_13
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_13_Input.txt");

        Day_13_02(lines);
    }

    void Day_13_01(string[] input)
    {
        List<Tuple<ListObject, ListObject>> pairs = new List<Tuple<ListObject, ListObject>>();

        for(int i = 0; i < input.Length - 1; i += 3)
        {
            ListObject l = ParseLine(input[i]);
            ListObject r = ParseLine(input[i + 1]);

            Tuple<ListObject, ListObject> newPair = new(l, r);
            pairs.Add(newPair);
        }


        List<int> passed = new();
        for(int i = 0; i < pairs.Count; i++)
        {
            if (CompareListObjs(pairs[i].Item1.subObjects, pairs[i].Item2.subObjects) == Result.PASS)
            {
                passed.Add(i + 1);
            }
        }


        Console.WriteLine("COUNT: " + passed.Sum());
    }

    void Day_13_02(string[] input)
    {
        List<ListObject> packets = new List<ListObject>();

        for (int i = 0; i < input.Length - 1; i += 3)
        {
            ListObject l = ParseLine(input[i]);
            ListObject r = ParseLine(input[i + 1]);

            packets.Add(l);
            packets.Add(r);
        }

        //Make the divider packets
        ListObject div1 = new ListObject(true);
        div1.subObjects.Add(new ListObject(true));
        div1.subObjects[0].subObjects.Add(new ListObject(false, 2));

        ListObject div2 = new ListObject(true);
        div2.subObjects.Add(new ListObject(true));
        div2.subObjects[0].subObjects.Add(new ListObject(false, 6));

        packets.Add(div1);
        packets.Add(div2);


        List<ListObject> orderedPackets = new();
        while(packets.Count > 0)
        {
            ListObject minPacket = packets[0];
            for(int i = 1; i < packets.Count; i++)
            {
                Result r = CompareListObjs(minPacket.subObjects, packets[i].subObjects);
                if(r == Result.FAIL)
                {
                    minPacket = packets[i];
                }
            }

            orderedPackets.Add(minPacket);
            packets.Remove(minPacket);
        }

        int firstIndex = -1;
        int secondIndex = -1;
        ListObject searchPacket = div1;
        for (int i = 0; i < orderedPackets.Count; i++)
        {
            if (orderedPackets[i] == searchPacket)
            {
                if(firstIndex == -1)
                {
                    firstIndex = i + 1;
                    searchPacket = div2;
                }
                else
                {
                    secondIndex = i + 1;
                    break;
                }
            }
        }

        Console.WriteLine("KEY: " + (firstIndex * secondIndex));
    }

    public ListObject ParseLine(string input)
    {
        ListObject headObject = new ListObject(true);
        Stack<ListObject> stack = new Stack<ListObject>();
        stack.Push(headObject);

        for(int i = 1; i < input.Length - 1; i++)
        {
            ListObject c = stack.Peek();
            if (input[i] == '[')
            {
                ListObject newObj = new ListObject(true);
                c.subObjects.Add(newObj);
                stack.Push(newObj);
            }
            else if (input[i] == ']')
            {
                stack.Pop();
            }
            else if (input[i] != ',')
            {
                string num = "";
                num += input[i];

                i++;
                while(i < input.Length && input[i] != ',' && input[i] != '[' && input[i] != ']')
                {
                    num += input[i];
                    i++;
                }
                i--;

                ListObject newObj = new ListObject(false, Convert.ToInt32(num));
                c.subObjects.Add(newObj);
            }
        }

        return headObject;
    }

    public class ListObject
    {
        public bool IsList;

        public List<ListObject> subObjects = null;
        public int value = -1;

        public ListObject(bool isList, int val = -1)
        {
            this.IsList = isList;
            if (IsList)
            {
                subObjects = new();
            }

            value = val;
        }

        public List<ListObject> GetSubObjects()
        {
            return subObjects;
        }

        public int GetValue()
        {
            return value;
        }

        public void ConvertToList()
        {
            if (IsList)
            {
                return;
            }


            IsList = true;
            ListObject newVal = new ListObject(false);
            subObjects = new();
            subObjects.Add(newVal);

            newVal.value = value;

            value = -1;
        }
    }

    public enum Result
    {
        PASS = 0,
        FAIL = 1,
        NEUTRAL = 2,
    }

    public Result CompareListObjs(List<ListObject> left, List<ListObject> right)
    {
        if (left == null || left.Count == 0)
        {
            if (right == null || right.Count == 0)
            {
                return Result.NEUTRAL;
            }
            else
            {
                return Result.PASS;
            }
        }
        else if (right == null || right.Count == 0)
        {
            return Result.FAIL;
        }

        Result r;
        if (left[0].IsList || right[0].IsList)
        {
            left[0].ConvertToList();
            right[0].ConvertToList();

            r = CompareListObjs(left[0].subObjects, right[0].subObjects);
        }
        else
        {
            r = CompareVals(left[0], right[0]);
        }

        if(r != Result.NEUTRAL)
        {
            return r;
        }

        return CompareListObjs(left.GetRange(1, left.Count - 1), right.GetRange(1, right.Count - 1));
    }

    public Result CompareVals(ListObject left, ListObject right)
    {
        if(left.value == right.value)
        {
            return Result.NEUTRAL;
        }
        else if(left.value < right.value)
        {
            return Result.PASS;
        }
        else
        {
            return Result.FAIL;
        }
    }
}