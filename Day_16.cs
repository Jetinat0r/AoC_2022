using System.Collections;
using System.Drawing;
using static Day_16;
using static System.Formats.Asn1.AsnWriter;

public class Day_16
{
    public static int recursionCounter = 0;
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_16_Input.txt");

        Day_16_01(lines);
    }

    void Day_16_01(string[] input)
    {
        List<Valve> valves = new List<Valve>();
        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            string name = splitInput[1];
            int flowRate = Convert.ToInt32(splitInput[4].Substring(5, splitInput[4].Length - 6));

            List<string> outgoingValveNames = new();
            for(int j = 9; j < splitInput.Length; j++)
            {
                if(j != splitInput.Length - 1)
                {
                    //Cut out the comma
                    outgoingValveNames.Add(splitInput[j].Substring(0, 2));
                }
                else
                {
                    outgoingValveNames.Add(splitInput[j]);
                }
            }

            Valve newValve = new Valve(name, flowRate, outgoingValveNames);
            valves.Add(newValve);


            Console.Write($"{name} -> {{ ");
            for(int j = 0; j < outgoingValveNames.Count; j++)
            {
                Console.Write(outgoingValveNames[j] + " ");
            }

            Console.Write("}\n");
        }

        PriorityQueue<Valve, int> heapedValves = new PriorityQueue<Valve, int>();
        foreach(Valve v in valves)
        {
            v.SetupConnections(valves);
            heapedValves.Enqueue(v, v.flowVal);
        }

        //Put valves into an easier to access list
        //Don't include valves w/ a flow rate of 0
        List<Valve> sortedValves = new List<Valve>();
        while(heapedValves.Count > 0)
        {
            Valve cv = heapedValves.Dequeue();

            if(cv.flowVal == 0)
            {
                continue;
            }

            sortedValves.Add(cv);
        }

        sortedValves.Reverse();


        int score = 0;
        int timer = 30;

        Valve curValve = valves.Find(GetValveByName("AA"));

        /*
        for (int i = 0; i < sortedValves.Count; i++)
        {
            Console.WriteLine("AA -> " + sortedValves[i].name + "(" + sortedValves[i].flowVal + ") : " + curValve.GetPathLength(sortedValves[i]));
        }
        Console.WriteLine();

        for (int i = 0; i < sortedValves.Count; i++)
        {
            for(int j = 0; j < sortedValves.Count; j++)
            {
                if(i == j)
                {
                    continue;
                }

                Console.WriteLine(sortedValves[i].name + " -> " + sortedValves[j].name + "(" + sortedValves[j].flowVal + ") : " + sortedValves[i].GetPathLength(sortedValves[j]));
            }

            Console.WriteLine();
        }

        */

        sortedValves.Remove(valves.Find(GetValveByName("XQ")));

        //curValve.DFS(8, sortedValves);
        //Electric boogaloo
        Universe singularityTwo = new Universe(26, 0, sortedValves, curValve);
        singularityTwo.SplitUniverse();
        score = singularityTwo.GetMaxScore();
        //singularityTwo.SplitElephant();

        Console.WriteLine($"MAX SCORE: {score}");
        Console.WriteLine(singularityTwo.FindMaxPath(score));

        sortedValves.Add(valves.Find(GetValveByName("XQ")));
        sortedValves.Remove(valves.Find(GetValveByName("VP")));
        sortedValves.Remove(valves.Find(GetValveByName("VM")));
        sortedValves.Remove(valves.Find(GetValveByName("TR")));
        sortedValves.Remove(sortedValves.Find(GetValveByName("DO")));
        sortedValves.Remove(sortedValves.Find(GetValveByName("KI")));

        Universe singularityThree = new Universe(26, 0, sortedValves, curValve);
        singularityThree.SplitUniverse();
        score = singularityThree.GetMaxScore();

        Console.WriteLine($"MAX SCORE: {score}");
        Console.WriteLine(singularityThree.FindMaxPath(score));

        //score = singularityTwo.GetMaxScore();
        /*
        while(timer > 1)
        {
            int? bestPath = null;
            Valve bestTargetValve = null;

            for(int i = 0; i < sortedValves.Count; i++)
            {
                int newPath = curValve.GetPathLength(sortedValves[i]);

                if(newPath + 1 > timer)
                {
                    continue;
                }


                if(bestPath == null || newPath < bestPath)
                {
                    bestPath = newPath;
                    bestTargetValve = sortedValves[i];
                }
                else if(newPath == bestPath)
                {
                    //This is where I should split timelines!
                    Console.WriteLine("Parallel Universe???");
                }
            }


            if(bestPath == null)
            {
                break;
            }

            timer -= (int)bestPath + 1;
            score += timer * bestTargetValve.flowVal;
            curValve = bestTargetValve;
            sortedValves.Remove(bestTargetValve);
        }
        */


    }


    public class Valve
    {
        public string name;
        public int flowVal;
        public bool isOpen = false;

        public List<string> nameOutgoing = new();
        public Dictionary<string, Valve> LeadsTo = new();

        public Valve(string n, int flowValue, List<string> tempOutgoing)
        {
            name = n;
            flowVal = flowValue;
            this.nameOutgoing = tempOutgoing;
        }


        public void DFS(int depthRemaining, List<Valve> targets, List<Valve> visited = null)
        {
            if(visited == null)
            {
                visited = new();
            }

            if(depthRemaining == 2 || depthRemaining == 1)
            {
                Console.Write("AA");
                for(int i = 0; i < visited.Count; i++)
                {
                    Console.Write(" -> " + visited[i].name);
                }
                Console.WriteLine();
                
            }
            else if(depthRemaining == 0)
            {
                Console.Write("AA");
                for (int i = 0; i < visited.Count; i++)
                {
                    Console.Write(" -> " + visited[i].name);
                }
                Console.WriteLine();
                return;
            }

            for(int i = 0; i < targets.Count; i++)
            {
                List<Valve> newTargets = new List<Valve>(targets);
                newTargets.RemoveAt(i);

                List<Valve> newVisited = new List<Valve>(visited);
                newVisited.Add(targets[i]);

                targets[i].DFS(depthRemaining - 1, newTargets, newVisited);
            }
        }

        public void SetupConnections(List<Valve> availableValves)
        {
            foreach(string v in nameOutgoing)
            {
                Valve valve = availableValves.Find(GetValveByName(v));

                if(valve == null)
                {
                    throw new ArgumentNullException("Valve doesn't exist!");
                }

                LeadsTo.Add(v, valve);
            }
        }

        public int GetPathLength(Valve desiredValve, int curPathLength = 0, List<Valve> visited = null)
        {
            if(this == desiredValve)
            {
                return curPathLength;
            }

            if(visited == null)
            {
                visited = new();
            }

            visited.Add(this);

            List<Valve> curValveBreadth = new();
            List<Valve> nextValveBreadth = new();
            curValveBreadth.Add(this);

            while(curValveBreadth.Count > 0)
            {
                for(int i = 0; i < curValveBreadth.Count; i++)
                {
                    Valve v = curValveBreadth[i];
                    foreach(Valve v2 in v.LeadsTo.Values)
                    {
                        if (visited.Contains(v2))
                        {
                            //Skip
                            continue;
                        }

                        if (v2 == desiredValve)
                        {
                            return curPathLength + 1;
                        }

                        visited.Add(v2);
                        nextValveBreadth.Add(v2);
                    }
                    
                }

                curValveBreadth = nextValveBreadth;
                nextValveBreadth = new();
                curPathLength++;
            }

            return 99999;
        }
    }

    public class Universe
    {
        public int timeRemaining;
        public int curScore;
        public List<Valve> sortedValves;

        //Used to know where we can go from here
        public Valve myCurValve;
        public Valve elephantValve;
        public int meTimer;
        public int elephantTimer;

        public List<Universe> timelineSplits = new();

        public Universe(int tr, int score, List<Valve> openValves, Valve cv, Valve elephantValve = null, int meAvailable = 0, int elephantAvailable = 0)
        {
            timeRemaining = tr;
            curScore = score;
            sortedValves = openValves;
            myCurValve = cv;
            if(elephantValve == null)
            {
                this.elephantValve = cv;
            }
            else
            {
                this.elephantValve = elephantValve;
            }

            meTimer = meAvailable;
            elephantTimer = elephantAvailable;
        }

        public void SplitUniverse()
        {
            if (timeRemaining > 0)
            {
                //Find best valve
                for (int i = 0; i < sortedValves.Count; i++)
                {
                    Valve targetValve = sortedValves[i];
                    int newPathLength = myCurValve.GetPathLength(targetValve);


                    //Ensure we have enough time to execute the path & spinning
                    if (newPathLength + 1 <= timeRemaining)
                    {
                        //Start a new Universe
                        List<Valve> newSortedList = new List<Valve>();
                        for(int j = 0; j < i; j++)
                        {
                            newSortedList.Add(sortedValves[j]);
                        }
                        for(int j = i + 1; j < sortedValves.Count; j++)
                        {
                            newSortedList.Add(sortedValves[j]);
                        }
                        Universe newUniverse = new Universe(timeRemaining - (newPathLength + 1), curScore + ((timeRemaining - (newPathLength + 1)) * targetValve.flowVal), newSortedList, targetValve);
                        timelineSplits.Add(newUniverse);
                    }
                }

                foreach(Universe universe in timelineSplits)
                {
                    universe.SplitUniverse();
                }
            }
        }

        public void SplitElephant()
        {
            while(meTimer > 0 && elephantTimer > 0)
            {
                meTimer--;
                elephantTimer--;
                timeRemaining--;
            }
            if (timeRemaining > 0)
            {
                List<int> myPathSet = new();
                List<Valve> myValveSet = new();
                List<int> elephantPathSet = new();
                List<Valve> elephantValveSet = new();

                //int max = sortedValves.Count > 6 ? (int)Math.Ceiling(sortedValves.Count * 0.5) : sortedValves.Count;
                //Find best valve
                for (int i = 0; i < sortedValves.Count; i++)
                {
                    Valve targetValve = sortedValves[i];
                    int myNewPathLength = myCurValve.GetPathLength(targetValve);
                    int elephantNewPathLength = elephantValve.GetPathLength(targetValve);


                    //Ensure we have enough time to execute the path & spinning
                    if (myNewPathLength + 1 <= timeRemaining)
                    {
                        //Add it to the sets
                        myPathSet.Add(myNewPathLength);
                        myValveSet.Add(targetValve);
                    }

                    if(elephantNewPathLength + 1 <= timeRemaining)
                    {
                        elephantPathSet.Add(elephantNewPathLength);
                        elephantValveSet.Add(targetValve);
                    }
                }

                //Special case for Universe 0
                if(myCurValve == elephantValve)
                {
                    for(int i = 0; i < myPathSet.Count - 1; i++)
                    {
                        Valve myTargetValve = myValveSet[i];
                        for (int j = i + 1; j < myPathSet.Count; j++)
                        {
                            Valve elephantTargetValve = myValveSet[j];

                            if (myTargetValve == elephantTargetValve)
                            {
                                //Don't go to the same place!
                                continue;
                            }

                            List<Valve> newSortedList = new();

                            for (int k = 0; k < sortedValves.Count; k++)
                            {
                                newSortedList.Add(sortedValves[k]);
                            }

                            newSortedList.Remove(myTargetValve);
                            newSortedList.Remove(elephantTargetValve);

                            Universe bothUni = new Universe(timeRemaining - 1,
                                curScore + ((timeRemaining - myPathSet[i] - 1) * myTargetValve.flowVal) + ((timeRemaining - elephantPathSet[j] - 1) * elephantTargetValve.flowVal),
                                newSortedList,
                                myTargetValve,
                                elephantTargetValve,
                                myPathSet[i],
                                elephantPathSet[j]);

                            timelineSplits.Add(bothUni);
                        }
                    }


                    goto POST_ADD;
                }

                //Create all possible universe pairs starting with ME
                for(int i = 0; i < myPathSet.Count; i++)
                {
                    Valve myTargetValve = myValveSet[i];

                    for(int j = 0; j < elephantPathSet.Count; j++)
                    {
                        Valve elephantTargetValve = elephantValveSet[j];

                        if(myTargetValve == elephantTargetValve)
                        {
                            //Don't go to the same place!
                            continue;
                        }

                        List<Valve> newSortedList = new();

                        for(int k = 0; k < sortedValves.Count; k++)
                        {
                            newSortedList.Add(sortedValves[k]);
                        }


                        if(meTimer == 0 && elephantTimer == 0)
                        {
                            //We can both move
                            newSortedList.Remove(myTargetValve);
                            newSortedList.Remove(elephantTargetValve);

                            Universe bothUni = new Universe(timeRemaining - 1,
                                curScore + ((timeRemaining - myPathSet[i] - 1) * myTargetValve.flowVal) + ((timeRemaining - elephantPathSet[j] - 1) * elephantTargetValve.flowVal),
                                newSortedList,
                                myTargetValve,
                                elephantTargetValve,
                                myPathSet[i],
                                elephantPathSet[j]);

                            timelineSplits.Add(bothUni);
                        }
                        else if(meTimer == 0)
                        {
                            //I can move
                            newSortedList.Remove(myTargetValve);

                            Universe meUni = new Universe(timeRemaining - 1,
                                curScore + ((timeRemaining - myPathSet[i] - 1) * myTargetValve.flowVal),
                                newSortedList,
                                myTargetValve,
                                elephantValve,
                                myPathSet[i],
                                elephantTimer - 1);

                            timelineSplits.Add(meUni);
                        }
                        else if(elephantTimer == 0) //Check should always pass, here jic
                        {
                            //The elephant can move
                            newSortedList.Remove(elephantTargetValve);

                            Universe elUni = new Universe(timeRemaining - 1,
                                curScore + ((timeRemaining - elephantPathSet[j] - 1) * elephantTargetValve.flowVal),
                                newSortedList,
                                myCurValve,
                                elephantTargetValve,
                                meTimer - 1,
                                elephantPathSet[j]);

                            timelineSplits.Add(elUni);
                        }
                    }
                }

            POST_ADD:

                //Split the universes
                //Console.WriteLine($"Generating {timelineSplits.Count} universes");
                List<int> generatedScores = new();
                foreach (Universe universe in timelineSplits)
                {
                    generatedScores.Add(universe.curScore);
                    //universe.SplitElephant();
                }
                generatedScores.Sort();
                foreach (Universe universe in timelineSplits)
                {
                    if(universe.curScore >= generatedScores[generatedScores.Count / 4])
                    {
                        universe.SplitElephant();

                    }
                }
            }
        }

        public int GetMaxScore()
        {
            List<int> scores = new();
            scores.Add(curScore);

            foreach(Universe v in timelineSplits)
            {
                scores.Add(v.GetMaxScore());
            }

            return scores.Max();
        }

        public string FindMaxPath(int max)
        {
            if(curScore == max)
            {
                return myCurValve.name;
            }

            for(int i = 0; i < timelineSplits.Count; i++)
            { 
                Universe v = timelineSplits[i];
                string x = v.FindMaxPath(max);
                if(x != "")
                {
                    return myCurValve.name + " " + x;
                }
            }

            return "";
        }
    }

    //Gets the Valve with the specified name if it can
    public static Predicate<Valve> GetValveByName(string n)
    {
        return (valve) => valve.name == n;
    }



    public static int CollapseUniverse(Valve curValve, List<Valve> sortedValves, int time, int score)
    {
        List<Valve> childUniverses = new();

        while (time > 1)
        {
            int? bestPath = null;
            Valve bestTargetValve = null;

            for (int i = 0; i < sortedValves.Count; i++)
            {
                int newPath = curValve.GetPathLength(sortedValves[i]);

                if (newPath + 1 > time)
                {
                    continue;
                }


                if (bestPath == null || newPath < bestPath)
                {
                    bestPath = newPath;
                    bestTargetValve = sortedValves[i];
                }
                else if (newPath == bestPath)
                {
                    childUniverses.Add(sortedValves[i]);
                }
            }


            if (bestPath == null)
            {
                return score;
            }

            time -= (int)bestPath + 1;
            score += time * bestTargetValve.flowVal;
            curValve = bestTargetValve;
            sortedValves.Remove(bestTargetValve);
        }


        //Clean up paths
        for(int i = 0; i < childUniverses.Count; i++)
        {
            //if
        }

        //Find best child score


        return score;
    }
}