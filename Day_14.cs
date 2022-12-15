using System.Drawing;

public class Day_14
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_14_Input.txt");

        Day_14_02(lines);
    }

    void Day_14_01(string[] input)
    {
        //400 -> 600 on the X
        const int minX = 400;
        //0 -> 180 on the Y

        FallingTile[,] map = new FallingTile[200, 180];

        //Parse input
        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(" -> ");

            List<Point> points = new List<Point>();
            for(int j = 0; j < splitInput.Length; j++)
            {
                string[] splittierInput = splitInput[j].Split(',');

                Point newPoint = new Point(Convert.ToInt32(splittierInput[0]) - minX, Convert.ToInt32(splittierInput[1]));
                points.Add(newPoint);
            }

            //Setup rock formations
            Point curPoint = points[0];
            for(int j = 1; j < points.Count; j++)
            {
                if(curPoint.X != points[j].X)
                {
                    //Move along X
                    int curX = curPoint.X;
                    int xDir = (points[j].X - curPoint.X) / Math.Abs(points[j].X - curPoint.X);

                    while (curX != points[j].X)
                    {
                        FallingTile nt = new FallingTile(new Point(curX, curPoint.Y), TileType.ROCK);
                        nt.atRest = true;
                        map[curX, curPoint.Y] = nt;

                        curX += xDir;
                    }

                    FallingTile finalt = new FallingTile(new Point(curX, curPoint.Y), TileType.ROCK);
                    finalt.atRest = true;
                    map[curX, curPoint.Y] = finalt;
                }
                else
                {
                    //Move along Y
                    int curY = curPoint.Y;
                    int yDir = (points[j].Y - curPoint.Y) / Math.Abs(points[j].Y - curPoint.Y);

                    while(curY != points[j].Y)
                    {
                        FallingTile nt = new FallingTile(new Point(curPoint.X, curY), TileType.ROCK);
                        nt.atRest = true;
                        map[curPoint.X, curY] = nt;

                        curY += yDir;
                    }

                    FallingTile finalt = new FallingTile(new Point(curPoint.X, curY), TileType.ROCK);
                    finalt.atRest = true;
                    map[curPoint.X, curY] = finalt;
                }

                curPoint = points[j];
            }
        }

        TileResult lastResult = TileResult.LANDED;
        int sandCounter = 0;
        FallingTile curSandTile = null;
        while(lastResult != TileResult.ABYSS)
        {
            switch (lastResult)
            {
                case(TileResult.LANDED):
                    //Make a new sand
                    curSandTile = new FallingTile(new Point(500 - minX, 0), TileType.SAND);
                    sandCounter++;

                    lastResult = curSandTile.SandFall(map);
                    break;

                case (TileResult.FALLING):
                    //Continue falling
                    lastResult = curSandTile.SandFall(map);
                    break;

                case (TileResult.ABYSS):
                    //Return!
                    //Actually it should never reach here...
                    Console.WriteLine("UH OH!");
                    break;
            }
        }

        //Print for fun!
        PrintMap(map);

        //Print final output!
        Console.WriteLine($"Sand Count: {sandCounter}");
    }

    void Day_14_02(string[] input)
    {
        //400 -> 600 on the X
        const int minX = -500;
        //0 -> 180 on the Y
        const int largestY = 179;

        FallingTile[,] map = new FallingTile[600 + (2 * -minX), 180];

        //Parse input
        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(" -> ");

            List<Point> points = new List<Point>();
            for (int j = 0; j < splitInput.Length; j++)
            {
                string[] splittierInput = splitInput[j].Split(',');

                Point newPoint = new Point(Convert.ToInt32(splittierInput[0]) - minX, Convert.ToInt32(splittierInput[1]));
                points.Add(newPoint);
            }

            //Setup rock formations
            Point curPoint = points[0];
            for (int j = 1; j < points.Count; j++)
            {
                if (curPoint.X != points[j].X)
                {
                    //Move along X
                    int curX = curPoint.X;
                    int xDir = (points[j].X - curPoint.X) / Math.Abs(points[j].X - curPoint.X);

                    while (curX != points[j].X)
                    {
                        FallingTile nt = new FallingTile(new Point(curX, curPoint.Y), TileType.ROCK);
                        nt.atRest = true;
                        map[curX, curPoint.Y] = nt;

                        curX += xDir;
                    }

                    FallingTile finalt = new FallingTile(new Point(curX, curPoint.Y), TileType.ROCK);
                    finalt.atRest = true;
                    map[curX, curPoint.Y] = finalt;
                }
                else
                {
                    //Move along Y
                    int curY = curPoint.Y;
                    int yDir = (points[j].Y - curPoint.Y) / Math.Abs(points[j].Y - curPoint.Y);

                    while (curY != points[j].Y)
                    {
                        FallingTile nt = new FallingTile(new Point(curPoint.X, curY), TileType.ROCK);
                        nt.atRest = true;
                        map[curPoint.X, curY] = nt;

                        curY += yDir;
                    }

                    FallingTile finalt = new FallingTile(new Point(curPoint.X, curY), TileType.ROCK);
                    finalt.atRest = true;
                    map[curPoint.X, curY] = finalt;
                }

                curPoint = points[j];
            }

            for(int j = 0; j < map.GetLength(0); j++)
            {
                map[j, largestY] = new FallingTile(new Point(j, largestY), TileType.ROCK);
                map[j, largestY].atRest = true;
            }
        }

        bool reachedEnd = false;
        TileResult lastResult = TileResult.LANDED;
        int sandCounter = 0;
        FallingTile curSandTile = null;
        while (!reachedEnd)
        {
            switch (lastResult)
            {
                case (TileResult.LANDED):
                    //Check if end reached
                    if(curSandTile != null && curSandTile.pos == new Point(500 - minX, 0))
                    {
                        reachedEnd = true;
                        break;
                    }

                    //Make a new sand
                    curSandTile = new FallingTile(new Point(500 - minX, 0), TileType.SAND);
                    sandCounter++;

                    lastResult = curSandTile.SandFall(map);
                    break;

                case (TileResult.FALLING):
                    //Continue falling
                    lastResult = curSandTile.SandFall(map);
                    break;

                case (TileResult.ABYSS):
                    //Return!
                    //Actually it should never reach here...
                    Console.WriteLine("UH OH!");
                    break;
            }
        }

        //Print for fun!
        PrintMap(map);

        //Print final output!
        Console.WriteLine($"Sand Count: {sandCounter}");
    }

    public enum TileResult
    {
        FALLING,
        LANDED,
        ABYSS
    }

    public enum TileType
    {
        AIR, //Don't plan on using this honestly
        ROCK,
        SAND,
    }

    public class FallingTile
    {
        public Point pos;
        public TileType type;
        public bool atRest = false;

        public FallingTile(Point startPos, TileType type)
        {
            pos = new Point(startPos.X, startPos.Y);
            this.type = type;
        }

        public TileResult SandFall(FallingTile[,] map)
        {
            int updateX;
            int updateY;

            //Check directly down
            if (map[pos.X, pos.Y + 1] == null)
            {
                updateX = 0;
                updateY = 1;
            }
            else if (pos.X > 0 && map[pos.X - 1, pos.Y + 1] == null)
            {
                //Check left - down
                updateX = -1;
                updateY = 1;
            }
            else if (pos.X < map.GetLength(0) - 1 && map[pos.X + 1, pos.Y + 1] == null)
            {
                //Check right - down
                updateX = 1;
                updateY = 1;
            }
            else
            {
                //Fail!
                atRest = true;
                map[pos.X, pos.Y] = this;

                return TileResult.LANDED;
            }

            //Update position
            pos.X += updateX;
            pos.Y += updateY;

            //Check if tile has fallen into the abyss
            if (pos.Y == map.GetLength(1) - 1)
            {
                return TileResult.ABYSS;
            }


            return TileResult.FALLING;
        }
    }

    public void PrintMap(FallingTile[,] map)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                if (map[i, j] != null)
                {
                    switch (map[i, j].type)
                    {
                        case (TileType.ROCK):
                            Console.Write('█');
                            break;

                        case (TileType.SAND):
                            Console.Write('#');
                            break;

                        default:
                            Console.Write('?');
                            break;
                    }
                }
                else
                {
                    Console.Write(' ');
                }
            }

            Console.WriteLine();
        }
    }
}