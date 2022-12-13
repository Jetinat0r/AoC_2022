public class Day_07
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_07_Input.txt");


        Day_07_02(lines);
    }

    public void Day_07_01(string[] input)
    {
        FakeFileSystem fs = new FakeFileSystem();
        Stack<string> curDirectory = new Stack<string>();
        curDirectory.Push("~");

        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            //Input is a command
            if (splitInput[0] == "$")
            {
                //Command is cd
                if (splitInput[1] == "cd")
                {
                    if (splitInput[2] == "/")
                    {
                        curDirectory.Clear();
                        curDirectory.Push("~");
                        continue;
                    }

                    if (splitInput[2] == "..")
                    {
                        curDirectory.Pop();
                        continue;
                    }

                    curDirectory.Push(splitInput[2]);
                    fs.LocateDir(ConvertStackToPath(curDirectory));
                }
                else if (splitInput[1] == "ls")
                {
                    int validLineIndex = i + 1;
                    while(validLineIndex < input.Length)
                    {
                        string[] fileData = input[validLineIndex].Split(' ');

                        //Found a command, quit
                        if (fileData[0] == "$")
                        {
                            i = validLineIndex - 1;
                            break;
                        }


                        if (fileData[0] == "dir")
                        {
                            fs.AddDir(ConvertStackToPath(curDirectory), fileData[1]);
                        }
                        else
                        {
                            FakeFile newFile = new FakeFile(ConvertStackToPath(curDirectory), fileData[1]);
                            newFile.size = Convert.ToInt32(fileData[0]);

                            fs.AddFile(ConvertStackToPath(curDirectory), newFile);
                        }

                        validLineIndex++;
                        i = validLineIndex;
                    }
                }
            }
            else
            {
                //Input is a file
                Console.WriteLine("UH OH!");
            }
        }

        fs.UpdateSize(fs.homeDir);

        List<FakeDirectory> removableDirs = new List<FakeDirectory>();
        foreach(FakeElement e in fs.homeDir.subElements)
        {
            if (e is FakeDirectory)
            {
                fs.GrabSubDirs(ref removableDirs, (FakeDirectory)e);
            }
        }

        List<int> largestDirs = new List<int>();
        for(int i = 0; i < removableDirs.Count; i++)
        {
            if (removableDirs[i].size > 100000)
            {
                removableDirs.RemoveAt(i);
                i--;
                continue;
            }

            largestDirs.Add(removableDirs[i].size);
        }

        largestDirs.Sort();
        largestDirs.Reverse();
        for(int i = 0; i < largestDirs.Count; i++)
        {
            Console.WriteLine((i + 1) + ": " + largestDirs[i]);
        }

        int sum = largestDirs.Sum();
        /*
        for(int i = 0; i < 3; i++)
        {
            sum += largestDirs[i];
        }
        */

        Console.WriteLine("SUM: " + sum);
    }

    public void Day_07_02(string[] input)
    {
        FakeFileSystem fs = new FakeFileSystem();
        Stack<string> curDirectory = new Stack<string>();
        curDirectory.Push("~");

        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            //Input is a command
            if (splitInput[0] == "$")
            {
                //Command is cd
                if (splitInput[1] == "cd")
                {
                    if (splitInput[2] == "/")
                    {
                        curDirectory.Clear();
                        curDirectory.Push("~");
                        continue;
                    }

                    if (splitInput[2] == "..")
                    {
                        curDirectory.Pop();
                        continue;
                    }

                    curDirectory.Push(splitInput[2]);
                    fs.LocateDir(ConvertStackToPath(curDirectory));
                }
                else if (splitInput[1] == "ls")
                {
                    int validLineIndex = i + 1;
                    while (validLineIndex < input.Length)
                    {
                        string[] fileData = input[validLineIndex].Split(' ');

                        //Found a command, quit
                        if (fileData[0] == "$")
                        {
                            i = validLineIndex - 1;
                            break;
                        }


                        if (fileData[0] == "dir")
                        {
                            fs.AddDir(ConvertStackToPath(curDirectory), fileData[1]);
                        }
                        else
                        {
                            FakeFile newFile = new FakeFile(ConvertStackToPath(curDirectory), fileData[1]);
                            newFile.size = Convert.ToInt32(fileData[0]);

                            fs.AddFile(ConvertStackToPath(curDirectory), newFile);
                        }

                        validLineIndex++;
                        i = validLineIndex;
                    }
                }
            }
            else
            {
                //Input is a file
                Console.WriteLine("UH OH!");
            }
        }

        fs.UpdateSize(fs.homeDir);

        List<FakeDirectory> removableDirs = new List<FakeDirectory>();
        fs.GrabSubDirs(ref removableDirs, fs.homeDir);

        int totalRoom = 70000000;
        int totalUsed = fs.homeDir.size;
        int freeSpace = totalRoom - totalUsed;
        int neededSpace = 30000000;
        int neededFrees = neededSpace - freeSpace;
        Console.WriteLine("N: " + neededFrees);
        //return;

        List<int> largestDirs = new List<int>();
        for (int i = 0; i < removableDirs.Count; i++)
        {
            if (removableDirs[i].size < neededFrees)
            {
                removableDirs.RemoveAt(i);
                i--;
                continue;
            }

            largestDirs.Add(removableDirs[i].size);
        }

        largestDirs.Sort();
        //largestDirs.Reverse();
        for (int i = 0; i < largestDirs.Count; i++)
        {
            Console.WriteLine((i + 1) + ": " + largestDirs[i]);
        }

        //int sum = largestDirs.Sum();
        /*
        for(int i = 0; i < 3; i++)
        {
            sum += largestDirs[i];
        }
        */

        Console.WriteLine("SMALLEST: " + largestDirs[0]);
    }

    string ConvertStackToPath(Stack<string> stack)
    {
        List<string> dirs = stack.ToList();
        dirs.Reverse();

        string path = "";
        foreach(string dir in dirs)
        {
            path += dir + "/";
        }

        return path;
    }

    public class FakeElement
    {
        public string path = "";
        public string name = "";

        public FakeElement(string p, string n)
        {
            path = p;
            name = n;
        }
    }

    public class FakeDirectory : FakeElement
    {
        public List<FakeElement> subElements;
        public int size;

        public FakeDirectory(string p, string n) : base(p, n)
        {
            subElements = new List<FakeElement>();
            size = 0;
        }
    }

    public class FakeFile : FakeElement
    {
        public int size;

        public FakeFile(string p, string n) : base(p, n)
        {
            size = 0;
        }
    }


    public class FakeFileSystem
    {
        public FakeDirectory homeDir = new FakeDirectory("~/", "~");

        public void AddDir(string path, string dirName)
        {
            FakeDirectory parentDir = LocateDir(path);
            FakeElement? elem = Array.Find(parentDir.subElements.ToArray(), (elem) => (elem is FakeDirectory && elem.name == dirName));

            if (elem == null)
            {
                FakeDirectory newDir = new FakeDirectory(path + dirName + "/", dirName);
                parentDir.subElements.Add(newDir);
            }
        }

        public FakeDirectory LocateDir(string path)
        {
            string[] dirs = path.Split('/');

            FakeDirectory curDirectory = homeDir;
            for (int i = 1; i < dirs.Length - 1; i++)
            {
                FakeElement? elem = Array.Find(curDirectory.subElements.ToArray(), (elem) => (elem is FakeDirectory && elem.name == dirs[i]));

                if (elem == null)
                {
                    Console.WriteLine("Elem is null! Making new Directory...");

                    FakeDirectory newDir = new FakeDirectory(curDirectory.path + dirs[i] + "/", dirs[i]);
                    curDirectory.subElements.Add(newDir);

                    curDirectory = newDir;
                }
                else if (elem.GetType() != typeof(FakeDirectory))
                {
                    Console.WriteLine("Very bad!");
                    return null;
                }
                else
                {
                    curDirectory = (FakeDirectory)elem;
                }

            }

            return curDirectory;
        }

        public void AddFile(string path, FakeFile file)
        {
            FakeDirectory dir = LocateDir(path);

            AddFile(dir, file);
        }

        public void AddFile(FakeDirectory dir, FakeFile file)
        {
            dir.subElements.Add(file);
        }

        public void UpdateSize(FakeDirectory curDir)
        {
            curDir.size = 0;

            foreach (FakeElement elem in curDir.subElements)
            {
                if (elem is FakeDirectory)
                {
                    UpdateSize((FakeDirectory)elem);
                    curDir.size += ((FakeDirectory)elem).size;
                }
                else
                {
                    curDir.size += ((FakeFile)elem).size;
                }
            }

            Console.WriteLine(curDir.path + " : " + curDir.size);
        }

        public void GrabSubDirs(ref List<FakeDirectory> dirList, FakeDirectory dir)
        {
            dirList.Add(dir);

            foreach (FakeElement e in dir.subElements)
            {
                if (e is FakeDirectory)
                {
                    GrabSubDirs(ref dirList, (FakeDirectory)e);
                }
            }
        }
    }
}


