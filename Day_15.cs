using System;
using System.Drawing;
using System.Linq;
using System.Numerics;

public class Day_15
{
    const int PART_ONE_Y = 2000000;
    const int PART_TWO_SIZE = 4000000;
    //const int PART_TWO_SIZE = 20;
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_15_Input.txt");

        //Diamond x = new Diamond(new Point(0, 0), 2, new Point(-2, 0), new Point(2, 0), new Point(0, -2), new Point(0, 2));
        //List<Point> generated = x.GetExtraPoints();

        Day_15_02(lines);
    }

    void Day_15_01(string[] input)
    {
        //int unavailableCounter = 0;
        int? minX = null;
        int? maxX = null;
        List<Point> modules = new();
        for(int i = 0; i < input.Length; i++)
        {
            Console.WriteLine(i + 1);
            string[] splitInput = input[i].Split(' ');

            int sensorX = Convert.ToInt32(splitInput[2].Substring(2, splitInput[2].Length - 3));
            int sensorY = Convert.ToInt32(splitInput[3].Substring(2, splitInput[3].Length - 3));
            Point sensorPoint = new Point(sensorX, sensorY);

            int beaconX = Convert.ToInt32(splitInput[8].Substring(2, splitInput[8].Length - 3));
            int beaconY = Convert.ToInt32(splitInput[9].Substring(2, splitInput[9].Length - 2));
            Point beaconPoint = new Point(beaconX, beaconY);


            int manhattenDistance = Math.Abs(beaconX - sensorX) + Math.Abs(beaconY - sensorY);

            int yDist;
            //Check if any invalid tiles ever cover the y=2000000
            if(sensorY < PART_ONE_Y)
            {
                if(sensorY + manhattenDistance < PART_ONE_Y)
                {
                    //Skip
                    continue;
                }
                else
                {
                    yDist = PART_ONE_Y - sensorY;
                }
            }
            else
            {
                if (sensorY - manhattenDistance > PART_ONE_Y)
                {
                    //Skip
                    continue;
                }
                else
                {
                    yDist = sensorY - PART_ONE_Y;
                }
            }

            //We're in distance, so figure it out
            //Find the min x
            //Find the max x
            int xDist = manhattenDistance - yDist;
            if(minX == null)
            {
                minX = sensorX - xDist;
                maxX = sensorX + xDist;
            }
            else
            {
                if(sensorX - xDist < minX)
                {
                    minX = sensorX - xDist;
                }

                if(sensorX + xDist > maxX)
                {
                    //Plus 1 accounts for 0 or something idk
                    maxX = sensorX + xDist + 1;
                }
            }

            if(sensorY == PART_ONE_Y)
            {
                if(modules.FindIndex(GetNodeByPos(sensorPoint)) == -1)
                {
                    modules.Add(sensorPoint);
                }
            }

            if(beaconY == PART_ONE_Y)
            {
                if (modules.FindIndex(GetNodeByPos(beaconPoint)) == -1)
                {
                    modules.Add(beaconPoint);
                }
                else
                {

                }
            }
            /*
            for(int x = sensorX - xDist; x <= sensorX + xDist; x++)
            {
                int y = PART_ONE_Y;
                MapTile curTile = mapTiles.Find(GetNodeByPos(new Point(x, y)));
                if (curTile == null)
                {
                    if((x == beaconX && y == beaconY) || (x == sensorX && y == sensorY))
                    {
                        mapTiles.Add(new MapTile(new Point(x, y), TileType.BEACON));
                    }
                    else
                    {
                        mapTiles.Add(new MapTile(new Point(x, y), TileType.UNREACHABLE));
                    }
                }
                else if((x == beaconX && y == beaconY) || (x == sensorX && y == sensorY))
                {
                    
                    mapTiles.Remove(curTile);
                    mapTiles.Add(new MapTile(new Point(x, y), TileType.BEACON));
                }
            }
            */
        }

        //int count = mapTiles.FindAll(GetNodeByYPos(PART_ONE_Y)).Count;
        Console.WriteLine("Occupying y=2000000: " + ((maxX - minX) - modules.Count));
    }

    public enum TileType
    {
        SENSOR,
        BEACON,
        UNREACHABLE,
        EMPTY, //Shouldn't be used...
    }

    void Day_15_02(string[] input)
    {
        //PART 2 INTERSECT:
        //X = 3068581
        //Y = 3017867

        List<Diamond> coveredArea = new();
        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            int sensorX = Convert.ToInt32(splitInput[2].Substring(2, splitInput[2].Length - 3));
            int sensorY = Convert.ToInt32(splitInput[3].Substring(2, splitInput[3].Length - 3));
            Point sensorPoint = new Point(sensorX, sensorY);

            int beaconX = Convert.ToInt32(splitInput[8].Substring(2, splitInput[8].Length - 3));
            int beaconY = Convert.ToInt32(splitInput[9].Substring(2, splitInput[9].Length - 2));
            Point beaconPoint = new Point(beaconX, beaconY);


            int manhattenDistance = Math.Abs(beaconX - sensorX) + Math.Abs(beaconY - sensorY);

            Diamond newDiamond = new Diamond(sensorPoint,
                manhattenDistance,
                new Point(sensorX - manhattenDistance, sensorY),
                new Point(sensorX + manhattenDistance, sensorY),
                new Point(sensorX, sensorY - manhattenDistance),
                new Point(sensorX, sensorY + manhattenDistance));
            coveredArea.Add(newDiamond);
        }

        Point emptyPos = new Point(-1, -1);
        for(int i = 0; i < coveredArea.Count; i++)
        {
            Diamond d = coveredArea[i];
            //Console.WriteLine($"{i}: polygon(({d.top.X}, {d.top.Y}), ({d.right.X}, {d.right.Y}), ({d.bottom.X}, {d.bottom.Y}), ({d.left.X}, {d.left.Y}))");
            List<Point> generatedPoints = d.GetExtraPoints();
            

            foreach(Point p in generatedPoints)
            {
                bool allDoNotContain = true;
                for (int j = 0; j < coveredArea.Count; j++)
                {
                    if (coveredArea[j].Contains(p))
                    {
                        allDoNotContain = false;
                        break;
                    }
                }

                if (allDoNotContain)
                {
                    emptyPos = p;
                    break;
                }
            }
            
            if(emptyPos != new Point(-1, -1))
            {
                break;
            }
        }

        Console.WriteLine($"Empty Pos: ({emptyPos.X}, {emptyPos.Y})");
    }

    public class Diamond
    {
        public Point center;
        public int manhattenDistance;

        public Point left;
        public Point right;
        public Point top;
        public Point bottom;

        public Diamond(Point c, int manhattenDistance, Point l, Point r, Point t, Point b)
        {
            center = c;
            this.manhattenDistance = manhattenDistance;

            left = l;

            right = r;

            top = t;

            bottom = b;
        }

        public Point[] GetPoints()
        {
            return new Point[] { left, right, top, bottom, left };
        }

        public List<Point> GetExtraPoints()
        {
            List<Point> points = new List<Point>();

            int x = center.X;
            int y = top.Y - 1;

            //Travel Top -> Right
            while(x < right.X + 1)
            {
                if(x >= 0 && x <= PART_TWO_SIZE && y >= 0 && y <= PART_TWO_SIZE)
                {
                    points.Add(new Point(x, y));
                }

                x += 1;
                y += 1;
            }

            //Travel Right -> Bottom
            while(x > center.X)
            {
                if (x >= 0 && x <= PART_TWO_SIZE && y >= 0 && y <= PART_TWO_SIZE)
                {
                    points.Add(new Point(x, y));
                }

                x -= 1;
                y += 1;
            }

            //Travel Bottom -> Left
            while (x > left.X - 1)
            {
                if(x >= 0 && x <= PART_TWO_SIZE && y >= 0 && y <= PART_TWO_SIZE)
                {
                    points.Add(new Point(x, y));
                }

                x -= 1;
                y -= 1;
            }

            //Travel Left -> Top
            while (x < center.X)
            {
                if (x >= 0 && x <= PART_TWO_SIZE && y >= 0 && y <= PART_TWO_SIZE)
                {
                    points.Add(new Point(x, y));
                }

                x += 1;
                y -= 1;
            }

            return points;
        }

        public bool Contains(Point p)
        {
            return Math.Abs(p.X - center.X) + Math.Abs(p.Y - center.Y) <= manhattenDistance;
        }
    }



    //Gets the Node with the specified point if it can
    public static Predicate<Point> GetNodeByPos(Point pos)
    {
        return (node) => node == pos;
    }

    /*
    //Gets the Node with the specified Y pos if it can
    public static Predicate<MapTile> GetNodeByYPos(int y)
    {
        return (node) => (node.pos.Y == y && node.type != TileType.BEACON);
    }
    */

    
}