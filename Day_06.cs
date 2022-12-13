using System.Text.RegularExpressions;

public class Day_06
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_06_Input.txt");

        
        string result = Day_06_01(lines[0]);


        Console.WriteLine("Total: " + result);

        
    }

    public string Day_06_01(string input)
    {
        //Match x = Regex.Match(input, @"(.)(?!(\1))(?!(\1 | \2))(?!(\1 | \2 | \3))");
        //Console.WriteLine(x);
        //Console.WriteLine(x.Length - 4);

        /*
        foreach (Match match in Regex.Matches(input, @"((.)(?!(\2))(?!(\2 | \3))(?!(\2 | \3 | \4)))"))
            Console.WriteLine("Found '{0}' at position {1}.",
                              match.Value, match.Index);
        */

        Queue<char> chars = new Queue<char>();
        for(int i = 0; i < 13; i++)
        {
            chars.Enqueue(input[i]);
        }

        bool hasDuplicates = HasDuplicates(chars);
        for(int i = 13; i < input.Length; i++)
        {
            /*
            foreach(char c in chars)
            {
                Console.Write(c);
            }
            Console.WriteLine();
            */

            chars.Enqueue(input[i]);
            hasDuplicates = HasDuplicates(chars);

            if (!hasDuplicates)
            {
                Console.WriteLine(input.Substring(i - 13, 14));
                Console.WriteLine(i);
                return "";
            }

            chars.Dequeue();
            
        }
        return "";
    }

    bool HasDuplicates(Queue<char> chars)
    {
        List<char> list = chars.ToList();

        for(int i = 0; i < list.Count; i++)
        {
            for(int j = i + 1; j < list.Count; j++)
            {
                if (list[i] == list[j])
                {
                    return true;
                }
            }
        }

        return false;
    }
}
