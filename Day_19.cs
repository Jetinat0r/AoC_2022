using System.Text.RegularExpressions;

public class Day_19
{
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_19_Input.txt");

        Day_19_01(lines);
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


        long totalScore = 0;
        /*
        for(int i = 0; i < blueprints.Count; i++)
        {
            blueprints[i].RunAllCycles();

            totalScore += blueprints[i].geodeCount * (i + 1);
        }
        */
        blueprints[0].RunAllCycles();
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

        //TODO: Smarter LOOK_AHEAD
        public int MAX_SIM_TIME = 24;
        public int LOOK_AHEAD = 3;

        public Blueprint(int id, int time, int[] oreReq, int[] clayReq, int[] obsReq, int[] geodeReq)
        {
            ID = id;

            timeRemaining = time;

            oreBotRecipe = oreReq;
            clayBotRecipe = clayReq;
            obsidianBotRecipe = obsReq;
            geodeBotRecipe = geodeReq;
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