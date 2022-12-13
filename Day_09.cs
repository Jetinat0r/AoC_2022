using System.Drawing;
using System.Drawing.Imaging;

public class Day_09
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_09_Input.txt");

        Day_09_01(lines);
    }

    void Day_09_01(string[] input)
    {
        List<Knot> rope = new List<Knot>();
        Knot headKnot = new Knot(new GridPos(0, 0), null);
        rope.Add(headKnot);

        for(int i = 0; i < 9; i++)
        {
            Knot newKnot = new Knot(new GridPos(0, 0), rope[i]);
            rope[i].child = newKnot;
            rope.Add(newKnot);
        }


        List<GridPos> occupiedPoses = new List<GridPos>();
        occupiedPoses.Add(new GridPos(0, 0));


        foreach (string line in input)
        {
            string[] splitInput = line.Split(' ');

            int xDir = 0;
            int yDir = 0;
            //Get Direction head moves
            switch (splitInput[0][0])
            {
                case ('R'):
                    xDir = 1;
                    break;

                case ('L'):
                    xDir = -1;
                    break;

                case ('U'):
                    yDir = 1;
                    break;

                case ('D'):
                    yDir = -1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Invalid Direction!");
            }

            int numTimes = Convert.ToInt32(splitInput[1]);
            for(int i = 0; i < numTimes; i++)
            {
                rope[0].pos.x += xDir;
                rope[0].pos.y += yDir;

                rope[0].child.MoveKnot();


                GridPos newTailPos = new GridPos(rope[^1].pos.x, rope[^1].pos.y);
                if (!occupiedPoses.Contains(newTailPos))
                {
                    occupiedPoses.Add(newTailPos);
                }
                //Console.WriteLine($"HEAD: ({headPos.x}, {headPos.y}); TAIL: ({tailPos.x}, {tailPos.y})");


            }


        }

        Console.WriteLine("Count: " + occupiedPoses.Count);


        /*
        GridPos smallest = new GridPos(0, 0);
        GridPos largest = new GridPos(0, 0);
        for(int i = 1; i < occupiedPoses.Count; i++)
        {
            smallest = GridPos.Min(smallest, occupiedPoses[i]);
            largest = GridPos.Max(largest, occupiedPoses[i]);
        }

        int xDiff = (largest.x - smallest.x) + 1;
        int yDiff = (largest.y - smallest.y) + 1;
        int xOff = Math.Abs(smallest.x);
        int yOff = Math.Abs(smallest.y);

        bool[,] visualizationGrid = new bool[xDiff, yDiff];

        for(int i = 0; i < occupiedPoses.Count; i++)
        {
            visualizationGrid[occupiedPoses[i].x + xOff, occupiedPoses[i].y + yOff] = true;
        }

        Bitmap bmp = new Bitmap(xDiff, yDiff);

        for(int i = 0; i < visualizationGrid.GetLength(0); i++)
        {
            for(int j = 0; j < visualizationGrid.GetLength(1); j++)
            {
                bmp.SetPixel(i, j, visualizationGrid[i, j] ? Color.Black : Color.White);
                //Console.Write(visualizationGrid[i, j] ? "#" : "_");
            }

            //Console.WriteLine();
        }

        bmp.Save("RopeVisualization.png", ImageFormat.Png);
        */
    }

    public class Knot
    {
        public GridPos pos;
        public Knot parent;
        public Knot child;

        public Knot(GridPos initialPos, Knot parent)
        {
            pos = initialPos;
            this.parent = parent;
            child = null;
        }

        public void MoveKnot()
        {
            if(parent == null)
            {
                throw new ArgumentNullException("Parent is null!");
            }

            int xDist = parent.pos.x - pos.x;
            int yDist = parent.pos.y - pos.y;
            double dist = Math.Sqrt((xDist * xDist) + (yDist * yDist));

            if (dist > 2)
            {
                //Move Diagonally
                int moveXDir = (parent.pos.x - pos.x);
                moveXDir = (moveXDir != 0) ? moveXDir / Math.Abs(moveXDir) : 0;
                int moveYDir = (parent.pos.y - pos.y);
                moveYDir = (moveYDir != 0) ? moveYDir / Math.Abs(moveYDir) : 0;

                pos.x += moveXDir;
                pos.y += moveYDir;
            }
            else
            {
                int xMove = 0;
                if (pos.x - parent.pos.x > 1)
                {
                    xMove = -1;
                }
                else if (parent.pos.x - pos.x > 1)
                {
                    xMove = 1;
                }

                int yMove = 0;
                if (pos.y - parent.pos.y > 1)
                {
                    yMove = -1;
                }
                else if (parent.pos.y - pos.y > 1)
                {
                    yMove = 1;
                }

                pos.x += xMove;
                pos.y += yMove;
            }

            if(child != null)
            {
                child.MoveKnot();
            }
        }
    }

    public class GridPos
    {
        public int x;
        public int y;

        public GridPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator==(GridPos obj1, GridPos obj2)
        {
            return obj1.x == obj2.x && obj1.y == obj2.y;
        }

        public static bool operator !=(GridPos obj1, GridPos obj2)
        {
            return obj1.x != obj2.x || obj1.y != obj2.y;
        }

        public static GridPos Min(GridPos obj1, GridPos obj2)
        {
            return new GridPos((obj1.x < obj2.x) ? obj1.x : obj2.x, (obj1.y < obj2.y) ? obj1.y : obj2.y);
        }

        public static GridPos Max(GridPos obj1, GridPos obj2)
        {
            return new GridPos((obj1.x > obj2.x) ? obj1.x : obj2.x, (obj1.y > obj2.y) ? obj1.y : obj2.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if(obj is GridPos pos)
            {
                return pos == this;
            }

            return false;
        }
    }
}