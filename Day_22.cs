using System.Drawing;
using static Day_22;

public class Day_22
{
    public const int PART_ONE_MAP_HEIGHT = 200;
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_22_Input.txt");

        Day_22_02(lines);
    }

    void Day_22_01(string[] input)
    {
        Dictionary<Point, CubeTile> map = new();

        for(int i = 0; i < PART_ONE_MAP_HEIGHT; i++)
        {
            for(int j = 0; j < input[i].Length; j++)
            {
                Point curPoint = new Point(j, i);
                if (input[i][j] == '.')
                {
                    CubeTile emptyTile = new CubeTile(curPoint, false);
                    map.Add(curPoint, emptyTile);
                }
                else if (input[i][j] == '#')
                {
                    CubeTile fullTile = new CubeTile(curPoint, true);
                    map.Add(curPoint, fullTile);
                }
            }
        }
        
        
        
        Point myPoint = new Point(50, 0);
        Facing facingDir = Facing.R;


        Queue<string> moves = new();
        string storedMove = "";
        bool OnNum = true;

        for(int i = 0; i < input[^1].Length; i++)
        {
            char nextInput = input[^1][i];
            if(OnNum && (nextInput == 'R' || nextInput == 'D' || nextInput == 'L' || nextInput == 'U'))
            {
                OnNum = false;
                moves.Enqueue(storedMove);

                storedMove = "";

                storedMove += nextInput;
            }
            else if(!OnNum && (nextInput != 'R' && nextInput != 'D' && nextInput != 'L' && nextInput != 'U'))
            {
                OnNum = true;
                moves.Enqueue(storedMove);

                storedMove = "";

                storedMove += nextInput;
            }
            else
            {
                storedMove += nextInput;
            }
        }
        moves.Enqueue(storedMove);

        //TESTING MOVES
        /*
        for (int i = 0; i < 44; i++)
        {
            Point destinationPoint = MoveOne(myPoint, facingDir, map);
            if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
            {
                break;
            }

            myPoint = destinationPoint;
        }
        facingDir = facingDir.Turn("L");
        for (int i = 0; i < 38; i++)
        {
            Point destinationPoint = MoveOne(myPoint, facingDir, map);
            if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
            {
                break;
            }

            myPoint = destinationPoint;
        }
        facingDir = facingDir.Turn("R");
        for (int i = 0; i < 40; i++)
        {
            Point destinationPoint = MoveOne(myPoint, facingDir, map);
            if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
            {
                break;
            }

            myPoint = destinationPoint;
        }
        facingDir = facingDir.Turn("R");
        Point dP = MoveOne(myPoint, facingDir, map);
        myPoint = dP;
        facingDir = facingDir.Turn("R");
        for (int i = 0; i < 3; i++)
        {
            Point destinationPoint = MoveOne(myPoint, facingDir, map);
            if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
            {
                break;
            }

            myPoint = destinationPoint;
        }
        facingDir = facingDir.Turn("L");
        for (int i = 0; i < 9; i++)
        {
            Point destinationPoint = MoveOne(myPoint, facingDir, map);
            if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
            {
                break;
            }

            myPoint = destinationPoint;
        }
        facingDir = facingDir.Turn("R");
        Point dP2 = MoveOne(myPoint, facingDir, map);
        myPoint = dP2;
        facingDir = facingDir.Turn("L");
        Point dP3 = MoveOne(myPoint, facingDir, map);
        myPoint = dP3;

        //Try printing

        for (int j = 0; j < PART_ONE_MAP_HEIGHT; j++)
        {
            for(int i = 0; i < PART_ONE_MAP_HEIGHT; i++)
            {
                if(myPoint == new Point(i, j))
                {
                    Console.Write(">");
                }
                else if(map.TryGetValue(new Point(i, j), out CubeTile v))
                {
                    Console.Write(v.IsWall ? "#" : ".");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }
        */
        


        while(moves.Count > 0)
        {
            string curMove = moves.Dequeue();

            if(curMove == "L" || curMove == "R")
            {
                facingDir = facingDir.Turn(curMove);
            }
            else
            {
                int moveAmount = Convert.ToInt32(curMove);

                for(int i = 0; i < moveAmount; i++)
                {
                    Point destinationPoint = MoveOne(myPoint, facingDir, map);
                    
                    if(destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
                    {
                        break;
                    }
                    

                    myPoint = destinationPoint;
                }
            }



            /*
            for (int j = 0; j < PART_ONE_MAP_HEIGHT; j++)
            {
                for (int i = 0; i < PART_ONE_MAP_HEIGHT; i++)
                {
                    if (myPoint == new Point(i, j))
                    {
                        if(facingDir == Facing.R)
                        {
                            Console.Write(">");
                        }
                        else if(facingDir == Facing.D)
                        {
                            Console.Write("v");
                        }
                        else if(facingDir == Facing.L)
                        {
                            Console.Write("<");
                        }
                        else
                        {
                            Console.Write("^");
                        }
                        
                    }
                    else if (map.TryGetValue(new Point(i, j), out CubeTile v))
                    {
                        Console.Write(v.IsWall ? "#" : ".");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
            */
        }


        Console.WriteLine($"{(1000 * (myPoint.Y + 1)) + (4 * (myPoint.X + 1)) + facingDir}");
    }

    void Day_22_02(string[] input)
    {
        Dictionary<Point, CubeTile> map = new();

        for (int i = 0; i < PART_ONE_MAP_HEIGHT; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                Point curPoint = new Point(j, i);
                if (input[i][j] == '.')
                {
                    CubeTile emptyTile = new CubeTile(curPoint, false);
                    map.Add(curPoint, emptyTile);
                }
                else if (input[i][j] == '#')
                {
                    CubeTile fullTile = new CubeTile(curPoint, true);
                    map.Add(curPoint, fullTile);
                }
            }
        }

        //Setup wrappings
        for (int i = 50; i < 100; i++)
        {
            //2 -> 6
            map[new Point(i, 0)].wrappingPoints.Add(Facing.U, new(Facing.R, new Point(0, 150 + (i - 50))));
        }

        for(int i = 100; i < 150; i++)
        {
            //1 -> 6
            map[new Point(i, 0)].wrappingPoints.Add(Facing.U, new(Facing.U, new Point((i - 100), 199)));
        }

        for(int i = 0; i < 50; i++)
        {
            //1 -> 4
            map[new Point(149, i)].wrappingPoints.Add(Facing.R, new(Facing.L, new Point(99, 149 - i)));
        }

        for(int i = 100; i < 150; i++)
        {
            //1 -> 3
            map[new Point(i, 49)].wrappingPoints.Add(Facing.D, new(Facing.L, new Point(99, 50 + (i - 100))));
        }

        for(int i = 50; i < 100; i++)
        {
            //3 -> 1
            map[new Point(99, i)].wrappingPoints.Add(Facing.R, new(Facing.U, new Point(100 + (i - 50), 49)));
        }

        for (int i = 100; i < 150; i++)
        {
            //4 -> 1
            map[new Point(99, i)].wrappingPoints.Add(Facing.R, new(Facing.L, new Point(149, 49 - (i - 100))));
        }

        for (int i = 50; i < 100; i++)
        {
            //4 -> 6
            map[new Point(i, 149)].wrappingPoints.Add(Facing.D, new(Facing.L, new Point(49, 150 + (i - 50))));
        }

        for (int i = 150; i < 200; i++)
        {
            //6 -> 4
            map[new Point(49, i)].wrappingPoints.Add(Facing.R, new(Facing.U, new Point(50 + (i - 150), 149)));
        }

        for (int i = 0; i < 50; i++)
        {
            //6 -> 1
            map[new Point(i, 199)].wrappingPoints.Add(Facing.D, new(Facing.D, new Point(100 + i, 0)));
        }

        for (int i = 150; i < 200; i++)
        {
            //6 -> 2
            map[new Point(0, i)].wrappingPoints.Add(Facing.L, new(Facing.D, new Point(50 + (i - 150), 0)));
        }

        for (int i = 100; i < 150; i++)
        {
            //5 -> 2
            map[new Point(0, i)].wrappingPoints.Add(Facing.L, new(Facing.R, new Point(50, 49 - (i - 100))));
        }

        for (int i = 0; i < 50; i++)
        {
            //5 -> 3
            map[new Point(i, 100)].wrappingPoints.Add(Facing.U, new(Facing.R, new Point(50, 50 + i)));
        }

        for (int i = 50; i < 100; i++)
        {
            //3 -> 5
            map[new Point(50, i)].wrappingPoints.Add(Facing.L, new(Facing.D, new Point((i - 50), 100)));
        }

        for (int i = 0; i < 50; i++)
        {
            //2 -> 5
            map[new Point(50, i)].wrappingPoints.Add(Facing.L, new(Facing.R, new Point(0, 149 - i)));
        }


        Point myPoint = new Point(50, 0);
        Facing facingDir = Facing.R;


        Queue<string> moves = new();
        string storedMove = "";
        bool OnNum = true;

        for (int i = 0; i < input[^1].Length; i++)
        {
            char nextInput = input[^1][i];
            if (OnNum && (nextInput == 'R' || nextInput == 'D' || nextInput == 'L' || nextInput == 'U'))
            {
                OnNum = false;
                moves.Enqueue(storedMove);

                storedMove = "";

                storedMove += nextInput;
            }
            else if (!OnNum && (nextInput != 'R' && nextInput != 'D' && nextInput != 'L' && nextInput != 'U'))
            {
                OnNum = true;
                moves.Enqueue(storedMove);

                storedMove = "";

                storedMove += nextInput;
            }
            else
            {
                storedMove += nextInput;
            }
        }
        moves.Enqueue(storedMove);

        while (moves.Count > 0)
        {
            /*
            for (int j = 0; j < PART_ONE_MAP_HEIGHT; j++)
            {
                for (int i = 0; i < PART_ONE_MAP_HEIGHT; i++)
                {
                    if (myPoint == new Point(i, j))
                    {
                        if (facingDir == Facing.R)
                        {
                            Console.Write(">");
                        }
                        else if (facingDir == Facing.D)
                        {
                            Console.Write("v");
                        }
                        else if (facingDir == Facing.L)
                        {
                            Console.Write("<");
                        }
                        else
                        {
                            Console.Write("^");
                        }

                    }
                    else if (map.TryGetValue(new Point(i, j), out CubeTile v))
                    {
                        Console.Write(v.IsWall ? "#" : ".");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
            */

            string curMove = moves.Dequeue();

            if (curMove == "L" || curMove == "R")
            {
                facingDir = facingDir.Turn(curMove);
            }
            else
            {
                int moveAmount = Convert.ToInt32(curMove);

                for (int i = 0; i < moveAmount; i++)
                {
                    Point destinationPoint = MoveTwo(myPoint, ref facingDir, map);

                    if (destinationPoint.X == myPoint.X && destinationPoint.Y == myPoint.Y)
                    {
                        break;
                    }


                    myPoint = destinationPoint;
                }
            }
        }


        Console.WriteLine($"{(1000 * (myPoint.Y + 1)) + (4 * (myPoint.X + 1)) + facingDir}");
    }

    public static Point MoveOne(Point myPoint, Facing curDir, Dictionary<Point, CubeTile> map)
    {
        switch (curDir)
        {
            case(Facing.R):
                Point rPoint = new Point(myPoint.X + 1, myPoint.Y);

                //Try simple moving
                if(map.TryGetValue(rPoint, out CubeTile rdestTile))
                {
                    if (rdestTile.IsWall)
                    {
                        return myPoint;
                    }

                    return rdestTile.p;
                }

                //Try wrapping
                List<CubeTile> ryTiles = map.Values.ToList().FindAll((tile) => tile.p.Y == myPoint.Y);
                Point minXTile = ryTiles[0].p;
                for(int i = 1; i < ryTiles.Count; i++)
                {
                    if(ryTiles[i].p.X < minXTile.X)
                    {
                        minXTile = ryTiles[i].p;
                    }
                }

                if (map[minXTile].IsWall)
                {
                    return myPoint;
                }

                return minXTile;



            case (Facing.D):
                Point dPoint = new Point(myPoint.X, myPoint.Y + 1);

                //Try simple moving
                if (map.TryGetValue(dPoint, out CubeTile ddestTile))
                {
                    if (ddestTile.IsWall)
                    {
                        return myPoint;
                    }

                    return ddestTile.p;
                }

                //Try wrapping
                List<CubeTile> dxTiles = map.Values.ToList().FindAll((tile) => tile.p.X == myPoint.X);
                Point minYTile = dxTiles[0].p;
                for (int i = 1; i < dxTiles.Count; i++)
                {
                    if (dxTiles[i].p.Y < minYTile.Y)
                    {
                        minYTile = dxTiles[i].p;
                    }
                }

                if (map[minYTile].IsWall)
                {
                    return myPoint;
                }

                return minYTile;




            case (Facing.L):
                Point lPoint = new Point(myPoint.X - 1, myPoint.Y);

                //Try simple moving
                if (map.TryGetValue(lPoint, out CubeTile ldestTile))
                {
                    if (ldestTile.IsWall)
                    {
                        return myPoint;
                    }

                    return ldestTile.p;
                }

                //Try wrapping
                List<CubeTile> lyTiles = map.Values.ToList().FindAll((tile) => tile.p.Y == myPoint.Y);
                Point maxXTile = lyTiles[0].p;
                for (int i = 1; i < lyTiles.Count; i++)
                {
                    if (lyTiles[i].p.X > maxXTile.X)
                    {
                        maxXTile = lyTiles[i].p;
                    }
                }

                if (map[maxXTile].IsWall)
                {
                    return myPoint;
                }

                return maxXTile;



            case (Facing.U):
                Point uPoint = new Point(myPoint.X, myPoint.Y - 1);

                //Try simple moving
                if (map.TryGetValue(uPoint, out CubeTile udestTile))
                {
                    if (udestTile.IsWall)
                    {
                        return myPoint;
                    }

                    return udestTile.p;
                }

                //Try wrapping
                List<CubeTile> uxTiles = map.Values.ToList().FindAll((tile) => tile.p.X == myPoint.X);
                Point maxYTile = uxTiles[0].p;
                for (int i = 1; i < uxTiles.Count; i++)
                {
                    if (uxTiles[i].p.Y > maxYTile.Y)
                    {
                        maxYTile = uxTiles[i].p;
                    }
                }

                if (map[maxYTile].IsWall)
                {
                    return myPoint;
                }

                return maxYTile;





            default:
                throw new Exception("Bad dir");
        }
    }


    public static Point MoveTwo(Point myPoint, ref Facing curDir, Dictionary<Point, CubeTile> map)
    {
        return map[myPoint].GetWrappingPoint(ref curDir, map);
    }



    public enum Facing
    {
        R = 0,
        D = 1,
        L = 2,
        U = 3
    }

    public class CubeTile
    {
        public Point p;

        public bool IsWall;

        public Dictionary<Facing, Tuple<Facing, Point>> wrappingPoints = new();

        public CubeTile(Point point, bool isWall)
        {
            p = point;
            IsWall = isWall;
        }

        public Point GetWrappingPoint(ref Facing dir, Dictionary<Point, CubeTile> map)
        {
            if (wrappingPoints.TryGetValue(dir, out Tuple<Facing, Point> v))
            {
                if (map[v.Item2].IsWall)
                {
                    return p;
                }

                dir = v.Item1;
                return v.Item2;
            }

            switch (dir)
            {
                case (Facing.R):
                    Point r = new Point(p.X + 1, p.Y);
                    if (map[r].IsWall)
                    {
                        return p;
                    }
                    return r;

                case (Facing.D):
                    Point d = new Point(p.X, p.Y + 1);
                    if (map[d].IsWall)
                    {
                        return p;
                    }
                    return d;

                case (Facing.L):
                    Point l = new Point(p.X - 1, p.Y);
                    if (map[l].IsWall)
                    {
                        return p;
                    }
                    return l;

                case (Facing.U):
                    Point u = new Point(p.X, p.Y - 1);
                    if (map[u].IsWall)
                    {
                        return p;
                    }
                    return u;

                default:
                    throw new Exception("Invalid Dir!");
            }
        }
    }
}


public static class FacingExtensions
{
    public static Facing Turn(this Facing dir, string turn)
    {
        if(turn == "L")
        {
            switch (dir)
            {
                case Facing.R: return Facing.U;
                case Facing.U: return Facing.L;
                case Facing.L: return Facing.D;
                case Facing.D: return Facing.R;
                default: throw new Exception("Bad Dir!");
            }
        }
        else if(turn == "R")
        {
            switch (dir)
            {
                case Facing.R: return Facing.D;
                case Facing.D: return Facing.L;
                case Facing.L: return Facing.U;
                case Facing.U: return Facing.R;
                default: throw new Exception("Bad Dir!");
            }
        }
        else
        {
            throw new Exception("BAD TURN DIR");
        }
    }
}