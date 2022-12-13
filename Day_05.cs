public class Day_05
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_05_Input.txt");

        Dictionary<char, int> charMap = new Dictionary<char, int>();
        for (int i = (int)'a'; i <= (int)'z'; i++)
        {
            charMap.Add((char)i, i - (int)'a' + 1);
        }
        for (int i = (int)'A'; i <= (int)'Z'; i++)
        {
            charMap.Add((char)i, i - (int)'A' + 27);
        }

        /*
        foreach(char key in charMap.Keys)
        {
            Console.WriteLine(key + " : " + charMap[key]);
        }
        */

        //int total = 0;
        string result = Day_05_02(lines);


        Console.WriteLine("Total: " + result);

        string Day_05_02(string[] input)
        {
            List<Stack<char>> stacks = new List<Stack<char>>();

            for (int i = 0; i < 9; i++)
            {
                stacks.Add(new Stack<char>());
            }

            stacks[0].Push('D');
            stacks[0].Push('H');
            stacks[0].Push('N');
            stacks[0].Push('Q');
            stacks[0].Push('T');
            stacks[0].Push('W');
            stacks[0].Push('V');
            stacks[0].Push('B');

            stacks[1].Push('D');
            stacks[1].Push('W');
            stacks[1].Push('B');

            stacks[2].Push('T');
            stacks[2].Push('S');
            stacks[2].Push('Q');
            stacks[2].Push('W');
            stacks[2].Push('J');
            stacks[2].Push('C');

            stacks[3].Push('F');
            stacks[3].Push('J');
            stacks[3].Push('R');
            stacks[3].Push('N');
            stacks[3].Push('Z');
            stacks[3].Push('T');
            stacks[3].Push('P');

            stacks[4].Push('G');
            stacks[4].Push('P');
            stacks[4].Push('V');
            stacks[4].Push('J');
            stacks[4].Push('M');
            stacks[4].Push('S');
            stacks[4].Push('T');

            stacks[5].Push('B');
            stacks[5].Push('W');
            stacks[5].Push('F');
            stacks[5].Push('T');
            stacks[5].Push('N');

            stacks[6].Push('B');
            stacks[6].Push('L');
            stacks[6].Push('D');
            stacks[6].Push('Q');
            stacks[6].Push('F');
            stacks[6].Push('H');
            stacks[6].Push('V');
            stacks[6].Push('N');

            stacks[7].Push('H');
            stacks[7].Push('P');
            stacks[7].Push('F');
            stacks[7].Push('R');

            stacks[8].Push('Z');
            stacks[8].Push('S');
            stacks[8].Push('M');
            stacks[8].Push('B');
            stacks[8].Push('L');
            stacks[8].Push('N');
            stacks[8].Push('P');
            stacks[8].Push('H');

            for (int i = 10; i < input.Length; i++)
            {
                string[] pieces = input[i].Split(' ');

                int amount = Convert.ToInt32(pieces[1]);
                int from = Convert.ToInt32(pieces[3]) - 1;
                int to = Convert.ToInt32(pieces[5]) - 1;

                List<char> popped = new List<char>();
                for (int j = 0; j < amount; j++)
                {
                    popped.Add(stacks[from].Pop());
                }

                for (int j = popped.Count - 1; j >= 0; j--)
                {
                    stacks[to].Push(popped[j]);
                }
            }

            string x = "";
            for (int i = 0; i < stacks.Count; i++)
            {
                x += stacks[i].Pop();
            }

            return x;
        }

    }
}
