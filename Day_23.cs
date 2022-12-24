using System.Drawing;

public class Day_23
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_23_Input.txt");

        Day_23_02(lines);
    }

    void Day_23_01(string[] input)
    {
        List<Elf> elves = new();

        for(int i = 0; i < input.Length; i++)
        {
            for(int j = 0; j < input[i].Length; j++)
            {
                if(input[i][j] == '#')
                {
                    elves.Add(new Elf(new Point(j, i)));
                }
            }
        }


        //Do movement
        for(int i = 0; i < 10; i++)
        {
            bool allSettled = true;
            foreach(Elf elf in elves)
            {
                allSettled = !elf.LookAhead(elves);
            }

            foreach (Elf elf in elves)
            {
                elf.CheckCanMove(elves);
            }

            foreach (Elf elf in elves)
            {
                elf.Move();
            }
        }



        //Find bounding box
        int minX = elves[0].curPoint.X;
        int maxX = elves[0].curPoint.X;
        int minY = elves[0].curPoint.Y;
        int maxY = elves[0].curPoint.Y;

        for(int i = 1; i < elves.Count; i++)
        {
            if (elves[i].curPoint.X < minX)
            {
                minX = elves[i].curPoint.X;
            }
            if (elves[i].curPoint.X > maxX)
            {
                maxX = elves[i].curPoint.X;
            }

            if (elves[i].curPoint.Y < minY)
            {
                minY = elves[i].curPoint.Y;
            }
            if (elves[i].curPoint.Y > maxY)
            {
                maxY = elves[i].curPoint.Y;
            }
        }

        //4025 is TOO HIGH!
        //3486 is TOO LOW!

        int width = (maxX - minX) + 1;
        int height = (maxY - minY) + 1;
        Console.WriteLine($"{minX} {maxX} {minY} {maxY}");
        Console.WriteLine($"W:({width}) H:({height}) T:({(width * height) - elves.Count})");
    }

    void Day_23_02(string[] input)
    {
        List<Elf> elves = new();

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '#')
                {
                    elves.Add(new Elf(new Point(j, i)));
                }
            }
        }


        int round = 1;
        //Do movement
        while(true)
        {
            bool allSettled = true;
            foreach (Elf elf in elves)
            {
                allSettled = !elf.LookAhead(elves) && allSettled;
            }

            if (allSettled)
            {
                break;
            }

            foreach (Elf elf in elves)
            {
                elf.CheckCanMove(elves);
            }

            foreach (Elf elf in elves)
            {
                elf.Move();
            }

            Console.WriteLine(round);
            round++;
        }


        Console.WriteLine($"R:({round})");
    }



    public class Elf
    {
        public Point curPoint;
        public Point desiredPoint = default(Point);
        public bool canMove = false;
        delegate bool MoveProposition(bool N, bool NE, bool E, bool SE, bool S, bool SW, bool W, bool NW);
        List<MoveProposition> moves = new();

        public Elf(Point p)
        {
            curPoint = p;

            moves.Add(CheckNorth);
            moves.Add(CheckSouth);
            moves.Add(CheckWest);
            moves.Add(CheckEast);
        }


        //Returns if the elf wants to move
        public bool LookAhead(List<Elf> elves)
        {
            canMove = false;

            bool N = elves.Find(FindElfPos(new Point(curPoint.X, curPoint.Y - 1))) != null;
            bool NE = elves.Find(FindElfPos(new Point(curPoint.X + 1, curPoint.Y - 1))) != null;
            bool E = elves.Find(FindElfPos(new Point(curPoint.X + 1, curPoint.Y))) != null;
            bool SE = elves.Find(FindElfPos(new Point(curPoint.X + 1, curPoint.Y + 1))) != null;
            bool S = elves.Find(FindElfPos(new Point(curPoint.X, curPoint.Y + 1))) != null;
            bool SW = elves.Find(FindElfPos(new Point(curPoint.X - 1, curPoint.Y + 1))) != null;
            bool W = elves.Find(FindElfPos(new Point(curPoint.X - 1, curPoint.Y))) != null;
            bool NW = elves.Find(FindElfPos(new Point(curPoint.X - 1, curPoint.Y - 1))) != null;

            MoveProposition firstM = moves[0];
            moves.RemoveAt(0);
            moves.Add(firstM);

            //If no movement necessary, don't!
            if (!(N || NE || E || SE || S || SW || W || NW))
            {
                desiredPoint = curPoint;
                return false;
            }

            

            if (firstM.Invoke(N, NE, E, SE, S, SW, W, NW))
            {
                return true;
            }

            for (int i = 0; i < 3; i++)
            {
                if (moves[i].Invoke(N, NE, E, SE, S, SW, W, NW))
                {
                    return true;
                }
            }

            


            desiredPoint = curPoint;
            return true;
        }

        public bool CheckNorth(bool N, bool NE, bool E, bool SE, bool S, bool SW, bool W, bool NW)
        {
            if (!(N || NW || NE))
            {
                desiredPoint = new Point(curPoint.X, curPoint.Y - 1);
                return true;
            }
            return false;
        }

        public bool CheckSouth(bool N, bool NE, bool E, bool SE, bool S, bool SW, bool W, bool NW)
        {
            if (!(S || SW || SE))
            {
                desiredPoint = new Point(curPoint.X, curPoint.Y + 1);
                return true;
            }
            return false;
        }

        public bool CheckWest(bool N, bool NE, bool E, bool SE, bool S, bool SW, bool W, bool NW)
        {
            if (!(W || NW || SW))
            {
                desiredPoint = new Point(curPoint.X - 1, curPoint.Y);
                return true;
            }
            return false;
        }

        public bool CheckEast(bool N, bool NE, bool E, bool SE, bool S, bool SW, bool W, bool NW)
        {
            if (!(E || SE || NE))
            {
                desiredPoint = new Point(curPoint.X + 1, curPoint.Y);
                return true;
            }
            return false;
        }

        public void CheckCanMove(List<Elf> elves)
        {
            if(curPoint == desiredPoint)
            {
                canMove = false;
            }
            else if(elves.FindAll(FindElfDestination(desiredPoint)).Count == 1)
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
        }

        public void Move()
        {
            if (canMove)
            {
                curPoint = desiredPoint;
            }
        }

    }

    public static Predicate<Elf> FindElfPos(Point p)
    {
        return (elf) => elf.curPoint == p;
    }

    public static Predicate<Elf> FindElfDestination(Point p)
    {
        return (elf) => elf.desiredPoint == p;
    }
}