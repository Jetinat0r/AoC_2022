using System.Diagnostics;

public class Day_08
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_08_Input.txt");

        int numTrials = 1;

        Stopwatch s1 = Stopwatch.StartNew();
        for(int i = 0; i < numTrials; i++)
        {
            Day_08_01(lines);
        }
        s1.Stop();

        Stopwatch s1b = Stopwatch.StartNew();
        for (int i = 0; i < numTrials; i++)
        {
            Day_08_01b(lines);
        }
        s1b.Stop();

        Stopwatch s2 = Stopwatch.StartNew();
        for (int i = 0; i < numTrials; i++)
        {
            Day_08_02(lines);
        }
        s2.Stop();

        TimeSpan ts1 = s1.Elapsed;
        TimeSpan ts1b = s1b.Elapsed;
        TimeSpan ts2 = s2.Elapsed;

        string elapsedTime1 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts1.Hours, ts1.Minutes, ts1.Seconds,
            ts1.Milliseconds);
        Console.WriteLine($"Day_08_01 {numTrials}x Run Time: {elapsedTime1}");

        string elapsedTime1b = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts1b.Hours, ts1b.Minutes, ts1b.Seconds,
            ts1b.Milliseconds);
        Console.WriteLine($"Day_08_01b {numTrials}x Run Time: {elapsedTime1b}");

        string elapsedTime2 = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts2.Hours, ts2.Minutes, ts2.Seconds,
            ts2.Milliseconds);
        Console.WriteLine($"Day_08_02 {numTrials}x Run Time: {elapsedTime2}");
    }

    public enum VisDir
    {
        Visible = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        Invisible = 5,
    }

    void Day_08_01(string[] input)
    {
        int[,] trees = new int[input.Length, input[0].Length];
        VisDir[,] visibilityMap = new VisDir[input.Length, input[0].Length];

        //Populate tree grid
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                trees[i, j] = input[i][j] - '0';
            }
        }

        int numVisible = 0;
        int numInvisible = 0;

        //Fill edges
        for (int i = 0; i < input.Length; i++)
        {
            numVisible++;
            visibilityMap[i, 0] = VisDir.Visible;

            numVisible++;
            visibilityMap[i, visibilityMap.GetLength(1) - 1] = VisDir.Visible;
        }
        for (int j = 1; j < input[0].Length - 1; j++)
        {
            numVisible++;
            visibilityMap[0, j] = VisDir.Visible;

            numVisible++;
            visibilityMap[visibilityMap.GetLength(0) - 1, j] = VisDir.Visible;
        }


        for (int i = 1; i < input.Length - 1; i++)
        {
            for (int j = 1; j < input[i].Length - 1; j++)
            {
                int curTreeVal = trees[i, j];
                bool isInvisible = true;


                //Check up
                for (int k = i - 1; k >= 0; k--)
                {
                    if (trees[k, j] >= curTreeVal)
                    {
                        break;
                    }

                    if (k == 0)
                    {
                        numVisible++;
                        visibilityMap[i, j] = VisDir.Up;
                        isInvisible = false;
                    }
                }

                if (!isInvisible)
                {
                    continue;
                }

                //Check down
                for (int k = i + 1; k < input.Length; k++)
                {
                    if (trees[k, j] >= curTreeVal)
                    {
                        break;
                    }

                    if (k == input.Length - 1)
                    {
                        numVisible++;
                        visibilityMap[i, j] = VisDir.Down;
                        isInvisible = false;
                    }
                }

                if (!isInvisible)
                {
                    continue;
                }

                //Check left
                for (int k = j - 1; k >= 0; k--)
                {
                    if (trees[i, k] >= curTreeVal)
                    {
                        break;
                    }

                    if (k == 0)
                    {
                        numVisible++;
                        visibilityMap[i, j] = VisDir.Left;
                        isInvisible = false;
                    }
                }

                if (!isInvisible)
                {
                    continue;
                }

                //Check right
                for (int k = j + 1; k < input[0].Length; k++)
                {
                    if (trees[i, k] >= curTreeVal)
                    {
                        break;
                    }

                    if (k == input[0].Length - 1)
                    {
                        numVisible++;
                        visibilityMap[i, j] = VisDir.Right;
                        isInvisible = false;
                    }
                }

                if (isInvisible)
                {
                    //Console.WriteLine("Actually Invisible!");
                    numInvisible++;
                    visibilityMap[i, j] = VisDir.Invisible;
                }
            }
        }

        
        for (int i = 0; i < visibilityMap.GetLength(0); i++)
        {
            for (int j = 0; j < visibilityMap.GetLength(1); j++)
            {
                char writtenChar;

                switch (visibilityMap[i, j])
                {
                    case (VisDir.Visible):
                        writtenChar = 'V';
                        break;
                    case (VisDir.Up):
                        writtenChar = 'U';
                        break;
                    case (VisDir.Down):
                        writtenChar = 'D';
                        break;
                    case (VisDir.Left):
                        writtenChar = 'L';
                        break;
                    case (VisDir.Right):
                        writtenChar = 'R';
                        break;
                    case (VisDir.Invisible):
                    default:
                        writtenChar = '_';
                        break;
                }

                Console.Write(writtenChar);
            }

            Console.WriteLine();
        }
        

        //Console.WriteLine(trees[0, 0]);
        Console.WriteLine("Num Invisible: " + numInvisible);
        Console.WriteLine("Num Visible: " + numVisible);
    }

    void Day_08_02(string[] input)
    {
        int[,] trees = new int[input.Length, input[0].Length];

        //Populate tree grid
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                trees[i, j] = input[i][j] - '0';
            }
        }

        List<int> viewDists = new List<int>();

        for (int i = 1; i < input.Length - 1; i++)
        {
            for (int j = 1; j < input[i].Length - 1; j++)
            {
                int curTreeVal = trees[i, j];


                //Check up
                int upViewDist = 0;
                for (int k = i - 1; k >= 0; k--)
                {
                    if (trees[k, j] >= curTreeVal)
                    {
                        upViewDist++;
                        break;
                    }

                    upViewDist++;
                }


                //Check down
                int downViewDist = 0;
                for (int k = i + 1; k < input.Length; k++)
                {
                    if (trees[k, j] >= curTreeVal)
                    {
                        downViewDist++;
                        break;
                    }

                    downViewDist++;
                }

                //Check left
                int leftViewDist = 0;
                for (int k = j - 1; k >= 0; k--)
                {
                    if (trees[i, k] >= curTreeVal)
                    {
                        leftViewDist++;
                        break;
                    }

                    leftViewDist++;
                }

                //Check right
                int rightViewDist = 0;
                for (int k = j + 1; k < input[0].Length; k++)
                {
                    if (trees[i, k] >= curTreeVal)
                    {
                        rightViewDist++;
                        break;
                    }

                    rightViewDist++;
                }

                viewDists.Add(upViewDist * downViewDist * leftViewDist * rightViewDist);
            }
        }

        Console.WriteLine("Max Dist: " + viewDists.Max());
        //Console.WriteLine(trees[0, 0]);
        //Console.WriteLine("Num Invisible: " + numInvisible);
        //Console.WriteLine("Num Visible: " + numVisible);
    }

    void Day_08_01b(string[] input)
    {
        int[,] trees = new int[input.Length, input[0].Length];
        bool[,] visibilityMap = new bool[input.Length, input[0].Length];
        for(int i = 0; i < visibilityMap.GetLength(0); i++)
        {
            for(int j = 0; j < visibilityMap.GetLength(1); j++)
            {
                if(i == 0 || i == visibilityMap.GetLength(0) - 1 || j == 0 || j == visibilityMap.GetLength(1) - 1)
                {
                    visibilityMap[i, j] = true;
                }
                else
                {
                    visibilityMap[i, j] = false;
                }
            }
        }

        //Populate tree grid
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                trees[i, j] = input[i][j] - '0';

            }
        }

        int numVisible = 0;
        //Add edges
        numVisible += (2 * input.Length);
        numVisible += (2 * (input[0].Length - 2));

        
        //Search Horizontal
        for(int i = 1; i < input.Length - 1; i++)
        {
            int curHighestTree = trees[i, 0];

            //Search L -> R
            for (int j = 1; j < input.Length - 1; j++)
            {
                if(trees[i, j] > curHighestTree)
                {
                    curHighestTree = trees[i, j];
                    if (visibilityMap[i, j] == false)
                    {
                        numVisible++;
                        visibilityMap[i, j] = true;
                    }
                }

                if(curHighestTree == 9)
                {
                    break;
                }
            }


            //Search R -> L
            curHighestTree = trees[i, input[i].Length - 1];
            for (int j = input.Length - 1; j > 0; j--)
            {
                if (trees[i, j] > curHighestTree)
                {
                    curHighestTree = trees[i, j];
                    if (visibilityMap[i, j] == false)
                    {
                        numVisible++;
                        visibilityMap[i, j] = true;
                    }
                }

                if (curHighestTree == 9)
                {
                    break;
                }
            }
        }




        //Search Vertical
        for (int j = 1; j < input[0].Length - 1; j++)
        {
            int curHighestTree = trees[0, j];

            //Search U -> D
            for (int i = 1; i < input.Length - 1; i++)
            {
                if (trees[i, j] > curHighestTree)
                {
                    curHighestTree = trees[i, j];
                    if (visibilityMap[i, j] == false)
                    {
                        numVisible++;
                        visibilityMap[i, j] = true;
                    }
                }

                if (curHighestTree == 9)
                {
                    break;
                }
            }


            //Search D -> U
            curHighestTree = trees[input[0].Length - 1, j];
            for (int i = input.Length - 1; i > 0; i--)
            {
                if (trees[i, j] > curHighestTree)
                {
                    curHighestTree = trees[i, j];
                    if (visibilityMap[i, j] == false)
                    {
                        numVisible++;
                        visibilityMap[i, j] = true;
                    }
                }

                if (curHighestTree == 9)
                {
                    break;
                }
            }
        }

        /*
        for (int i = 0; i < visibilityMap.GetLength(0); i++)
        {
            for (int j = 0; j < visibilityMap.GetLength(1); j++)
            {
                char writtenChar = visibilityMap[i, j] ? 'V' : '_';

                Console.Write(writtenChar);
            }

            Console.WriteLine();
        }
        */

        Console.WriteLine("Total Visible: " + numVisible);
    }
}
