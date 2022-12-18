using System.Drawing;

public class Day_17
{
    static ulong TOTAL_ROCKS = 1000000000000;
    //static ulong TOTAL_ROCKS = 15000;

    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_17_Input.txt");

        //Day_17_02(lines);

        Day_17_02(lines);
    }

    void Day_17_01(string[] input)
    {
        int jetCounter = 0;
        string jetLine = input[0];
        int savedJetCounter = -1;

        int width = 7;
        ulong curTop = 0;

        int rockCounter = 0;
        ulong allCounter = 1;

        List<ULongPoint> settledRocks = new() { new ULongPoint(0, 0), new ULongPoint(1, 0), new ULongPoint(2, 0), new ULongPoint(3, 0), new ULongPoint(4, 0), new ULongPoint(5, 0), new ULongPoint(6, 0) };
        List<ULongPoint> curRock = GenerateShape(ref rockCounter, curTop);

        ulong lastCurTop = 0;
        ulong lastAllCounter = 0;

        List<ulong> heightDiffs = new();
        while(allCounter <= TOTAL_ROCKS)
        {
            //Move left/right
            if(GetMoveDir(jetLine, ref jetCounter) > 0)
            {
                //Move right
                bool canMoveRight = true;
                foreach(ULongPoint p in curRock)
                {
                    if(p.X == 6 || settledRocks.Contains(new ULongPoint(p.X + 1, p.Y)))
                    {
                        canMoveRight = false;
                        break;
                    }
                }

                if (canMoveRight)
                {
                    for(int j = 0; j < curRock.Count; j++)
                    {
                        curRock[j] = new ULongPoint(curRock[j].X + 1, curRock[j].Y);
                    }
                }
            }
            else
            {
                //Move left
                bool canMoveLeft = true;
                foreach (ULongPoint p in curRock)
                {
                    if (p.X == 0 || settledRocks.Contains(new ULongPoint(p.X - 1, p.Y)))
                    {
                        canMoveLeft = false;
                        break;
                    }
                }

                if (canMoveLeft)
                {
                    for (int j = 0; j < curRock.Count; j++)
                    {
                        curRock[j] = new ULongPoint(curRock[j].X - 1, curRock[j].Y);
                    }
                }
            }


            //Move down
            bool canMoveDown = true;
            foreach (ULongPoint p in curRock)
            {
                if (settledRocks.Contains(new ULongPoint(p.X, p.Y - 1)))
                {
                    canMoveDown = false;
                    break;
                }
            }

            if (canMoveDown)
            {
                for (int j = 0; j < curRock.Count; j++)
                {
                    curRock[j] = new ULongPoint(curRock[j].X, curRock[j].Y - 1);
                }
            }
            else
            {
                ulong greatestY = 0;
                foreach(ULongPoint p in curRock)
                {
                    if(p.Y > greatestY)
                    {
                        greatestY = p.Y;
                    }
                }
                curTop = greatestY > curTop ? greatestY : curTop;

                settledRocks.AddRange(curRock);

                /*
                if(allCounter == 2778)
                {
                    savedJetCounter = jetCounter;
                    Console.WriteLine($"Top Diff: ({curTop - lastCurTop}) :: jetCounter: ({jetCounter}) :: rocks placed: ({allCounter})");
                    lastCurTop = curTop;
                }
                else if (allCounter > 2778 && (allCounter - 2778) % 1720 == 0 && jetCounter == savedJetCounter)
                {
                    Console.WriteLine($"Top Diff: ({curTop - lastCurTop}) :: jetCounter: ({jetCounter}) :: rocks placed: ({allCounter})");
                    lastCurTop = curTop;
                }
                */

                Console.Write($"{curTop - lastCurTop}");
                lastCurTop = curTop;

                if (settledRocks.Count > 2500)
                {
                    settledRocks.RemoveRange(0, 500);
                }
                
                //heightDiffs.Add(curTop - lastCurTop);
                //lastCurTop = curTop;


                curRock = GenerateShape(ref rockCounter, curTop);
                allCounter++;


            }
        }

        Console.WriteLine("TOP: " + curTop);
    }

    void Day_17_02(string[] input)
    {
        ulong curTop = 0;
        ulong rockCounter = 0;

        for(int i = 0; i < input[2].Length; i++)
        {
            rockCounter++;
            curTop += Convert.ToUInt64("" + input[2][i]);
        }


        

        ulong combinedTop = 0;
        ulong combinedRockCounter = 0;

        for (int i = 0; i < input[4].Length; i++)
        {
            combinedRockCounter++;
            combinedTop += Convert.ToUInt64("" + input[4][i]);
        }

        ulong remainingRockAdds = ((TOTAL_ROCKS - rockCounter) / combinedRockCounter);

        curTop += combinedTop * remainingRockAdds;
        rockCounter += combinedRockCounter * remainingRockAdds;

        Console.WriteLine("TOP + ROCK: " + curTop + " : " + rockCounter);

        int c = 0;
        while (rockCounter < TOTAL_ROCKS)
        {
            curTop += Convert.ToUInt64("" + input[4][c]);
            rockCounter++;

            c++;
            c %= input[4].Length;
        }

        //GIVEN ANSWER: 1589142857183
        Console.WriteLine("TOP & ROCKS = " + curTop + " : " + rockCounter);
    }

    public List<ULongPoint> GenerateShape(ref int counter, ulong curTop)
    {
        if(counter == 5)
        {
            counter = 0;
        }
        counter++;
        switch((counter - 1) % 5)
        {
            default:
            case 0:
                return new List<ULongPoint>() { 
                new ULongPoint(2, curTop + 4),
                new ULongPoint(3, curTop + 4),
                new ULongPoint(4, curTop + 4),
                new ULongPoint(5, curTop + 4) };
                break;

            case 1:
                return new List<ULongPoint>() {
                new ULongPoint(2, curTop + 5),
                new ULongPoint(3, curTop + 5),
                new ULongPoint(4, curTop + 5),
                new ULongPoint(3, curTop + 4),
                new ULongPoint(3, curTop + 6) };
                break;

            case 2:
                return new List<ULongPoint>() {
                new ULongPoint(2, curTop + 4),
                new ULongPoint(3, curTop + 4),
                new ULongPoint(4, curTop + 4),
                new ULongPoint(4, curTop + 5),
                new ULongPoint(4, curTop + 6) };
                break;

            case 3:
                return new List<ULongPoint>() {
                new ULongPoint(2, curTop + 4),
                new ULongPoint(2, curTop + 5),
                new ULongPoint(2, curTop + 6),
                new ULongPoint(2, curTop + 7) };
                break;

            case 4:
                return new List<ULongPoint>() {
                new ULongPoint(2, curTop + 4),
                new ULongPoint(3, curTop + 4),
                new ULongPoint(2, curTop + 5),
                new ULongPoint(3, curTop + 5) };
                break;
        }
    }

    public int GetMoveDir(string jetline, ref int counter)
    {
        int move;
        if (jetline[counter] == '<')
        {
            move = -1;
        }
        else
        {
            move = 1;
        }

        counter++;
        counter %= jetline.Length;
        return move;
    }

    public struct ULongPoint
    {
        public ulong X;
        public ulong Y;

        public ULongPoint(ulong x, ulong y)
        {
            X = x;
            Y = y;
        }
    }

    //Gets the ULongPoint with the specified point if it can
    public static Predicate<ULongPoint> GetNodeByPos(ULongPoint pos)
    {
        return (node) => node.X == pos.X && node.Y == pos.Y;
    }
}