using System.Drawing;

public class Day_18
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_18_Input.txt");

        Day_18_02(lines);
    }

    void Day_18_01(string[] input)
    {
        List<LavaDrop> drops = new();
        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(',');

            int x = Convert.ToInt32(splitInput[0]);
            int y = Convert.ToInt32(splitInput[1]);
            int z = Convert.ToInt32(splitInput[2]);

            LavaDrop newDrop = new LavaDrop(x, y, z);
            drops.Add(newDrop);
        }

        int openCounter = 0;

        for(int i = 0; i < drops.Count; i++)
        {
            int openSides = 0;

            LavaDrop curDrop = drops[i];
            if(drops.Find(GetDropByPos(new LavaDrop(curDrop.X - 1, curDrop.Y, curDrop.Z))) == null)
            {
                openCounter++;
            }
            if (drops.Find(GetDropByPos(new LavaDrop(curDrop.X + 1, curDrop.Y, curDrop.Z))) == null)
            {
                openCounter++;
            }
            if (drops.Find(GetDropByPos(new LavaDrop(curDrop.X, curDrop.Y - 1, curDrop.Z))) == null)
            {
                openCounter++;
            }
            if (drops.Find(GetDropByPos(new LavaDrop(curDrop.X, curDrop.Y + 1, curDrop.Z))) == null)
            {
                openCounter++;
            }
            if (drops.Find(GetDropByPos(new LavaDrop(curDrop.X, curDrop.Y, curDrop.Z - 1))) == null)
            {
                openCounter++;
            }
            if (drops.Find(GetDropByPos(new LavaDrop(curDrop.X, curDrop.Y, curDrop.Z + 1))) == null)
            {
                openCounter++;
            }

            openCounter += openSides;
        }

        Console.WriteLine(openCounter);
    }

    void Day_18_02(string[] input)
    {
        int minX = 16; //16
        int minY = 11; //11
        int minZ = 6; //6
        int maxX = minX;
        int maxY = minY;
        int maxZ = minZ;

        List<LavaDrop> drops = new();
        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(',');

            int x = Convert.ToInt32(splitInput[0]);
            int y = Convert.ToInt32(splitInput[1]);
            int z = Convert.ToInt32(splitInput[2]);

            if(x < minX)
            {
                minX = x;
            }
            if(x > maxX)
            {
                maxX = x;
            }

            if (y < minY)
            {
                minY = y;
            }
            if (y > maxY)
            {
                maxY = y;
            }

            if (z < minZ)
            {
                minZ = z;
            }
            if (z > maxZ)
            {
                maxZ = z;
            }

            LavaDrop newDrop = new LavaDrop(x, y, z);
            drops.Add(newDrop);
        }

        int openCounter = 0;
        List<LavaDrop> knownOpen = new();
        for (int i = 0; i < drops.Count; i++)
        {
            int openSides = 0;

            LavaDrop curDrop = drops[i];

            LavaDrop lDrop = new LavaDrop(curDrop.X - 1, curDrop.Y, curDrop.Z);
            if (drops.Find(GetDropByPos(lDrop)) == null)
            {
                if(TryFindExit(lDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            LavaDrop rDrop = new LavaDrop(curDrop.X + 1, curDrop.Y, curDrop.Z);
            if (drops.Find(GetDropByPos(rDrop)) == null)
            {
                if (TryFindExit(rDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            LavaDrop dDrop = new LavaDrop(curDrop.X, curDrop.Y - 1, curDrop.Z);
            if (drops.Find(GetDropByPos(dDrop)) == null)
            {
                if (TryFindExit(dDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            LavaDrop uDrop = new LavaDrop(curDrop.X, curDrop.Y + 1, curDrop.Z);
            if (drops.Find(GetDropByPos(uDrop)) == null)
            {
                if (TryFindExit(uDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            LavaDrop bDrop = new LavaDrop(curDrop.X, curDrop.Y, curDrop.Z - 1);
            if (drops.Find(GetDropByPos(bDrop)) == null)
            {
                if (TryFindExit(bDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            LavaDrop fDrop = new LavaDrop(curDrop.X, curDrop.Y, curDrop.Z + 1);
            if (drops.Find(GetDropByPos(fDrop)) == null)
            {
                if (TryFindExit(fDrop, knownOpen, new List<LavaDrop>(), drops, minX, maxX, minY, maxY, minZ, maxZ))
                {
                    openCounter++;
                }
            }

            openCounter += openSides;
        }

        Console.WriteLine(openCounter);
    }

    public class LavaDrop
    {
        public int X;
        public int Y;
        public int Z;

        public LavaDrop(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    //Gets the LavaDrop with the specified point if it can
    public static Predicate<LavaDrop> GetDropByPos(LavaDrop pos)
    {
        return (drop) => drop.X == pos.X && drop.Y == pos.Y && drop.Z == pos.Z;
    }

    public bool TryFindExit(LavaDrop emptyTile, List<LavaDrop> knownOpen, List<LavaDrop> visited, List<LavaDrop> placedTiles, int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
    {
        if(knownOpen.Find(GetDropByPos(emptyTile)) != null)
        {
            return true;
        }

        if(emptyTile.X <= minX || emptyTile.X >= maxX || emptyTile.Y <= minY || emptyTile.Y >= maxY || emptyTile.Z <= minZ || emptyTile.Z >= maxZ)
        {
            knownOpen.Add(emptyTile);
            return true;
        }

        visited.Add(emptyTile);


        LavaDrop lDrop = new LavaDrop(emptyTile.X - 1, emptyTile.Y, emptyTile.Z);
        if(knownOpen.Find(GetDropByPos(lDrop)) != null)
        {
            return true;
        }
        if(visited.Find(GetDropByPos(lDrop)) == null && placedTiles.Find(GetDropByPos(lDrop)) == null)
        {
            //Search!
            if(TryFindExit(lDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }


        LavaDrop rDrop = new LavaDrop(emptyTile.X + 1, emptyTile.Y, emptyTile.Z);
        if (knownOpen.Find(GetDropByPos(rDrop)) != null)
        {
            return true;
        }
        if (visited.Find(GetDropByPos(rDrop)) == null && placedTiles.Find(GetDropByPos(rDrop)) == null)
        {
            //Search!
            if (TryFindExit(rDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }

        LavaDrop dDrop = new LavaDrop(emptyTile.X, emptyTile.Y - 1, emptyTile.Z);
        if (knownOpen.Find(GetDropByPos(dDrop)) != null)
        {
            return true;
        }
        if (visited.Find(GetDropByPos(dDrop)) == null && placedTiles.Find(GetDropByPos(dDrop)) == null)
        {
            //Search!
            if (TryFindExit(dDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }

        LavaDrop uDrop = new LavaDrop(emptyTile.X, emptyTile.Y + 1, emptyTile.Z);
        if (knownOpen.Find(GetDropByPos(uDrop)) != null)
        {
            return true;
        }
        if (visited.Find(GetDropByPos(uDrop)) == null && placedTiles.Find(GetDropByPos(uDrop)) == null)
        {
            //Search!
            if (TryFindExit(uDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }

        LavaDrop bDrop = new LavaDrop(emptyTile.X, emptyTile.Y, emptyTile.Z - 1);
        if (knownOpen.Find(GetDropByPos(bDrop)) != null)
        {
            return true;
        }
        if (visited.Find(GetDropByPos(bDrop)) == null && placedTiles.Find(GetDropByPos(bDrop)) == null)
        {
            //Search!
            if (TryFindExit(bDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }

        LavaDrop fDrop = new LavaDrop(emptyTile.X, emptyTile.Y, emptyTile.Z + 1);
        if (knownOpen.Find(GetDropByPos(fDrop)) != null)
        {
            return true;
        }
        if (visited.Find(GetDropByPos(fDrop)) == null && placedTiles.Find(GetDropByPos(fDrop)) == null)
        {
            //Search!
            if (TryFindExit(fDrop, knownOpen, visited, placedTiles, minX, maxX, minY, maxY, minZ, maxZ))
            {
                knownOpen.Add(emptyTile);
                return true;
            }
        }



        return false;
    }
}