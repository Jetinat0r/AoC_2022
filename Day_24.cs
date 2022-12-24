using System.Drawing;
using static Day_23;

public class Day_24
{
    public const int MIN_X = 1;
    public const int MAX_X = 120;
    public const int MIN_Y = 1;
    public const int MAX_Y = 25;
    public static Point START_POS = new Point(1, 0);
    public static Point END_POS = new Point(120, 26);

    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_24_Input.txt");

        Day_24_02(lines);
    }

    void Day_24_01(string[] input)
    {
        List<Blizzard> blizzards = new List<Blizzard>();
        //Add some constraints
        blizzards.Add(new Blizzard(new Point(START_POS.X - 1, START_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(START_POS.X + 1, START_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(END_POS.X - 1, END_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(END_POS.X + 1, END_POS.Y), 'N'));

        for (int i = 1; i < input.Length - 1; i++)
        {
            for(int j = 1; j < input[i].Length - 1; j++)
            {
                char curChar = input[i][j];
                if (curChar == '>' || curChar == '<' || curChar == '^' || curChar == 'v')
                {
                    blizzards.Add(new Blizzard(new Point(j, i), curChar));
                }
            }
        }

        List<Point> possiblePositions = new();
        possiblePositions.Add(START_POS);

        bool foundEnd = false;
        int counter = 0;
        while (!foundEnd)
        {
            foreach(Blizzard b in blizzards)
            {
                b.Move();
            }

            List<Point> newPoses = new();
            foreach (Point p in possiblePositions)
            {
                newPoses.AddRange(GetPossibleMoves(p, blizzards));
            }

            for(int i = 0; i < newPoses.Count; i++)
            {
                Point p = newPoses[i];
                if(p == END_POS)
                {
                    foundEnd = true;
                }

                //Remove duplicates
                if(newPoses.FindAll(FindPoints(p)).Count > 1)
                {
                    newPoses.RemoveAt(i);
                    i--;
                }
            }

            possiblePositions = newPoses;
            counter++;
            Console.WriteLine(counter);
        }

        Console.WriteLine($"C: {counter}");
    }

    void Day_24_02(string[] input)
    {
        List<Blizzard> blizzards = new List<Blizzard>();
        //Add some constraints
        blizzards.Add(new Blizzard(new Point(START_POS.X - 1, START_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(START_POS.X + 1, START_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(END_POS.X - 1, END_POS.Y), 'N'));
        blizzards.Add(new Blizzard(new Point(END_POS.X + 1, END_POS.Y), 'N'));

        for (int i = 1; i < input.Length - 1; i++)
        {
            for (int j = 1; j < input[i].Length - 1; j++)
            {
                char curChar = input[i][j];
                if (curChar == '>' || curChar == '<' || curChar == '^' || curChar == 'v')
                {
                    blizzards.Add(new Blizzard(new Point(j, i), curChar));
                }
            }
        }

        for(int i = 0; i < 589; i++)
        {
            foreach (Blizzard b in blizzards)
            {
                b.Move();
            }
        }

        List<Point> possiblePositions = new();
        possiblePositions.Add(START_POS);

        bool foundEnd = false;
        int counter = 305;
        counter = 589;
        while (!foundEnd)
        {
            foreach (Blizzard b in blizzards)
            {
                b.Move();
            }

            List<Point> newPoses = new();
            foreach (Point p in possiblePositions)
            {
                newPoses.AddRange(GetPossibleMoves(p, blizzards));
            }

            for (int i = 0; i < newPoses.Count; i++)
            {
                Point p = newPoses[i];
                if (p == END_POS)
                {
                    foundEnd = true;
                }

                //Remove duplicates
                if (newPoses.FindAll(FindPoints(p)).Count > 1)
                {
                    newPoses.RemoveAt(i);
                    i--;
                }
            }

            possiblePositions = newPoses;
            counter++;
            Console.WriteLine(counter);
        }

        Console.WriteLine($"C: {counter}");
    }

    public List<Point> GetPossibleMoves(Point curPoint, List<Blizzard> blizzards)
    {
        List<Point> moves = new();

        if(new Point(curPoint.X, curPoint.Y + 1) == END_POS)
        {
            moves.Add(END_POS);
            return moves;
        }


        if(blizzards.Find(FindBlizzardPos(curPoint)) == null)
        {
            moves.Add(curPoint);
        }

        if(curPoint.X > MIN_X)
        {
            Point l = new Point(curPoint.X - 1, curPoint.Y);
            if (blizzards.Find(FindBlizzardPos(l)) == null)
            {
                moves.Add(l);
            }
        }


        if (curPoint.X < MAX_X)
        {
            Point r = new Point(curPoint.X + 1, curPoint.Y);
            if (blizzards.Find(FindBlizzardPos(r)) == null)
            {
                moves.Add(r);
            }
        }


        if (curPoint.Y > MIN_Y)
        {
            Point u = new Point(curPoint.X, curPoint.Y - 1);
            if (blizzards.Find(FindBlizzardPos(u)) == null)
            {
                moves.Add(u);
            }
        }


        if (curPoint.Y < MAX_Y)
        {
            Point d = new Point(curPoint.X, curPoint.Y + 1);
            if (blizzards.Find(FindBlizzardPos(d)) == null)
            {
                moves.Add(d);
            }
        }


        return moves;
    }

    public List<Point> GetPossibleMovesBack(Point curPoint, List<Blizzard> blizzards)
    {
        List<Point> moves = new();

        if (new Point(curPoint.X, curPoint.Y - 1) == START_POS)
        {
            moves.Add(START_POS);
            return moves;
        }


        if (blizzards.Find(FindBlizzardPos(curPoint)) == null)
        {
            moves.Add(curPoint);
        }

        if (curPoint.X > MIN_X)
        {
            Point l = new Point(curPoint.X - 1, curPoint.Y);
            if (blizzards.Find(FindBlizzardPos(l)) == null)
            {
                moves.Add(l);
            }
        }


        if (curPoint.X < MAX_X)
        {
            Point r = new Point(curPoint.X + 1, curPoint.Y);
            if (blizzards.Find(FindBlizzardPos(r)) == null)
            {
                moves.Add(r);
            }
        }


        if (curPoint.Y > MIN_Y)
        {
            Point u = new Point(curPoint.X, curPoint.Y - 1);
            if (blizzards.Find(FindBlizzardPos(u)) == null)
            {
                moves.Add(u);
            }
        }


        if (curPoint.Y < MAX_Y)
        {
            Point d = new Point(curPoint.X, curPoint.Y + 1);
            if (blizzards.Find(FindBlizzardPos(d)) == null)
            {
                moves.Add(d);
            }
        }


        return moves;
    }

    public class Blizzard
    {
        public Point pos;

        public int xDir = 0;
        public int yDir = 0;


        public Blizzard(Point p, char dir)
        {
            pos = p;

            switch (dir)
            {
                case '>':
                    xDir = 1;
                    break;

                case '<':
                    xDir = -1;
                    break;

                case '^':
                    yDir = -1;
                    break;

                case 'v':
                    yDir = 1;
                    break;

                case 'N':

                    break;

                default:
                    throw new Exception("Bad Dir!");
            }
        }

        public void Move()
        {
            if(xDir == 0 && yDir == 0)
            {
                return;
            }

            int newX = pos.X + xDir;
            int newY = pos.Y + yDir;

            if(newX > MAX_X)
            {
                newX = MIN_X;
            }
            else if(newX < MIN_X)
            {
                newX = MAX_X;
            }

            if(newY > MAX_Y)
            {
                newY = MIN_Y;
            }
            else if(newY < MIN_Y)
            {
                newY = MAX_Y;
            }

            pos = new Point(newX, newY);
        }
    }

    public static Predicate<Blizzard> FindBlizzardPos(Point p)
    {
        return (blizzard) => blizzard.pos == p;
    }

    public static Predicate<Point> FindPoints(Point p)
    {
        return (point) => point == p;
    }
}