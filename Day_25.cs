using System.Drawing;

public class Day_25
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_25_Input.txt");

        Day_25_01(lines);
    }

    public string GetSNAFU(long num, int highestPow)
    {
        string s = "";

        long remainingSum = num;
        for (int i = highestPow; i >= 0; i--)
        {
            long pow = (long)Math.Pow(5, i);

            if (remainingSum == 0)
            {
                s += "0";
            }
            else if (remainingSum == pow * 2)
            {
                s += "2";
                remainingSum -= pow * 2;
            }
            else if (remainingSum == pow)
            {
                s += "1";
                remainingSum -= pow;
            }
            else if (remainingSum == -(pow * 2))
            {
                s += "=";
                remainingSum += pow * 2;
            }
            else if (remainingSum == -(pow))
            {
                s += "-";
                remainingSum += pow;
            }
            else
            {
                long lowPow = GetMax(0, i - 1);
                if(remainingSum > 0)
                {
                    if (remainingSum > pow * 2 || remainingSum >= pow * 2 - lowPow)
                    {
                        s += "2";
                        remainingSum -= pow * 2;
                    }
                    else if (remainingSum > pow || remainingSum >= pow - lowPow)
                    {
                        s += "1";
                        remainingSum -= pow;
                    }
                    else
                    {
                        s += "0";
                    }
                }
                else
                {
                    if(remainingSum < -(pow * 2) || remainingSum <= -(pow * 2) + lowPow)
                    {
                        s += "=";
                        remainingSum += pow * 2;
                    }
                    else if (remainingSum < -(pow) || remainingSum <= -(pow) + lowPow)
                    {
                        s += "-";
                        remainingSum += pow;
                    }
                    else
                    {
                        s += "0";
                    }
                }
            }
        }

        return s;
        /*
        Console.WriteLine(s);

        long newNum = 0;
        for (int j = 0; j < s.Length; j++)
        {
            char curChar = s[^(j + 1)];

            if (curChar == '-')
            {
                newNum -= (long)Math.Pow(5, j);
            }
            else if (curChar == '=')
            {
                newNum -= (long)Math.Pow(5, j) * 2;
            }
            else
            {
                newNum += (long)Math.Pow(5, j) * Convert.ToInt64("" + curChar);
            }
        }

        Console.WriteLine(newNum);
        */
    }

    void Test2()
    {
        long num = 13;

        //Expected: 1-000

        string s = "";

        long remainingSum = num;
        int power = 0;
        
        while (remainingSum != 0)
        {
            //long pow = (long)Math.Pow(5, power);

            s = Convert.ToString(remainingSum % 5) + s;
            remainingSum /= 5;

            power++;
        }

        string newS = "";
        for(int i = 0; i < s.Length; i++)
        {
            switch (s[i])
            {
                case '4':
                    newS += '2';
                    break;
                case '3':
                    newS += '1';
                    break;
                case '2':
                    newS += '0';
                    break;
                case '1':
                    newS += '-';
                    break;
                case '0':
                    newS += '=';
                    break;
            }
        }

        Console.WriteLine(s);
        Console.WriteLine(newS);
        Console.WriteLine("=========");

        s = newS;

        long newNum = 0;
        for (int j = 0; j < s.Length; j++)
        {
            char curChar = s[^(j + 1)];

            if (curChar == '-')
            {
                newNum -= (long)Math.Pow(5, j);
            }
            else if (curChar == '=')
            {
                newNum -= (long)Math.Pow(5, j) * 2;
            }
            else
            {
                newNum += (long)Math.Pow(5, j) * Convert.ToInt64("" + curChar);
            }
        }

        Console.WriteLine(newNum);
    }

    public long GetMax(long curNum, int pow)
    {
        if(pow < 0)
        {
            return curNum;
        }
        if (pow == 0)
        {
            curNum += 2;
            return curNum;
        }
        curNum += (long)Math.Pow(5, pow) * 2;

        return GetMax(curNum, pow - 1);
    }

    void Day_25_01(string[] input)
    {
        List<long> fuelNums = new();

        for(int i = 0; i < input.Length; i++)
        {
            long curNum = 0;
            for(int j = 0; j < input[i].Length; j++)
            {
                char curChar = input[i][^(j + 1)];

                if(curChar == '-')
                {
                    curNum -= (long)Math.Pow(5, j);
                }
                else if(curChar == '=')
                {
                    curNum -= (long)Math.Pow(5, j) * 2;
                }
                else
                {
                    curNum += (long)Math.Pow(5, j) * Convert.ToInt64("" + curChar);
                }
            }

            fuelNums.Add(curNum);
        }

        //29361331235500

        long sum = fuelNums.Sum();
        Console.WriteLine(fuelNums.Sum());

        string s = GetSNAFU(sum, 19);
        Console.WriteLine(s);
        long newNum = 0;
        for (int j = 0; j < s.Length; j++)
        {
            char curChar = s[^(j + 1)];

            if (curChar == '-')
            {
                newNum -= (long)Math.Pow(5, j);
            }
            else if (curChar == '=')
            {
                newNum -= (long)Math.Pow(5, j) * 2;
            }
            else
            {
                newNum += (long)Math.Pow(5, j) * Convert.ToInt64("" + curChar);
            }
        }

        Console.WriteLine(newNum);

        /*
        long remainingSum = sum;
        int power = 0;
        for (int i = 19; i >= 0; i--)
        {
            long pow = (long)Math.Pow(5, i);

            if (remainingSum == 0)
            {
                s += "0";
            }
            else if (remainingSum == pow * 2)
            {
                s += "2";
                remainingSum -= pow * 2;
            }
            else if (remainingSum == pow)
            {
                s += "1";
                remainingSum -= pow;
            }
            else if (remainingSum == -(pow * 2))
            {
                s += "=";
                remainingSum += pow * 2;
            }
            else if (remainingSum == -(pow))
            {
                s += "-";
                remainingSum += pow;
            }
            else
            {
                if (pow > remainingSum)
                {
                    s += "1";
                    remainingSum -= pow;
                }
                else if (pow * 2 > remainingSum)
                {
                    s += "2";
                    remainingSum -= pow * 2;
                }
                else if (-(pow) < remainingSum)
                {
                    s += "-";
                    remainingSum += pow;
                }
                else if (-(pow * 2) < remainingSum)
                {
                    s += "-";
                    remainingSum += pow * 2;
                }
                else
                {
                    s += "0";
                }
            }
        }

        Console.WriteLine(s);

        long newNum = 0;
        for (int j = 0; j < s.Length; j++)
        {
            char curChar = s[^(j + 1)];

            if (curChar == '-')
            {
                newNum -= (long)Math.Pow(5, j);
            }
            else if (curChar == '=')
            {
                newNum -= (long)Math.Pow(5, j) * 2;
            }
            else
            {
                newNum += (long)Math.Pow(5, j) * Convert.ToInt64("" + curChar);
            }
        }

        Console.WriteLine(newNum);
        */
    }
}