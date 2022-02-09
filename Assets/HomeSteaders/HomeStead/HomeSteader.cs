using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// People work as follows
//  They Need a house to live (Beds in upgrades) 1 per house.
//  A person with a House also cost you 1 Food (Bread)
//      A person who moves in also cost you 1 Food (Bread)
//      (Regardless if they are working)
//      (As counts towards Helpers/Moves)
//  They Want a place to work (WorkStations in upgrades) 1 per Worksite.
//      If they don't work, they are added to the Moves list and Helpers
//      You tax them 1 per People
//      You tax them 1 per Job have
//  Taxes might go up, based on their Income level
// 
//  A person regardless of their job cost you 1 Coin and 1 Food (Bread)
//  A person with a job is 2 Tax (Netting you 1 Coin)
//      A person wwith a Job (That you have to buy 1 Food) nets you zero taxes
//          Assuming Food (Bread) = 1 Gold

namespace HomeStead
{
    /// <summary>
    /// Items that are traded and used to build things
    /// WheatSeed (Future Idea) (Used to Plant in Farms)
    /// Gold - Money (Mined Resources)
    /// Wood - Lumber (Building Material) (Cleared from Forests)
    /// Rock - Stone (Building Material) (Cleared from Rocks)
    /// Wheat - Farmed from Farms (Wheat Stalk) (Wheat Flour)
    /// Bread - Food - that is Farmed from Wheat, and turned into Bread at a Bakery
    /// </summary>
    public enum Items
    {
        Gold, Wood, Rock, Wheat, Bread, 
    }

    /// <summary>
    /// Things in a weekly report (That is what is happening per week)
    /// IE Taxes per week, still make Money (But how you got it)
    /// People - Generic term for Humans
    /// Helpers - Unemployed People (Who add to your own 'Personal' Helper)
    /// Jobs - People who have Work (Gives taxes)
    /// JobSeekers - People who have a Home (but no work)
    /// Migrants - People that are not quite moved in (Come from nowhere - out of thin air)
    /// Moves - People (Job/JobSeekers) who are fed up, no houses/food (Recorded for one Week tick)
    /// Moves - How mnay Moves you as the Player has (Clicks)
    /// Taxes - People with Jobs, Food and Homes pay this Per week
    /// </summary>
    public enum Things
    {
        People, Helpers, Jobs, JobSeekers, Migrants, Movers, Moves, Taxes
    }


    /// <summary>
    /// Json class to hold our data for our game 'Character'
    /// Keeps track of our Map pieces, and links all our items together.
    /// </summary>
    [System.Serializable]
    public class HomeSteader
    {
        /// <summary>
        /// What we call ourself, NO checking. IE 'Micaiah'
        /// </summary>
        public string Name;
        /// <summary>
        /// Our Internal ID - built off a GUID
        /// </summary>
        public string ID;
        /// <summary>
        /// Our generic moves, we can only do so much so often
        /// </summary>
        public int Moves = 1;
        /// <summary>
        /// Helper is a term for 'Our' user input into the system.
        /// So we always have ONE person available to work/do things.
        /// </summary>
        public int Helper = 1;
        /// <summary>
        /// Our helper to get Money from our Inventory.
        /// Called Gold, BUT its just Cash/Money/Bucks
        /// </summary>
        public int Cash => itemDictionary[Items.Gold];
        /// <summary>
        /// We have Helpers, ourself, PLUS anyone looking for work.
        /// General labors to build, collect work.
        /// </summary>
        public int Helpers => Helper + thingDictionary[Things.JobSeekers];

        /// <summary>
        /// This reponds to UI changes to update, must be carefully controlled
        /// Can reverb on itself to create feedback loops.
        /// </summary>
        public System.Action OnChanged;

        /// <summary>
        /// Items, our tangiable items.
        /// </summary>
        [SerializeField] Dictionary<Items, int> itemDictionary = null;
        /// <summary>
        /// Things are soft resourecs, Humans, and various items.
        /// </summary>
        [SerializeField] Dictionary<Things, int> thingDictionary = null;

        /// <summary>
        /// ALL our Territory in 'Node's
        /// </summary>
        public List<HomeSquare> homeSquares = null;

        /// <summary>
        /// Once we have build on our Square, we keep track of it here.
        /// For weekly changes
        /// </summary>
        public List<HomeSquare> homeSquaresBuilt = null;

        /// <summary>
        /// Given our constraints and Sizing, we have 220 Squares of land per 'Character'
        /// Also considered to be Square == 'Node'
        /// </summary>
        public const int Squares = 220;

        /// <summary>
        /// We used Arrays to pull data, NOT meant to change data.
        /// </summary>
        public KeyValue[] ItemsArray
        {
            get
            {
                List<KeyValue> items = new List<KeyValue>();
                foreach (Items item in itemDictionary.Keys)
                {
                    KeyValue newValue = new KeyValue(item.ToString(), itemDictionary[item]);
                    items.Add(newValue);
                }
                return items.ToArray();
            }
        }

        /// <summary>
        /// We used Arrays to pull data, NOT meant to change data.
        /// </summary>
        public KeyValue[] ThingsArray
        {
            get
            {
                List<KeyValue> things = new List<KeyValue>();
                foreach (Things thing in thingDictionary.Keys)
                {
                    KeyValue newValue = new KeyValue(thing.ToString(), thingDictionary[thing]);
                    things.Add(newValue);
                }
                return things.ToArray();
            }
        }

        /// <summary>
        /// Given a simple name 'Micaiah' or some data
        /// WE create a generic 'Character'
        /// </summary>
        /// <param name="steaderName"></param>
        public HomeSteader(string steaderName)
        {
            this.Name = steaderName;
            ID = System.Guid.NewGuid().ToString();
            itemDictionary = new Dictionary<Items, int>();
            thingDictionary = new Dictionary<Things, int>();
            homeSquares = new List<HomeSquare>();
            homeSquaresBuilt = new List<HomeSquare>();
            for (int i = 0; i < Squares; i++)
            {
                Create(i);
            }

            foreach (Things thing in System.Enum.GetValues(typeof(Things)))
            {
                thingDictionary.Add(thing, 0);
            }

            foreach (Items item in System.Enum.GetValues(typeof(Items)))
            {
                itemDictionary.Add(item, 0);
                //thingDictionary.Add(item.ToString(), 0);
            }

            // Starting Resources
            // Without starting resources, people will starve, and can't do much.
            // Moves DO become a resource you could use, but VERY slowly.
            Change(Items.Gold, 1000);
            // Food is ABSOLUTELY Neccisary since it takes SOO long to make them (eventually)
            Change(Items.Bread, 10);

#if UNITY_EDITOR
            // Testing Resources - So we don't have to 'play' the game to test.
            Change(Items.Gold, 1000);
            Moves += 100;
            foreach (Items item in System.Enum.GetValues(typeof(Items)))
            {
                Change(item, 999);
            }
#endif

            CalculateValues();

            Week.OnWeekTicked += WeekTick;
            OnChanged?.Invoke();
        }

        /// <summary>
        /// We use this to generate our map 'unit'
        /// We have in Corners, that no one can use.
        /// We have an edge of the 'other' nearby 'unit's
        /// Finally we have our 'PLayer' map 'unit'
        /// </summary>
        /// <param name="i"></param>
        void Create(int i)
        {
            if (i == 0 || i == 19 || i == 200 || i == 219)
            {
                Generate(HomeStyle.Corner, i);
            }
            else if (i >= 1 && i <= 18)
            {
                Generate(HomeStyle.Other, i);
            }
            else if (i >= 201 && i <= 218)
            {
                Generate(HomeStyle.Other, i);
            }
            else if (i % 20 == 0)
            {
                Generate(HomeStyle.Other, i);
            }
            else if (i % 20 == 19)
            {
                Generate(HomeStyle.Other, i);
            }
            else
            {
                Generate(HomeStyle.Player, i, ID);
            }
        }

        void Generate(HomeStyle homeStyle, int i, string ownerID = "Game")
        {
            HomeSquare homeSquare = new HomeSquare(homeStyle, ownerID, i);
            homeSquares.Add(homeSquare);
        }

        /// <summary>
        /// Each week we tick and caclulate our taxes, people, and the things we gained.
        /// </summary>
        public void WeekTick()
        {
            PayPeople();
            HirePeople();
            MovePeople();
            MigratePeople();
            CalculateValues();
            EarnTaxes();

            //foreach (Items item in System.Enum.GetValues(typeof(Items)))
            //{
            //    thingDictionary[item.ToString()] = itemDictionary[item];
            //}

            OnChanged?.Invoke();
        }

        /// <summary>
        /// Based on the People and the People with Jobs
        /// you Pay taxes, the Helper(s) doesn't pay taxes.
        /// </summary>
        void EarnTaxes()
        {
            int taxes = 0;
            // Helpers are added and need to be removed to figure out taxes.
            int people = (thingDictionary[Things.People] - Helper);
            int jobs = thingDictionary[Things.Jobs];
            taxes += people;
            taxes += jobs;
            Change(Items.Gold, taxes);
            // This is showing the taxes per tick/week
            thingDictionary[Things.Taxes] = taxes;
        }

        /// <summary>
        /// First we determine what jobs need pay;
        /// This really works on the 4th wave.
        /// </summary>
        void PayPeople()
        {
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        int iWorkers = hs.PeopleCount - hs.JobMax;
                        //UnityEngine.Debug.Log("iWorkers: " + iWorkers);
                        if (hs.PeopleCount > 0 && hs.isResourceReady == false)
                        {
                            UnityEngine.Debug.Log("Worked Jobs: " + hs.PeopleCount);
                            for (int i = 0; i < hs.PeopleCount; i++)
                            {
                                if (CanAfford(Items.Gold, 1))
                                {
                                    UnityEngine.Debug.Log(hs.HomeResource + " Paid 1 Person");
                                    Change(Items.Gold, -1);
                                }
                                else
                                {
                                    UnityEngine.Debug.Log(hs.HomeResource + " failed to pay a person.");
                                    hs.MoveOut();
                                }
                            }
                        } // Else no employed Workers
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Second we determine what jobs people can find.
        /// This really works on the 3rd wave.
        /// </summary>
        void HirePeople()
        {
            int jobSeekers = thingDictionary[Things.JobSeekers];
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        if (CanAfford(Items.Gold, 1))
                        {
                            if (hs.WorkerSpace > 0 && jobSeekers > 0)
                            {
                                UnityEngine.Debug.Log(hs.HomeResource + " hired Worker.");
                                Change(Items.Gold, -1);
                                jobSeekers--;
                                hs.GainWorker();
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.Log(hs.HomeResource + " failed to pay a person.");
                            hs.MoveOut();
                        }
                        break;
                    }
                }
            }
            thingDictionary[Things.JobSeekers] = jobSeekers;
        }

        /// <summary>
        /// Third we check if people can move in, from the migrants.
        /// Migrants only happen on the second week (first they come) then they settle.
        /// </summary>
        void MovePeople()
        {
            int movers = 0;
            int migrants = thingDictionary[Things.Migrants];
            foreach (HomeSquare hs in homeSquares)
            {
                if (migrants > 0)
                {
                    switch (hs.HomeResource)
                    {
                        case HomeResources.House:
                        {
                            if (hs.HomeResource == HomeResources.House)
                            {
                                if (CanAfford(Items.Bread, 1))
                                {
                                    if (hs.Space > 0 && migrants > 0)
                                    {
                                        UnityEngine.Debug.Log(hs.HomeResource + " stopped being vacant.");
                                        Change(Items.Bread, -1);
                                        migrants--;
                                        hs.MoveIn();
                                    }
                                }
                                else
                                {
                                    UnityEngine.Debug.Log(hs.HomeResource + " failed to feed " + hs.PeopleCount + " people.");
                                    for (int i = 0; i < hs.PeopleCount; i++)
                                    {
                                        hs.MoveOut();
                                        movers++;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            thingDictionary[Things.Movers] = movers;
            thingDictionary[Things.Migrants] = migrants;
        }

        /// <summary>
        /// Four we check if their is space, and bread, we have people migrant in.
        /// Migrants only happen on the second week (first they come) then they settle.
        /// </summary>
        void MigratePeople()
        {
            int migrants = thingDictionary[Things.Migrants];
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.House:
                    {
                        if (hs.HomeResource == HomeResources.House)
                        {
                            for (int i = 0; i < hs.Space; i++)
                            {
                                if (CanAfford(Items.Bread, 1) && hs.MigrantSpace > 0)
                                {
                                    UnityEngine.Debug.Log(hs.HomeResource + " stopped being vacant.");
                                    migrants += 1;
                                    hs.MigrantCount += 1;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            thingDictionary[Things.Migrants] = migrants;
        }

        /// <summary>
        /// Figured out the values for the dictionaries to show.
        /// </summary>
        void CalculateValues()
        {
            int iPeople = 0;
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.House:
                    {
                        iPeople += hs.PeopleCount;
                        break;
                    }
                }
            }

            int iJobs = 0;
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        iJobs += hs.JobMax;
                        break;
                    }
                }
            }

            int workersAvailable = iPeople;
            foreach (HomeSquare hs in homeSquares)
            {
                switch (hs.HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        workersAvailable -= hs.PeopleCount;
                        break;
                    }
                }
            }

            //foreach (HomeSquare hs in homeSquaresBuilt)
            //{
            //    if (hs.JobCount < hs.PeopleCount && workersAvailable >= 1)
            //    {
            //        hs.JobCount++;
            //    }
            //}

            //int iHelpers = iPeople - iJobs;

            // Helper is our Arbitray person, and must be added in all displays

            //Moves += iHelpers + Helper;

            // Helper(s) are added to the people, but removed for taxes.
            thingDictionary[Things.People] = iPeople + Helper;
            thingDictionary[Things.Helpers] = Helpers;
            thingDictionary[Things.Jobs] = iJobs;
            thingDictionary[Things.JobSeekers] = workersAvailable;
        }

        /// <summary>
        /// We have moves we can do each turn, this spends them.
        /// </summary>
        /// <param name="iMove"></param>
        /// <returns></returns>
        public bool SpendMove(int iMove = 1)
        {
            if (Moves >= iMove)
            {
                Moves -= iMove;
                OnChanged?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds in an Item(type) based on its value.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="value"></param>
        public void Change(Items itemType, int value)
        {
            itemDictionary[itemType] += value;
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Given an item(type), can we afford its value.
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool CanAfford(Items itemType, int amount)
        {
            if (amount == 0)
            {
                return true;
            }
            else if (amount < 0)
            {
                return false;
            }
            return itemDictionary[itemType] >= amount;
        }

        /// <summary>
        /// Given a new built structure, can we afford its meta cost.
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public bool CanBuy(HomeResources house)
        {
            switch (house)
            {
                case HomeResources.Farm:
                {
                    if (CanAfford(Items.Gold, 100) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford Farm: Cash < 100");
                        return false;
                    }
                    if (CanAfford(Items.Wood, 100) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford Farm: Wood < 5");
                        return false;
                    }
                    return true;
                }
                case HomeResources.House:
                {
                    if (CanAfford(Items.Gold, 100) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford House: Cash < 100");
                        return false;
                    }
                    if (CanAfford(Items.Wood, 20) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford House: Wood < 20");
                        return false;
                    }
                    //if (itemDictionary[Items.Rock] < 10)
                    //{
                    //    UnityEngine.Debug.Log("Not Afford House: Rock < 10");
                    //    return false;
                    //}
                    return true;
                }
                case HomeResources.Bakery:
                {
                    if (CanAfford(Items.Gold, 100) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford Bakery: Cash < 100");
                        return false;
                    }
                    if (CanAfford(Items.Wood, 100) == false)
                    {
                        UnityEngine.Debug.Log("Not Afford Bakery: Wood < 20");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This is how we buy whatever we wnat from HomeResources
        /// First we check CanBuy, and check again here
        /// </summary>
        /// <param name="house"></param>
        public void Buy(HomeResources house)
        {
            if (CanBuy(house) == false)
            {
                return;
            }
            switch (house)
            {
                case HomeResources.Farm:
                {
                    Change(Items.Gold, -100);
                    Change(Items.Wood, -5);
                    break;
                }
                case HomeResources.House:
                {
                    Change(Items.Gold, -100);
                    Change(Items.Wood, -20);
                    // Upgrade House with Rocks.
                    //Change(Items.Rock, -10);
                    break;
                }
                case HomeResources.Bakery:
                {
                    Change(Items.Gold, -100);
                    Change(Items.Wood, -20);
                    // Upgrade Bakery with Rocks.
                    //Change(Items.Rock, -10);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds in additonal data on top of our node.
        /// House, Farm, ect..
        /// </summary>
        /// <param name="homeSquare"></param>
        public void ChangeSquare(HomeSquare homeSquare)
        {
            if (homeSquaresBuilt.Contains(homeSquare) == false)
            {
                homeSquaresBuilt.Add(homeSquare);
            }
        }
    }
}