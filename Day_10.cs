public class Day_10
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_10_Input.txt");

        Day_10_02(lines);
    }

    void Day_10_01(string[] input)
    {
        List<int> importantCycles = new List<int>() { 20, 60, 100, 140, 180, 220, 260, 300};
        int curCycle = 1;
        int curRegister = 1;

        List<int> signals = new List<int>();

        for (int i = 0; i < input.Length; i++)
        {
            string [] splitInput = input[i].Split(' ');

            if (splitInput[0] == "noop")
            {
                if (importantCycles.Contains(curCycle))
                {
                    signals.Add(curCycle * curRegister);
                    Console.WriteLine($"{curCycle}: {curCycle * curRegister}");
                }

                curCycle++;
            }
            else
            {
                for(int j = 0; j < 2; j++)
                {
                    if (importantCycles.Contains(curCycle))
                    {
                        signals.Add(curCycle * curRegister);
                        Console.WriteLine($"{curCycle}: {curCycle * curRegister}");
                    }

                    curCycle++;
                }

                curRegister += Convert.ToInt32(splitInput[1]);
            }
        }

        Console.WriteLine($"SUM: {signals.Sum()}");
    }

    void Day_10_02(string[] input)
    {
        List<int> importantCycles = new List<int>() { 40, 80, 120, 160, 200, 240 };
        int curCycle = 1;
        int curRegister = 1;

        bool[] display = new bool[240];

        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            if (splitInput[0] == "noop")
            {
                if(curRegister - 1 == (curCycle - 1) % 40 || curRegister == (curCycle - 1) % 40 || curRegister + 1 == (curCycle - 1) % 40)
                {
                    display[curCycle - 1] = true;
                }

                curCycle++;
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    if (curRegister - 1 == (curCycle - 1) % 40 || curRegister == (curCycle - 1) % 40 || curRegister + 1 == (curCycle - 1) % 40)
                    {
                        display[curCycle - 1] = true;
                    }

                    curCycle++;
                }

                curRegister += Convert.ToInt32(splitInput[1]);
            }
        }

        for(int i = 0; i < 240; i++)
        {
            if(i % 40 == 0)
            {
                Console.WriteLine();
            }

            Console.Write(display[i] ? "#" : ".");
        }
    }
}