using System.Collections;
using System.Text.RegularExpressions;

public class Day_19
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_19_Input.txt");

        Day_19_02(lines);
    }

    void Day_19_01(string[] input)
    {
        List<Blueprint> blueprints = new List<Blueprint>();

        //Parse input
        for(int i = 0; i < input.Length; i++)
        {
            Regex getBotInfo = new Regex(@"(ore|clay|obsidian|geode) robot[^.]+([\d]+ ore)*([\d]+ clay)*([\d]+ obsidian)*([\d]+ geodes)*[.]");
            MatchCollection matchedInfo = getBotInfo.Matches(input[i]);

            int[][] botInfo = new int[4][];
            for(int j = 0; j < matchedInfo.Count; j++)
            {
                botInfo[j] = new int[4];
                for(int k = 0; k < botInfo[j].Length; k++)
                {
                    botInfo[j][k] = 0;
                }

                string[] splitBotInfo = matchedInfo[j].Value.Split(' ');

                int infoIndex = 4;
                while(infoIndex < splitBotInfo.Length)
                {
                    switch (splitBotInfo[infoIndex])
                    {
                        case ("ore"):
                        case ("ore."):
                            botInfo[j][0] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("clay"):
                        case ("clay."):
                            botInfo[j][1] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("obsidian"):
                        case ("obsidian."):
                            botInfo[j][2] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("geodes"):
                        case ("geodes."):
                            botInfo[j][3] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;
                    }

                    infoIndex += 3;
                }

            }

            Blueprint newBlueprint = new Blueprint(i + 1, 24, botInfo[0], botInfo[1], botInfo[2], botInfo[3]);
            blueprints.Add(newBlueprint);
        }

        //845 is too low

        long totalScore = 0;
        for(int i = 0; i < blueprints.Count; i++)
        {
            List<Blueprint> subPrints = new();
            subPrints.Add(blueprints[i]);

            long curID = blueprints[i].ID;
            long largestGeodeCount = 0;
            long largestBotCount = 1;

            while(subPrints.Count > 0)
            {
                List<Blueprint> newPrints = new();

                while (subPrints.Count > 0)
                {
                    Blueprint curPrint = subPrints[0];
                    subPrints.RemoveAt(0);

                    newPrints.AddRange(curPrint.GenerateSubPrints());
                }

                //Find largest geode count & trim
                foreach(Blueprint b in newPrints)
                {
                    if(b.geodeCount > largestGeodeCount)
                    {
                        largestGeodeCount = b.geodeCount;
                    }
                }

                //Remove redundant paths
                for(int j = 0; j < newPrints.Count; j++)
                {
                    if(largestGeodeCount == 0)
                    {
                        Blueprint b = newPrints[j];
                        if(!Blueprint.Cheat(b.timeRemaining, b.oreCount, b.clayCount, b.obsidianCount, b.geodeCount, b.oreBots, b.clayBots, b.obsidianBots, b.geodeBots, b.obsidianBotRecipe[1], b.geodeBotRecipe[2], 0))
                        {
                            newPrints.RemoveAt(j);
                            j--;
                        }
                    }
                    else if(newPrints[j].geodeCount < largestGeodeCount)
                    {
                        newPrints.RemoveAt(j);
                        j--;
                    }
                }

                subPrints = newPrints;
            }

            Console.WriteLine($"{curID}: {largestGeodeCount}");
            totalScore += curID * largestGeodeCount;
        }

        Console.WriteLine(totalScore);
        //blueprints[0].RunAllCycles();
    }

    void Day_19_02(string[] input)
    {
        List<Blueprint> blueprints = new List<Blueprint>();
        BlueprintComparer bpc = new();
        //11840 is TOO LOW!

        int numToParse = input.Length == 2 ? 2 : 3;

        //Parse input
        for (int i = 0; i < numToParse; i++)
        {
            Regex getBotInfo = new Regex(@"(ore|clay|obsidian|geode) robot[^.]+([\d]+ ore)*([\d]+ clay)*([\d]+ obsidian)*([\d]+ geodes)*[.]");
            MatchCollection matchedInfo = getBotInfo.Matches(input[i]);

            int[][] botInfo = new int[4][];
            for (int j = 0; j < matchedInfo.Count; j++)
            {
                botInfo[j] = new int[4];
                for (int k = 0; k < botInfo[j].Length; k++)
                {
                    botInfo[j][k] = 0;
                }

                string[] splitBotInfo = matchedInfo[j].Value.Split(' ');

                int infoIndex = 4;
                while (infoIndex < splitBotInfo.Length)
                {
                    switch (splitBotInfo[infoIndex])
                    {
                        case ("ore"):
                        case ("ore."):
                            botInfo[j][0] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("clay"):
                        case ("clay."):
                            botInfo[j][1] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("obsidian"):
                        case ("obsidian."):
                            botInfo[j][2] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;

                        case ("geodes"):
                        case ("geodes."):
                            botInfo[j][3] = Convert.ToInt32(splitBotInfo[infoIndex - 1]);
                            break;
                    }

                    infoIndex += 3;
                }

            }

            Blueprint newBlueprint = new Blueprint(i + 1, 32, botInfo[0], botInfo[1], botInfo[2], botInfo[3]);
            blueprints.Add(newBlueprint);
        }

        long totalScore = 0;
        for (int i = 0; i < blueprints.Count; i++)
        {
            List<Blueprint> subPrints = new();
            subPrints.Add(blueprints[i]);

            long curID = blueprints[i].ID;
            long largestGeodeCount = 0;


            blueprints[i].GeneratePartTwoPrints(ref largestGeodeCount);
            /*
            while (subPrints.Count > 0)
            {
                int largestTheoreticalGeode = 0;
                List<Blueprint> newPrints = new();

                while (subPrints.Count > 0)
                {
                    Blueprint curPrint = subPrints[0];
                    subPrints.RemoveAt(0);

                    newPrints.AddRange(curPrint.GeneratePartTwoPrints(ref largestGeodeCount));
                }

                
                for(int j = 0; j < newPrints.Count; j++)
                {
                    if (newPrints[j].theoreticalMax == 0)
                    {
                        newPrints.RemoveAt(j);
                        j--;
                    }
                }
                
                subPrints = newPrints;
            }
            */

            Console.WriteLine($"{curID}: {largestGeodeCount}");
            if(totalScore == 0)
            {
                totalScore = largestGeodeCount;
            }
            else
            {
                totalScore *= largestGeodeCount;
            }
        }

        Console.WriteLine(totalScore);
        //blueprints[0].RunAllCycles();
    }

    public class BlueprintComparer : IComparer<Blueprint>
    {
        public int Compare(Blueprint x, Blueprint y)
        {
            if(x.theoreticalMax == y.theoreticalMax)
            {
                return 0;
            }
            else if (x.theoreticalMax < y.theoreticalMax)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    public class Blueprint
    {
        public int ID;

        public int timeRemaining;

        public long oreCount = 0;
        public long clayCount = 0;
        public long obsidianCount = 0;
        public long geodeCount = 0;

        public long oreBots = 1;
        public long clayBots = 0;
        public long obsidianBots = 0;
        public long geodeBots = 0;

        //Recipes go in order: req ore, req clay, req obsidian, req geodes
        public int[] oreBotRecipe;
        public int[] clayBotRecipe;
        public int[] obsidianBotRecipe;
        public int[] geodeBotRecipe;

        //Simulation Vars
        public bool skippedOre = false;
        public bool skippedClay = false;
        public bool skippedObsidian = false;
        public bool skippedGeode = false;

        public int timeSincePurchase = 0;

        public long theoreticalMax;
        public int maxOre;

        //TODO: Smarter LOOK_AHEAD
        public int MAX_SIM_TIME = 32;
        public int LOOK_AHEAD = 3;

        public Blueprint(int id, int time, int[] oreReq, int[] clayReq, int[] obsReq, int[] geodeReq)
        {
            ID = id;

            timeRemaining = time;

            oreBotRecipe = oreReq;
            clayBotRecipe = clayReq;
            obsidianBotRecipe = obsReq;
            geodeBotRecipe = geodeReq;

            if(oreReq[0] > clayReq[0])
            {
                if (oreReq[0] > obsReq[0])
                {
                    if(oreReq[0] > geodeReq[0])
                    {
                        maxOre = oreReq[0];
                    }
                    else
                    {
                        maxOre = geodeReq[0];
                    }
                }
                else
                {
                    if (obsReq[0] > geodeReq[0])
                    {
                        maxOre = obsReq[0];
                    }
                    else
                    {
                        maxOre = geodeReq[0];
                    }
                }
            }
            else
            {
                if (clayReq[0] > obsReq[0])
                {
                    if (clayReq[0] > geodeReq[0])
                    {
                        maxOre = clayReq[0];
                    }
                    else
                    {
                        maxOre = geodeReq[0];
                    }
                }
                else
                {
                    if (obsReq[0] > geodeReq[0])
                    {
                        maxOre = obsReq[0];
                    }
                    else
                    {
                        maxOre = geodeReq[0];
                    }
                }
            }

            theoreticalMax = Blueprint.CheatNum(timeRemaining, oreCount, clayCount, obsidianCount, geodeCount, oreBots, clayBots, obsidianBots, geodeBots, obsidianBotRecipe[1], geodeBotRecipe[2]);
        }

        public Blueprint(Blueprint other)
        {
            ID = other.ID;

            timeRemaining = other.timeRemaining;

            oreCount = other.oreCount;
            clayCount = other.clayCount;
            obsidianCount = other.obsidianCount;
            geodeCount = other.geodeCount;

            oreBots = other.oreBots;
            clayBots = other.clayBots;
            obsidianBots = other.obsidianBots;
            geodeBots = other.geodeBots;

            oreBotRecipe = other.oreBotRecipe;
            clayBotRecipe = other.clayBotRecipe;
            obsidianBotRecipe = other.obsidianBotRecipe;
            geodeBotRecipe = other.geodeBotRecipe;

            skippedOre = other.skippedOre;
            skippedClay = other.skippedClay;
            skippedObsidian = other.skippedObsidian;
            skippedGeode = other.skippedGeode;

            timeSincePurchase = other.timeSincePurchase;
            maxOre = other.maxOre;

            theoreticalMax = other.theoreticalMax;
        }

        public void UpdateTheoreticalMax()
        {
            theoreticalMax = Blueprint.CheatNum(timeRemaining, oreCount, clayCount, obsidianCount, geodeCount, oreBots, clayBots, obsidianBots, geodeBots, obsidianBotRecipe[1], geodeBotRecipe[2]);
        }

        public void RunAllCycles()
        {
            while(timeRemaining > 0)
            {
                RunCycle();
            }

            Console.WriteLine($"Blueprint ({ID}) ended with ({geodeCount}) geodes for a total score of ({ID * geodeCount})");
        }

        public void RunCycle()
        {
            //If we're out of time, end!
            if(timeRemaining == 0)
            {
                return;
            }
            if(timeRemaining < 0)
            {
                throw new Exception("OUCH");
            }

            //Buy a new bot
            int[] newBots = BuyBots();

            //Collect resources
            oreCount += oreBots;
            clayCount += clayBots;
            obsidianCount += obsidianBots;
            geodeCount += geodeBots;


            //Add the newly created bot to the masses
            oreBots += newBots[0];
            clayBots += newBots[1];
            obsidianBots += newBots[2];
            geodeBots += newBots[3];


            timeRemaining--;
        }

        public List<Blueprint> GeneratePartTwoPrints(ref long largestGeodeCount)
        {
            List<Blueprint> subPrints = new List<Blueprint>();
            if (timeRemaining == 0 || theoreticalMax == 0 || theoreticalMax < largestGeodeCount)
            {
                return subPrints;
            }


            int timeToOreBot = SimulateOreBotGeneration(timeRemaining, oreCount, oreBots) + 1;
            int timeToClayBot = SimulateClayBotGeneration(timeRemaining,
                            oreCount,
                            oreBots) + 1;
            int timeToObsidianBot = SimulateObsidianBotGeneration(timeRemaining,
                            oreCount,
                            oreBots,
                            clayCount,
                            clayBots) + 1;
            int timeToGeodeBot = SimulateGeodeBotGeneration(timeRemaining,
                            oreCount,
                            oreBots,
                            obsidianCount,
                            obsidianBots) + 1;


            if(timeToOreBot < 999 && oreBots < maxOre)
            {
                Blueprint orePrint = new Blueprint(this);

                orePrint.timeRemaining -= timeToOreBot;

                orePrint.oreCount += (orePrint.oreBots * timeToOreBot);
                orePrint.clayCount += (orePrint.clayBots * timeToOreBot);
                orePrint.obsidianCount += (orePrint.obsidianBots * timeToOreBot);
                orePrint.geodeCount += (orePrint.geodeBots * timeToOreBot);

                orePrint.oreCount -= oreBotRecipe[0];

                orePrint.oreBots += 1;

                orePrint.UpdateTheoreticalMax();

                subPrints.Add(orePrint);
            }

            if (timeToClayBot < 999 && clayBots < obsidianBotRecipe[1])
            {
                Blueprint clayPrint = new Blueprint(this);

                clayPrint.timeRemaining -= timeToClayBot;

                clayPrint.oreCount += (clayPrint.oreBots * timeToClayBot);
                clayPrint.clayCount += (clayPrint.clayBots * timeToClayBot);
                clayPrint.obsidianCount += (clayPrint.obsidianBots * timeToClayBot);
                clayPrint.geodeCount += (clayPrint.geodeBots * timeToClayBot);

                clayPrint.oreCount -= clayBotRecipe[0];

                clayPrint.clayBots += 1;

                clayPrint.UpdateTheoreticalMax();

                subPrints.Add(clayPrint);
            }

            if (timeToObsidianBot < 999 && obsidianBots < geodeBotRecipe[2])
            {
                Blueprint obsidianPrint = new Blueprint(this);

                obsidianPrint.timeRemaining -= timeToObsidianBot;

                obsidianPrint.oreCount += (obsidianPrint.oreBots * timeToObsidianBot);
                obsidianPrint.clayCount += (obsidianPrint.clayBots * timeToObsidianBot);
                obsidianPrint.obsidianCount += (obsidianPrint.obsidianBots * timeToObsidianBot);
                obsidianPrint.geodeCount += (obsidianPrint.geodeBots * timeToObsidianBot);

                obsidianPrint.oreCount -= obsidianBotRecipe[0];
                obsidianPrint.clayCount -= obsidianBotRecipe[1];

                obsidianPrint.obsidianBots += 1;

                obsidianPrint.UpdateTheoreticalMax();

                subPrints.Add(obsidianPrint);
            }

            if (timeToGeodeBot < 999)
            {
                Blueprint geodePrint = new Blueprint(this);

                geodePrint.timeRemaining -= timeToGeodeBot;

                geodePrint.oreCount += (geodePrint.oreBots * timeToGeodeBot);
                geodePrint.clayCount += (geodePrint.clayBots * timeToGeodeBot);
                geodePrint.obsidianCount += (geodePrint.obsidianBots * timeToGeodeBot);
                geodePrint.geodeCount += (geodePrint.geodeBots * timeToGeodeBot);

                geodePrint.oreCount -= geodeBotRecipe[0];
                geodePrint.obsidianCount -= geodeBotRecipe[2];

                geodePrint.geodeBots += 1;

                //geodePrint.geodeCount += geodePrint.timeRemaining;

                geodePrint.UpdateTheoreticalMax();

                subPrints.Add(geodePrint);
            }


            if(subPrints.Count == 0)
            {
                //Finish geode calculations
                long finalGeodes = geodeCount + (timeRemaining * geodeBots);
                if(finalGeodes > largestGeodeCount)
                {
                    largestGeodeCount = finalGeodes;
                }
            }

            foreach (Blueprint b in subPrints)
            {
                if(b.geodeCount > largestGeodeCount)
                {
                    largestGeodeCount = b.geodeCount;
                }

                b.GeneratePartTwoPrints(ref largestGeodeCount);
            }

            return new();
        }


        public List<Blueprint> GenerateSubPrints()
        {
            List<Blueprint> subPrints = new List<Blueprint>();
            if(timeRemaining == 0)
            {
                return subPrints;
            }

            Blueprint finalPrint = new(this);
            finalPrint.timeRemaining -= 1;

            finalPrint.oreCount += finalPrint.oreBots;
            finalPrint.clayCount += finalPrint.clayBots;
            finalPrint.obsidianCount += finalPrint.obsidianBots;
            finalPrint.geodeCount += finalPrint.geodeBots;


            bool hasAtLeastOnePath = false;
            //WE ALWAYS BUY A GEODE BOT
            if (CanBuyGeodeBot())
            {
                hasAtLeastOnePath = true;

                Blueprint geodeSub = new(this);
                geodeSub.skippedOre = false;
                geodeSub.skippedClay = false;
                geodeSub.skippedObsidian = false;
                geodeSub.skippedGeode = false;

                geodeSub.timeRemaining -= 1;

                //Spend resources
                geodeSub.PurchaseGeodeBot();

                //Tick up resources
                geodeSub.oreCount += geodeSub.oreBots;
                geodeSub.clayCount += geodeSub.clayBots;
                geodeSub.obsidianCount += geodeSub.obsidianBots;
                geodeSub.geodeCount += geodeSub.geodeBots;

                geodeSub.geodeBots += 1;

                subPrints.Add(geodeSub);
                return subPrints;
            }
            if (CanBuyObsidianBot() && !skippedObsidian && obsidianBots < geodeBotRecipe[2])
            {
                hasAtLeastOnePath = true;

                Blueprint buyObsidianSub = new(this);
                buyObsidianSub.skippedOre = false;
                buyObsidianSub.skippedClay = false;
                buyObsidianSub.skippedObsidian = false;
                buyObsidianSub.skippedGeode = false;

                buyObsidianSub.timeRemaining -= 1;

                //Spend resources
                buyObsidianSub.PurchaseObsidianBot();

                //Tick up resources
                buyObsidianSub.oreCount += buyObsidianSub.oreBots;
                buyObsidianSub.clayCount += buyObsidianSub.clayBots;
                buyObsidianSub.obsidianCount += buyObsidianSub.obsidianBots;
                buyObsidianSub.geodeCount += buyObsidianSub.geodeBots;

                buyObsidianSub.obsidianBots += 1;

                subPrints.Add(buyObsidianSub);


                finalPrint.skippedObsidian = true;
                subPrints.Add(finalPrint);
                return subPrints;
            }
            if (CanBuyClayBot() && !skippedClay && clayBots < obsidianBotRecipe[1])
            {
                hasAtLeastOnePath = true;

                Blueprint buyClaySub = new(this);
                buyClaySub.skippedOre = false;
                buyClaySub.skippedClay = false;
                buyClaySub.skippedObsidian = false;
                buyClaySub.skippedGeode = false;

                buyClaySub.timeRemaining -= 1;

                //Spend resources
                buyClaySub.PurchaseClayBot();

                //Tick up resources
                buyClaySub.oreCount += buyClaySub.oreBots;
                buyClaySub.clayCount += buyClaySub.clayBots;
                buyClaySub.obsidianCount += buyClaySub.obsidianBots;
                buyClaySub.geodeCount += buyClaySub.geodeBots;

                buyClaySub.clayBots += 1;

                subPrints.Add(buyClaySub);


                finalPrint.skippedClay = true;
            }
            if (CanBuyOreBot() && !skippedOre && oreBots < maxOre)
            {
                hasAtLeastOnePath = true;

                Blueprint buyOreSub = new(this);
                buyOreSub.skippedOre = false;
                buyOreSub.skippedClay = false;
                buyOreSub.skippedObsidian = false;
                buyOreSub.skippedGeode = false;

                buyOreSub.timeRemaining -= 1;

                //Spend resources
                buyOreSub.PurchaseOreBot();

                //Tick up resources
                buyOreSub.oreCount += buyOreSub.oreBots;
                buyOreSub.clayCount += buyOreSub.clayBots;
                buyOreSub.obsidianCount += buyOreSub.obsidianBots;
                buyOreSub.geodeCount += buyOreSub.geodeBots;

                buyOreSub.oreBots += 1;

                subPrints.Add(buyOreSub);

                /*
                Blueprint skipOreSub = new(this);
                skipOreSub.skippedOre = true;

                skipOreSub.timeRemaining -= 1;

                //Tick up resources
                skipOreSub.oreCount += skipOreSub.oreBots;
                skipOreSub.clayCount += skipOreSub.clayBots;
                skipOreSub.obsidianCount += skipOreSub.obsidianBots;
                skipOreSub.geodeCount += skipOreSub.geodeBots;

                subPrints.Add(skipOreSub);
                */

                finalPrint.skippedOre = true;
            }

            /*
            if (!CanBuyGeodeBot() &&
                (!CanBuyObsidianBot() || skippedObsidian) &&
                (!CanBuyClayBot() || skippedClay) &&
                (!CanBuyOreBot() || skippedOre))
            {
                //Generate a default path
                Blueprint skipAllSub = new(this);
                //skipAllSub.skippedOre = true;

                skipAllSub.timeRemaining -= 1;

                //Tick up resources
                skipAllSub.oreCount += skipAllSub.oreBots;
                skipAllSub.clayCount += skipAllSub.clayBots;
                skipAllSub.obsidianCount += skipAllSub.obsidianBots;
                skipAllSub.geodeCount += skipAllSub.geodeBots;

                subPrints.Add(skipAllSub);
            }
            */

            subPrints.Add(finalPrint);

            return subPrints;
        }

        public static bool Cheat(int time, long ore, long clay, long obs, long geo, long oreBots, long clayBots, long obsBots, long geoBots, int obsCost, int geoCost, int largestGeo)
        {
            if(time == 0)
            {
                return false;
            }

            time -= 1;

            int obsToBuy = 0;
            int geoToBuy = 0;
            //Buy robots in order
            if (obs >= geoCost)
            {
                geoToBuy += 1;
                obs -= geoCost;
            }
            if (clay >= obsCost)
            {
                obsToBuy += 1;
                clay -= obsCost;
            }

            ore += oreBots;
            clay += clayBots;
            obs += obsBots;
            geo += geoBots;

            if(geo > 0 && geo >= largestGeo)
            {
                return true;
            }

            oreBots += 1;
            clayBots += 1;
            obsBots += obsToBuy;
            geoBots += geoToBuy;

            return Cheat(time, ore, clay, obs, geo, oreBots, clayBots, obsBots, geoBots, obsCost, geoCost, largestGeo);
        }

        public static bool Cheat(int time, long ore, long clay, long obs, long geo, long oreBots, long clayBots, long obsBots, long geoBots, int obsCost, int geoCost, ref int largestGeo)
        {
            if (time == 0)
            {
                return false;
            }

            time -= 1;

            int obsToBuy = 0;
            int geoToBuy = 0;
            //Buy robots in order
            if (obs >= geoCost)
            {
                geoToBuy += 1;
                obs -= geoCost;
            }
            if (clay >= obsCost)
            {
                obsToBuy += 1;
                clay -= obsCost;
            }

            ore += oreBots;
            clay += clayBots;
            obs += obsBots;
            geo += geoBots;

            if (geo > 0 && geo >= largestGeo)
            {
                largestGeo = (int)geo;
                return true;
            }

            oreBots += 1;
            clayBots += 1;
            obsBots += obsToBuy;
            geoBots += geoToBuy;

            return Cheat(time, ore, clay, obs, geo, oreBots, clayBots, obsBots, geoBots, obsCost, geoCost, largestGeo);
        }

        public static long CheatNum(int time, long ore, long clay, long obs, long geo, long oreBots, long clayBots, long obsBots, long geoBots, int obsCost, int geoCost)
        {
            if (time == 0)
            {
                return geo;
            }

            time -= 1;

            int obsToBuy = 0;
            int geoToBuy = 0;
            //Buy robots in order
            if (obs >= geoCost)
            {
                geoToBuy += 1;
                obs -= geoCost;
            }
            if (clay >= obsCost)
            {
                obsToBuy += 1;
                clay -= obsCost;
            }

            ore += oreBots;
            clay += clayBots;
            obs += obsBots;
            geo += geoBots;

            oreBots += 1;
            clayBots += 1;
            obsBots += obsToBuy;
            geoBots += geoToBuy;

            return CheatNum(time, ore, clay, obs, geo, oreBots, clayBots, obsBots, geoBots, obsCost, geoCost);
        }

        public int[] BuyBots()
        {
            //Generate an emtpy list for later
            int[] boughtBot = new int[4];
            for(int i = 0; i < boughtBot.Length; i++)
            {
                boughtBot[i] = 0;
            }

            //Do logic to buy robots *BLEGH*
            long predictedOre = oreCount + (oreBots * LOOK_AHEAD);
            long predictedClay = clayCount + (clayBots * LOOK_AHEAD);
            long predictedObsidian = obsidianCount + (obsidianBots * LOOK_AHEAD);
            long predictedGeode = geodeCount + (geodeBots * LOOK_AHEAD);

            //For each bot:
            //0: Calc time to get 1 if no changes made
            //UNNECESSARY: Calc time to get 1 if one more GeodeBot was obtained
            //3: Calc time to get 1 if one more ObsidianBot was obtained
            //2: Calc time to get 1 if one more ClayBot was obtained
            //1: Calc time to get 1 if one more OreBot was obtained

            int timeToOreBot = SimulateOreBotGeneration(timeRemaining, oreCount, oreBots);

            int timeToClayBot = SimulateClayBotGeneration(timeRemaining,
                oreCount,
                oreBots);
            int timeToClayAndOreBot = SimulateClayBotGeneration(timeRemaining - timeToOreBot,
                oreCount - oreBotRecipe[0],
                oreBots + 1,
                timeToOreBot);

            int timeToObsidianBot = SimulateObsidianBotGeneration(timeRemaining,
                oreCount,
                oreBots,
                clayCount,
                clayBots);
            int timeToObsidianAndOreBot = SimulateObsidianBotGeneration(timeRemaining - timeToOreBot,
                oreCount - oreBotRecipe[0],
                oreBots + 1,
                clayCount,
                clayBots,
                timeToOreBot);
            int timeToObsidianAndClayBot = SimulateObsidianBotGeneration(timeRemaining - timeToClayBot,
                oreCount - clayBotRecipe[0],
                oreBots,
                clayCount,
                clayBots + 1,
                timeToClayBot);
            int timeToObsidianAndOreAndClayBot = SimulateObsidianBotGeneration(timeRemaining - timeToClayAndOreBot,
                oreCount - oreBotRecipe[0] - clayBotRecipe[0],
                oreBots + 1,
                clayCount,
                clayBots + 1,
                timeToClayAndOreBot);

            int timeToGeodeBot = SimulateGeodeBotGeneration(timeRemaining,
                oreCount,
                oreBots,
                obsidianCount,
                obsidianBots);
            int timeToGeodeAndOreBot = SimulateGeodeBotGeneration(timeRemaining - timeToOreBot,
                oreCount - oreBotRecipe[0],
                oreBots + 1,
                obsidianCount,
                obsidianBots,
                timeToOreBot);
            int timeToGeodeAndClayBot = SimulateGeodeBotGeneration(timeRemaining - timeToClayBot,
                oreCount - clayBotRecipe[0],
                oreBots,
                obsidianCount,
                obsidianBots,
                timeToClayBot);
            int timeToGeodeAndObsidianBot = SimulateGeodeBotGeneration(timeRemaining - timeToObsidianBot,
                oreCount - obsidianBotRecipe[0],
                oreBots,
                obsidianCount,
                obsidianBots + 1,
                timeToObsidianBot);
            int timeToGeodeAndOreAndClayBot = SimulateGeodeBotGeneration(timeRemaining - timeToClayAndOreBot,
                oreCount - oreBotRecipe[0] - clayBotRecipe[0],
                oreBots + 1,
                obsidianCount,
                obsidianBots,
                timeToClayAndOreBot);
            int timeToGeodeAndOreAndObsidianBot = SimulateGeodeBotGeneration(timeRemaining - timeToObsidianAndOreBot,
                oreCount - oreBotRecipe[0] - obsidianBotRecipe[0],
                oreBots + 1,
                obsidianCount,
                obsidianBots + 1,
                timeToObsidianAndOreBot);
            int timeToGeodeAndClayAndObsidianBot = SimulateGeodeBotGeneration(timeRemaining - timeToObsidianAndClayBot,
                oreCount - clayBotRecipe[0] - obsidianBotRecipe[0],
                oreBots,
                obsidianCount,
                obsidianBots + 1,
                timeToObsidianAndClayBot);
            int timeToGeodeAndOreAndClayAndObsidianBot = SimulateGeodeBotGeneration(timeRemaining - timeToObsidianAndClayBot,
                oreCount - oreBotRecipe[0] - clayBotRecipe[0] - obsidianBotRecipe[0],
                oreBots + 1,
                obsidianCount,
                obsidianBots + 1,
                timeToObsidianAndOreAndClayBot);

            Console.WriteLine($"Time Remaining:                                                 {timeRemaining}");
            Console.WriteLine();
            Console.WriteLine("Bot Counts:");
            Console.WriteLine($"Ore Bots:                                                       {oreBots}");
            Console.WriteLine($"Clay Bots:                                                      {clayBots}");
            Console.WriteLine($"Obsidian Bots:                                                  {obsidianBots}");
            Console.WriteLine($"Geode Bots:                                                     {geodeBots}");
            Console.WriteLine();
            Console.WriteLine("Resource Counts:");
            Console.WriteLine($"Ore Count:                                                      {oreCount}");
            Console.WriteLine($"Clay Count:                                                     {clayCount}");
            Console.WriteLine($"Obsidian Count:                                                 {obsidianCount}");
            Console.WriteLine($"Geode Count:                                                    {geodeCount}");
            Console.WriteLine();
            Console.WriteLine("Ore Times:");
            Console.WriteLine($"Time to Ore:                                                    {timeToOreBot}");
            Console.WriteLine();
            Console.WriteLine("Clay Times:");
            Console.WriteLine($"Time to Clay:                                                   {timeToClayBot}");
            Console.WriteLine($"Time to Clay        and Ore:                                    {timeToClayAndOreBot}");
            Console.WriteLine();
            Console.WriteLine("Obsidian Times:");
            Console.WriteLine($"Time to Obsidian:                                               {timeToObsidianBot}");
            Console.WriteLine($"Time to Obsidian    and Ore:                                    {timeToObsidianAndOreBot}");
            Console.WriteLine($"Time to Obsidian                and Clay:                       {timeToObsidianAndClayBot}");
            Console.WriteLine($"Time to Obsidian    and Ore     and Clay:                       {timeToObsidianAndOreAndClayBot}");
            Console.WriteLine();
            Console.WriteLine("Geode Times:");
            Console.WriteLine($"Time to Geode:                                                  {timeToGeodeBot}");
            Console.WriteLine($"Time to Geode       and Ore:                                    {timeToGeodeAndOreBot}");
            Console.WriteLine($"Time to Geode                   and Clay:                       {timeToGeodeAndClayBot}");
            Console.WriteLine($"Time to Geode                               and Obsidian:       {timeToGeodeAndObsidianBot}");
            Console.WriteLine($"Time to Geode       and Ore     and Clay:                       {timeToGeodeAndOreAndClayBot}");
            Console.WriteLine($"Time to Geode       and Ore                 and Obsidian:       {timeToGeodeAndOreAndObsidianBot}");
            Console.WriteLine($"Time to Geode                   and Clay    and Obsidian:       {timeToGeodeAndClayAndObsidianBot}");
            Console.WriteLine($"Time to Geode       and Ore     and Clay    and Obsidian:       {timeToGeodeAndOreAndClayAndObsidianBot}");
            Console.WriteLine();
            Console.WriteLine("===========================================================================================================");


            Random r = new Random();
            if (CanBuyGeodeBot() && r.Next(0, 2) >= 1)
            {
                PurchaseGeodeBot();
                boughtBot[3] = 1;
            }
            else if (CanBuyObsidianBot() && r.Next(0, 4) >= 1)
            {
                PurchaseObsidianBot();
                boughtBot[2] = 1;
            }
            else if (CanBuyClayBot() && r.Next(0, 4) >= 2)
            {
                PurchaseClayBot();
                boughtBot[1] = 1;
            }
            else if (CanBuyOreBot() && r.Next(0, 4) >= 2)
            {
                PurchaseOreBot();
                boughtBot[0] = 1;
            }

            return boughtBot;
        }

        //Smarter Bot Predictions
        public int SimulateOreBotGeneration(int timeRemaining, long curOre, long curOreBots, int simTime = 0)
        {
            //If we can't make it in time, die and make it undesirable
            if(timeRemaining == 0 || simTime > MAX_SIM_TIME)
            {
                return 99999;
            }

            //If we've made it, say so!
            if(curOre >= oreBotRecipe[0])
            {
                return simTime;
            }

            //We haven't made it, keep trying!
            return SimulateOreBotGeneration(timeRemaining - 1, curOre + curOreBots, curOreBots, simTime + 1);
        }

        public int SimulateClayBotGeneration(int timeRemaining, long curOre, long curOreBots, int simTime = 0)
        {
            //If we can't make it in time, die and make it undesirable
            if (timeRemaining == 0 || simTime > MAX_SIM_TIME)
            {
                return 99999;
            }

            //If we've made it, say so!
            if (curOre >= clayBotRecipe[0])
            {
                return simTime;
            }

            //We haven't made it, keep trying!
            return SimulateClayBotGeneration(timeRemaining - 1, curOre + curOreBots, curOreBots, simTime + 1);
        }

        public int SimulateObsidianBotGeneration(int timeRemaining, long curOre, long curOreBots, long curClay, long curClayBots, int simTime = 0)
        {
            //If we can't make it in time, die and make it undesirable
            if (timeRemaining == 0 || simTime > MAX_SIM_TIME)
            {
                return 99999;
            }

            //If we've made it, say so!
            if (curOre >= obsidianBotRecipe[0] && curClay >= obsidianBotRecipe[1])
            {
                return simTime;
            }

            //We haven't made it, keep trying!
            return SimulateObsidianBotGeneration(timeRemaining - 1, curOre + curOreBots, curOreBots, curClay + curClayBots, curClayBots, simTime + 1);
        }

        public int SimulateGeodeBotGeneration(int timeRemaining, long curOre, long curOreBots, long curObsidian, long curObsidianBots, int simTime = 0)
        {
            //If we can't make it in time, die and make it undesirable
            if (timeRemaining == 0 || simTime > MAX_SIM_TIME)
            {
                return 99999;
            }

            //If we've made it, say so!
            if (curOre >= geodeBotRecipe[0] && curObsidian >= geodeBotRecipe[2])
            {
                return simTime;
            }

            //We haven't made it, keep trying!
            return SimulateGeodeBotGeneration(timeRemaining - 1, curOre + curOreBots, curOreBots, curObsidian + curObsidianBots, curObsidianBots, simTime + 1);
        }

        //Current Purchases
        public bool CanBuyOreBot()
        {
            return (oreCount >= oreBotRecipe[0]) &&
                (clayCount >= oreBotRecipe[1]) &&
                (obsidianCount >= oreBotRecipe[2]) &&
                (geodeCount >= oreBotRecipe[3]);
        }

        public bool CanBuyClayBot()
        {
            return (oreCount >= clayBotRecipe[0]) &&
                (clayCount >= clayBotRecipe[1]) &&
                (obsidianCount >= clayBotRecipe[2]) &&
                (geodeCount >= clayBotRecipe[3]);
        }

        public bool CanBuyObsidianBot()
        {
            return (oreCount >= obsidianBotRecipe[0]) &&
                (clayCount >= obsidianBotRecipe[1]) &&
                (obsidianCount >= obsidianBotRecipe[2]) &&
                (geodeCount >= obsidianBotRecipe[3]);
        }

        public bool CanBuyGeodeBot()
        {
            return (oreCount >= geodeBotRecipe[0]) &&
                (clayCount >= geodeBotRecipe[1]) &&
                (obsidianCount >= geodeBotRecipe[2]) &&
                (geodeCount >= geodeBotRecipe[3]);
        }


        //Predictive Purchases
        public bool PredictedBuyOreBot(long predictedOreCount, long predictedClayCount, long predictedObsidianCount, long predictedGeodeCount)
        {
            return (predictedOreCount >= oreBotRecipe[0]) &&
                (predictedClayCount >= oreBotRecipe[1]) &&
                (predictedObsidianCount >= oreBotRecipe[2]) &&
                (predictedGeodeCount >= oreBotRecipe[3]);
        }

        public bool PredictedBuyClayBot(long predictedOreCount, long predictedClayCount, long predictedObsidianCount, long predictedGeodeCount)
        {
            return (predictedOreCount >= clayBotRecipe[0]) &&
                (predictedClayCount >= clayBotRecipe[1]) &&
                (predictedObsidianCount >= clayBotRecipe[2]) &&
                (predictedGeodeCount >= clayBotRecipe[3]);
        }

        public bool PredictedBuyObsidianBot(long predictedOreCount, long predictedClayCount, long predictedObsidianCount, long predictedGeodeCount)
        {
            return (predictedOreCount >= obsidianBotRecipe[0]) &&
                (predictedClayCount >= obsidianBotRecipe[1]) &&
                (predictedObsidianCount >= obsidianBotRecipe[2]) &&
                (predictedGeodeCount >= obsidianBotRecipe[3]);
        }

        public bool PredictedBuyGeodeBot(long predictedOreCount, long predictedClayCount, long predictedObsidianCount, long predictedGeodeCount)
        {
            return (predictedOreCount >= geodeBotRecipe[0]) &&
                (predictedClayCount >= geodeBotRecipe[1]) &&
                (predictedObsidianCount >= geodeBotRecipe[2]) &&
                (predictedGeodeCount >= geodeBotRecipe[3]);
        }


        //Spend resources
        public void PurchaseOreBot()
        {
            oreCount -= oreBotRecipe[0];
            clayCount -= oreBotRecipe[1];
            obsidianCount -= oreBotRecipe[2];
            geodeCount -= oreBotRecipe[3];
        }

        public void PurchaseClayBot()
        {
            oreCount -= clayBotRecipe[0];
            clayCount -= clayBotRecipe[1];
            obsidianCount -= clayBotRecipe[2];
            geodeCount -= clayBotRecipe[3];
        }

        public void PurchaseObsidianBot()
        {
            oreCount -= obsidianBotRecipe[0];
            clayCount -= obsidianBotRecipe[1];
            obsidianCount -= obsidianBotRecipe[2];
            geodeCount -= obsidianBotRecipe[3];
        }

        public void PurchaseGeodeBot()
        {
            oreCount -= geodeBotRecipe[0];
            clayCount -= geodeBotRecipe[1];
            obsidianCount -= geodeBotRecipe[2];
            geodeCount -= geodeBotRecipe[3];
        }
    }
}