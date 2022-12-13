using System.Numerics;

public class Day_11
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_11_Input.txt");

        //ExtraParser(lines);
        Day_11_01(lines);
    }

    void ExtraParser(string[] input)
    {
        //List 1: item num
        //List 2: Transfer data
        //Tuple: 1: from 2: to
        List<List<Tuple<int, int>>> data = new();

        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            int itemId = Convert.ToInt32(splitInput[2]);
            int from = Convert.ToInt32(splitInput[4]);
            int to = Convert.ToInt32(splitInput[6]);
            if (data.Count == itemId)
            {
                data.Add(new());
            }

            Tuple<int, int> transfer = new(from, to);

            data[itemId].Add(transfer);
        }


        for (int i = 0; i < data.Count; i++)
        {
            for(int j = 0; j < data[i].Count; j++)
            {
                Console.WriteLine($"{i}: {data[i][j].Item1} -> {data[i][j].Item2}");
            }
            Console.WriteLine();
        }
}

    void Day_11_01(string[] input)
    {
        List<Monkey> monkeys = new List<Monkey>();
        
        List<BigInteger> divisors = new List<BigInteger>();
        int itemId = 0;

        int monkeyNum = 0;
        Queue<MonkeyItem> startingItems = new Queue<MonkeyItem>();
        Monkey.InspectOperation curInspectOperation = null;
        BigInteger curDivisibleBy = 0;
        int curTrueMonkey = -1;
        int curFalseMonkey = -1;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == "" || input[i] == "\n")
            {
                Monkey newMonkey = new Monkey(curInspectOperation, curDivisibleBy, curTrueMonkey, curFalseMonkey, startingItems);
                monkeys.Add(newMonkey);

                startingItems = new Queue<MonkeyItem>();
                curInspectOperation = null;
                curDivisibleBy = 0;
                curTrueMonkey = -1;
                curFalseMonkey = -1;

                monkeyNum++;
                continue;
            }

            string[] splitInput = input[i].Split(' ');
            List<string> splittedInput = new List<string>();
            for(int j = 0; j < splitInput.Length; j++)
            {
                if (splitInput[j] != "")
                {
                    splittedInput.Add(splitInput[j]);
                }
            }
            splitInput = splittedInput.ToArray();

            switch (splitInput[0])
            {
                case ("Monkey"):
                    Console.WriteLine("Monkey :)");
                    break;

                case ("Starting"):
                    for(int j = 2; j < splitInput.Length; j++)
                    {
                        string curItem;

                        if (splitInput[j].Contains(','))
                        {
                            curItem = splitInput[j].Remove(splitInput[j].Length - 1);
                        }
                        else
                        {
                            curItem = splitInput[j];
                        }

                        startingItems.Enqueue(new MonkeyItem(itemId, Convert.ToUInt32(curItem)));
                        itemId++;
                    }
                    break;

                case ("Operation:"):

                    Monkey.InspectOperation inspectOperation = null;

                    if (splitInput[4] == "+")
                    {
                        if (splitInput[5] == "old")
                        {
                            inspectOperation = (ref BigInteger myItem) => myItem += myItem;
                        }
                        else
                        {
                            inspectOperation = (ref BigInteger myItem) => myItem += Convert.ToUInt32(splitInput[5]);
                        }
                    }
                    else if (splitInput[4] == "*")
                    {
                        if (splitInput[5] == "old")
                        {
                            inspectOperation = (ref BigInteger myItem) => myItem *= myItem;
                        }
                        else
                        {
                            inspectOperation = (ref BigInteger myItem) => myItem *= Convert.ToUInt32(splitInput[5]);
                        }
                    }

                    curInspectOperation = inspectOperation;
                    break;

                case ("Test:"):
                    curDivisibleBy = Convert.ToUInt32(splitInput[3]);
                    divisors.Add(curDivisibleBy);

                    i++;
                    splitInput = input[i].Split(' ');
                    splittedInput.Clear();
                    for (int j = 0; j < splitInput.Length; j++)
                    {
                        if (splitInput[j] != "")
                        {
                            splittedInput.Add(splitInput[j]);
                        }
                    }
                    splitInput = splittedInput.ToArray();

                    curTrueMonkey = Convert.ToInt32(splitInput[5]);

                    i++;
                    splitInput = input[i].Split(' ');
                    splittedInput.Clear();
                    for (int j = 0; j < splitInput.Length; j++)
                    {
                        if (splitInput[j] != "")
                        {
                            splittedInput.Add(splitInput[j]);
                        }
                    }
                    splitInput = splittedInput.ToArray();

                    curFalseMonkey = Convert.ToInt32(splitInput[5]);
                    break;
            }
        }

        Monkey newM = new Monkey(curInspectOperation, curDivisibleBy, curTrueMonkey, curFalseMonkey, startingItems);
        monkeys.Add(newM);

        
        for (int i = 0; i < monkeys.Count; i++)
        {
            monkeys[i].monkeyId = i;
            monkeys[i].monkeys = monkeys;
        }

        
        BigInteger lowestDiv = divisors[0];

        for(int i = 1; i < divisors.Count; i++)
        {
            BigInteger newLowestDiv = GetGCD(lowestDiv, divisors[i]);
            if(newLowestDiv == 1)
            {
                lowestDiv *= divisors[i];
            }
            else
            {
                lowestDiv = newLowestDiv;
            }
            //lowestDiv *= divisors[i];
        }
        Console.WriteLine(lowestDiv);
        //lowestDiv += 1;
        

        //Force Monkey Items
        //monkeys[0].items = new Queue<MonkeyItem>( new MonkeyItem[] { new MonkeyItem(0, 79) } );
        //monkeys[1].items = new Queue<MonkeyItem>();
        //monkeys[2].items = new Queue<MonkeyItem>();
        //monkeys[3].items = new Queue<MonkeyItem>();



        //Be monkeys
        for (int i = 0; i < 10000; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                monkeys[j].MonkeyAround2(lowestDiv);
            }

            //Console.WriteLine(i);
            
            if (i == 0 || i == 19 || i == 999 || i == 1999 || i == 2999 || i == 3999 || i == 4999 || i == 5999 || i == 6999 || i == 7999 || i == 8999 || i == 9999)
            {
                Console.WriteLine("Iteration " + (i + 1));
                for (int j = 0; j < monkeys.Count; j++)
                {
                    Console.Write($"Monkey {j}: {monkeys[j].businessCounter}");

                    Console.WriteLine();
                }
            }
            
            
        }

        for (int i = 0; i < monkeys.Count; i++)
        {
            Console.WriteLine($"{i}: {monkeys[i].businessCounter}");
        }
    }

    public class Monkey
    {
        public int monkeyId = 0;
        public List<Monkey> monkeys = null;

        public Queue<MonkeyItem> items = new Queue<MonkeyItem>();
        public delegate void InspectOperation(ref BigInteger item);
        public InspectOperation Inspect;

        public delegate bool DetermineFate(int item);
        public BigInteger divisibleBy;

        public int trueMonkey;
        public int passedToTrue = 0;
        public int falseMonkey;
        public int passedToFalse = 0;

        public int businessCounter = 0;
        public int itemsHeld = 0;

        public Monkey(InspectOperation inspect, BigInteger determineFate, int trueMonkey, int falseMonkey, Queue<MonkeyItem> startingItems = null)
        {
            Inspect = inspect;
            divisibleBy = determineFate;
            this.trueMonkey = trueMonkey;
            this.falseMonkey = falseMonkey;

            if(startingItems != null)
            {
                items = startingItems;
                itemsHeld += items.Count;
            }
        }

        public void MonkeyAround()
        {
            int numItems = items.Count;
            for(int i = 0; i < numItems; i++)
            {
                businessCounter++;

                MonkeyItem curItem = items.Dequeue();
                Inspect(ref curItem.worry);

                //Relief
                curItem.worry /= 3;

                if(curItem.worry % divisibleBy == 0)
                {
                    monkeys[trueMonkey].items.Enqueue(curItem);
                }
                else
                {
                    monkeys[falseMonkey].items.Enqueue(curItem);
                }
            }
        }

        public void MonkeyAround2(BigInteger round)
        {
            int numItems = items.Count;
            for (int i = 0; i < numItems; i++)
            {

                MonkeyItem curItem = items.Dequeue();
                Inspect(ref curItem.worry);

                businessCounter++;

                //Relief
                curItem.worry %= round;

                if (curItem.worry % divisibleBy == 0)
                {
                    monkeys[trueMonkey].items.Enqueue(curItem);
                    monkeys[trueMonkey].itemsHeld++;
                }
                else
                {
                    monkeys[falseMonkey].items.Enqueue(curItem);
                    monkeys[falseMonkey].itemsHeld++;
                }
            }
        }
    }

    public class MonkeyItem
    {
        public int itemId;
        public BigInteger worry;

        public MonkeyItem(int monkeyNum, BigInteger initWorry)
        {
            this.itemId = monkeyNum;
            this.worry = initWorry;
        }
    }

    static BigInteger GetGCD(BigInteger num1, BigInteger num2)

    {

        while (num1 != num2)

        {

            if (num1 > num2)

                num1 = num1 - num2;



            if (num2 > num1)

                num2 = num2 - num1;

        }

        return num1;

    }
}