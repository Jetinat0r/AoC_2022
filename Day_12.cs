using Microsoft.VisualBasic;
using System.Drawing;
using System.IO;

public class Day_12
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_12_Input.txt");

        Day_12_02(lines);
    }

    void Day_12_01(string[] input)
    {
        Tile[,] map = new Tile[input[0].Length, input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                map[j, i] = new Tile(input[i][j], j, i);
            }
        }

        //Console.WriteLine(map[1, 6].Height + " " + map[2, 6].X + " " + map[2, 6].Y);

        Point startPos = new Point(-1, -1);
        Point endPos = new Point(-1, -1);

        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                map[j, i].SetupTile(map);

                if (map[j, i].IsStart)
                {
                    startPos = new Point(j, i);
                }
                else if (map[j, i].IsEnd)
                {
                    endPos = new Point(j, i);
                }

                //Console.Write((char)(map[j, i].Height + 'a'));
            }

            //Console.WriteLine();
        }


        PathNode path = AStar(map, startPos, endPos);
        int counter = -1;
           
        while (!(path is null))
        {
            counter++;
            //Console.WriteLine($"({path.position.X}, {path.position.Y})");
            path = path.cameFrom;
        }


        Console.WriteLine($"COUNTER: {counter}");

    }

    void Day_12_02(string[] input)
    {
        Tile[,] map = new Tile[input[0].Length, input.Length];

        for(int i = 0; i < input.Length; i++)
        {
            for(int j = 0; j < input[i].Length; j++)
            {
                map[j, i] = new Tile(input[i][j], j, i);
            }
        }

        //Console.WriteLine(map[1, 6].Height + " " + map[2, 6].X + " " + map[2, 6].Y);

        Point startPos = new Point(-1, -1);
        Point endPos = new Point(-1, -1);

        for (int i = 0; i < map.GetLength(1); i++)
        {
            for(int j = 0; j < map.GetLength(0); j++)
            {
                map[j, i].SetupTile(map);

                if (map[j, i].IsStart)
                {
                    startPos = new Point(j, i);
                }
                else if (map[j, i].IsEnd)
                {
                    endPos = new Point(j, i);
                }

                //Console.Write((char)(map[j, i].Height + 'a'));
            }

            //Console.WriteLine();
        }

        List<Tile> lowestTiles = new List<Tile>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].Height == 0)
                {
                    lowestTiles.Add(map[i, j]);
                }
            }
        }

        List<PathNode> results = new List<PathNode>();
        foreach(Tile tile in lowestTiles)
        {
            results.Add(AStar(map, new Point(tile.X, tile.Y), endPos));
        }

        List<int> counters = new List<int>();
        
        //PathNode path = AStar(map, startPos, endPos);
        for(int i = 0; i < results.Count; i++)
        {
            int counter = -1;
            PathNode node = results[i];
            while (!(node is null))
            {
                counter++;
                //Console.WriteLine($"({path.position.X}, {path.position.Y})");
                node = node.cameFrom;
            }

            if(counter != -1)
            {
                counters.Add(counter);
            }
        }
        

        Console.WriteLine($"COUNTER: {counters.Min()}");
        
    }

    public PathNode AStar(Tile[,] map, Point startPos, Point endPos)
    {
        List<PathNode> OpenNodes = new List<PathNode>();
        List<PathNode> ClosedNodes = new List<PathNode>();

        OpenNodes.Add(new PathNode(new Point(startPos.X, startPos.Y)));
        int distToEnd = HeuristicFunction(startPos, endPos);
        OpenNodes[0].SetScore(0, distToEnd);

        while (OpenNodes.Count > 0)
        {
            //A*
            //G Cost: Dist from start
            //H Cost: Dist from end
            //F Cost: G + H Cost

            //Check F -> H to determine which node gets selected

            //Get Min move
            List<PathNode> MinFs = new List<PathNode>();
            int curMinFScore = 99999999;
            for(int i = 0; i < OpenNodes.Count; i++)
            {
                if (OpenNodes[i].F < curMinFScore)
                {
                    curMinFScore = OpenNodes[i].F;
                    MinFs.Clear();
                    MinFs.Add(OpenNodes[i]);
                }
                else if (OpenNodes[i].F == curMinFScore)
                {
                    MinFs.Add(OpenNodes[i]);
                }
            }

            List<PathNode> MinHs = new List<PathNode>();
            int curMinHScore = 99999999;
            for(int i = 0; i < MinFs.Count; i++)
            {
                if (MinFs[i].H < curMinHScore)
                {
                    curMinHScore = MinFs[i].H;
                    MinHs.Clear();
                    MinHs.Add(MinFs[i]);
                }
                else if (MinFs[i].H == curMinHScore)
                {
                    MinHs.Add(MinFs[i]);
                }
            }

            PathNode curNode = MinHs[0];
            //ClosedNodes.Find(GetNodeByPos());
            //ClosedNodes.Find((node) => node.position.X == pos.)

            //Make a move
            //Get move weights
            List<int> changes = map[curNode.position.X, curNode.position.Y].CheckDirections(map);

            //Up
            if (changes[0] != -100)
            {
                PathNode upOpenNode = OpenNodes.Find(GetNodeByPos(new Point(curNode.position.X, curNode.position.Y - 1)));
                PathNode upClosedNode = ClosedNodes.Find(GetNodeByPos(new Point(curNode.position.X, curNode.position.Y - 1)));
                if (upOpenNode != null)
                {
                    upOpenNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(upOpenNode.position, endPos), curNode);
                }
                else if (upClosedNode != null)
                {
                    upClosedNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(upClosedNode.position, endPos), curNode);
                }
                else
                {
                    PathNode newUpNode = new PathNode(new Point(curNode.position.X, curNode.position.Y - 1));
                    newUpNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(newUpNode.position, endPos), curNode);
                    OpenNodes.Add(newUpNode);
                }
            }

            //Down
            if (changes[1] != -100)
            {
                PathNode downOpenNode = OpenNodes.Find(GetNodeByPos(new Point(curNode.position.X, curNode.position.Y + 1)));
                PathNode downClosedNode = ClosedNodes.Find(GetNodeByPos(new Point(curNode.position.X, curNode.position.Y + 1)));
                if (downOpenNode != null)
                {
                    downOpenNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(downOpenNode.position, endPos), curNode);
                }
                else if (downClosedNode != null)
                {
                    downClosedNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(downClosedNode.position, endPos), curNode);
                }
                else
                {
                    PathNode newDownNode = new PathNode(new Point(curNode.position.X, curNode.position.Y + 1));
                    newDownNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(newDownNode.position, endPos), curNode);
                    OpenNodes.Add(newDownNode);
                }
            }

            //Left
            if (changes[2] != -100)
            {
                PathNode leftOpenNode = OpenNodes.Find(GetNodeByPos(new Point(curNode.position.X - 1, curNode.position.Y)));
                PathNode leftClosedNode = ClosedNodes.Find(GetNodeByPos(new Point(curNode.position.X - 1, curNode.position.Y)));
                if (leftOpenNode != null)
                {
                    leftOpenNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(leftOpenNode.position, endPos), curNode);
                }
                else if (leftClosedNode != null)
                {
                    leftClosedNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(leftClosedNode.position, endPos), curNode);
                }
                else
                {
                    PathNode newLeftNode = new PathNode(new Point(curNode.position.X - 1, curNode.position.Y));
                    newLeftNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(newLeftNode.position, endPos), curNode);
                    OpenNodes.Add(newLeftNode);
                }
            }

            //Right
            if (changes[3] != -100)
            {
                PathNode rightOpenNode = OpenNodes.Find(GetNodeByPos(new Point(curNode.position.X + 1, curNode.position.Y)));
                PathNode rightClosedNode = ClosedNodes.Find(GetNodeByPos(new Point(curNode.position.X + 1, curNode.position.Y)));
                if (rightOpenNode != null)
                {
                    rightOpenNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(rightOpenNode.position, endPos), curNode);
                }
                else if (rightClosedNode != null)
                {
                    rightClosedNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(rightClosedNode.position, endPos), curNode);
                }
                else
                {
                    PathNode newRightNode = new PathNode(new Point(curNode.position.X + 1, curNode.position.Y));
                    newRightNode.SetScore(curNode.G + PathNode.CardinalMove, HeuristicFunction(newRightNode.position, endPos), curNode);
                    OpenNodes.Add(newRightNode);
                }
            }

            OpenNodes.Remove(curNode);
            ClosedNodes.Add(curNode);

            PathNode reachedEndOpen = OpenNodes.Find(GetNodeByPos(endPos));
            if (!(reachedEndOpen is null))
            {
                /*
                Console.WriteLine();
                Console.WriteLine("========================================================================================================================================");
                for (int i = 0; i < map.GetLength(1); i++)
                {
                    for (int j = 0; j < map.GetLength(0); j++)
                    {
                        if (OpenNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                        {
                            Console.Write("O");
                        }
                        else if (ClosedNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                        {
                            Console.Write("X");
                        }
                        else
                        {
                            Console.Write("_");
                        }

                        //Console.Write((char)(map[j, i].Height + 'a'));
                    }

                    Console.WriteLine();
                }
                Console.WriteLine("========================================================================================================================================");
                */


                return reachedEndOpen;
            }

            PathNode reachedEndClosed = ClosedNodes.Find(GetNodeByPos(endPos));
            if (!(reachedEndClosed is null))
            {
                return reachedEndClosed;
            }

            
            /*
            Console.WriteLine();
            Console.WriteLine("========================================================================================================================================");
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if(OpenNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                    {
                        Console.Write("O");
                    }
                    else if (ClosedNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write("_");
                    }

                    //Console.Write((char)(map[j, i].Height + 'a'));
                }

                Console.WriteLine();
            }
            Console.WriteLine("========================================================================================================================================");
            */
        }


        Console.WriteLine();
        Console.WriteLine("========================================================================================================================================");
        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                if (OpenNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                {
                    Console.Write("O");
                }
                else if (ClosedNodes.Find(GetNodeByPos(new Point(j, i))) != null)
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write("_");
                }

                //Console.Write((char)(map[j, i].Height + 'a'));
            }

            Console.WriteLine();
        }
        Console.WriteLine("========================================================================================================================================");

        return null;
    }

    public class Tile
    {
        public static int LOWER = 1;
        public static int SAME = 2;
        public static int GREATER = 3;

        public bool IsStart = false;
        public bool IsEnd = false;

        public int Height;

        public int X;
        public int Y;

        //Where you can go from each tile
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public Tile(char height, int x, int y)
        {
            if (height == 'S')
            {
                //Is Start Tile
                Height = 'a' - 'a';
                IsStart = true;
            }
            else if (height == 'E')
            {
                //Is End Tile
                Height = 'z' - 'a';
                IsEnd = true;
            }
            else
            {
                Height = height - 'a';
            }

            X = x;
            Y = y;
        }

        public void BADSetupTile(Tile[,] map)
        {
            //Check Left
            if(X - 1 < 0)
            {
                Left = false;
            }
            else if (map[X - 1, Y].Height >= Height - 1 && map[X - 1, Y].Height <= Height + 1)
            {
                Left = true;
            }
            else
            {
                Left = false;
            }

            //Check Right
            if (X + 1 >= map.GetLength(0))
            {
                Right = false;
            }
            else if (map[X + 1, Y].Height >= Height - 1 && map[X + 1, Y].Height <= Height + 1)
            {
                Right = true;
            }
            else
            {
                Right = false;
            }


            //Check Up
            if (Y - 1 < 0)
            {
                Up = false;
            }
            else if (map[X, Y - 1].Height >= Height - 1 && map[X, Y - 1].Height <= Height + 1)
            {
                Up = true;
            }
            else
            {
                Up = false;
            }

            //Check Down
            if (Y + 1 >= map.GetLength(1))
            {
                Down = false;
            }
            else if (map[X, Y + 1].Height >= Height - 1 && map[X, Y + 1].Height <= Height + 1)
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
        }

        public void SetupTile(Tile[,] map)
        {
            //Check Left
            if (X - 1 < 0)
            {
                Left = false;
            }
            else if (map[X - 1, Y].Height <= Height + 1)
            {
                Left = true;
            }
            else
            {
                Left = false;
            }

            //Check Right
            if (X + 1 >= map.GetLength(0))
            {
                Right = false;
            }
            else if (map[X + 1, Y].Height <= Height + 1)
            {
                Right = true;
            }
            else
            {
                Right = false;
            }


            //Check Up
            if (Y - 1 < 0)
            {
                Up = false;
            }
            else if (map[X, Y - 1].Height <= Height + 1)
            {
                Up = true;
            }
            else
            {
                Up = false;
            }

            //Check Down
            if (Y + 1 >= map.GetLength(1))
            {
                Down = false;
            }
            else if (map[X, Y + 1].Height <= Height + 1)
            {
                Down = true;
            }
            else
            {
                Down = false;
            }
        }

        public List<int> CheckDirections(Tile[,] map)
        {
            List<int> result = new List<int>();

            //Check Up
            if (Up)
            {
                if (map[X, Y - 1].Height <= Height - 1)
                {
                    result.Add(LOWER + Height);
                }
                else if (map[X, Y - 1].Height == Height)
                {
                    result.Add(SAME + Height);
                }
                else if (map[X, Y - 1].Height == Height + 1)
                {
                    result.Add(GREATER + Height);
                }
            }
            else
            {
                result.Add(-100);
            }

            //Check Down
            if (Down)
            {
                if (map[X, Y + 1].Height <= Height - 1)
                {
                    result.Add(LOWER + Height);
                }
                else if (map[X, Y + 1].Height == Height)
                {
                    result.Add(SAME + Height);
                }
                else if (map[X, Y + 1].Height == Height + 1)
                {
                    result.Add(GREATER + Height);
                }
            }
            else
            {
                result.Add(-100);
            }

            //Check Left
            if (Left)
            {
                if (map[X - 1, Y].Height <= Height - 1)
                {
                    result.Add(LOWER + Height);
                }
                else if (map[X - 1, Y].Height == Height)
                {
                    result.Add(SAME + Height);
                }
                else if (map[X - 1, Y ].Height == Height + 1)
                {
                    result.Add(GREATER + Height);
                }
            }
            else
            {
                result.Add(-100);
            }

            //Check Right
            if (Right)
            {
                if (map[X + 1, Y].Height <= Height - 1)
                {
                    result.Add(LOWER + Height);
                }
                else if (map[X + 1, Y].Height == Height)
                {
                    result.Add(SAME + Height);
                }
                else if (map[X + 1, Y].Height == Height + 1)
                {
                    result.Add(GREATER + Height);
                }
            }
            else
            {
                result.Add(-100);
            }

            return result;
        }
    }

    public class PathNode
    {
        public const int CardinalMove = 10;

        public PathNode cameFrom = null;

        public Point position;

        public int G = -1;
        public int H = -1;
        public int F = -1;

        public bool IsClosed = false;

        public PathNode(Point pos)
        {
            position = pos;
        }

        public void SetScore(int g, int h, PathNode origin = null)
        {
            if(G == -1)
            {
                G = g;
                H = h;
                cameFrom = origin;
            }
            else
            {
                if(g < G)
                {
                    G = g;
                    cameFrom = origin;
                }
            }


            F = G + H;
        }
    }

    public static int HeuristicFunction(Point startPos, Point endPos)
    {
        return (Math.Abs(endPos.X - startPos.X) + Math.Abs(endPos.Y - startPos.Y)) * PathNode.CardinalMove;
    }


    //Gets the Node with the specified point if it can
    public static Predicate<PathNode> GetNodeByPos(Point pos)
    {
        return (node) => (node.position.X == pos.X && node.position.Y == pos.Y);
    }
}