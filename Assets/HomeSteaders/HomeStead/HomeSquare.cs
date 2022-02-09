using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Gives us various Map 'Node's.  One tile of Map Data.
    /// Forest - Woods (Wood/Lumber)
    /// Rocks - Stone (Building Material)
    /// River - Water (More like a small river through a plain.)
    /// Farm - Tilled soil - NOT a house
    /// House - Multiple family dweeling upgrading for more People
    /// Bakery - Farms make Wheat, Bakery turns them into Bread.
    /// </summary>
    public enum HomeResources
    {
        Forest, Rocks, River, Plain, Farm, House, Bakery
    }

    /// <summary>
    /// One 'Node' of our map data.  An item that holds various data.
    /// Depending on what we build.
    /// </summary>
    [System.Serializable]
    public class HomeSquare
    {
        public string Name = "";
        public string OwnerID = "None";
        public HomeResources HomeResource = HomeResources.Forest;
        public HomeStyle HomeStyle = HomeStyle.Player;

        public int K = 0;
        public int X = 0;
        public int Y = 0;

        public string Coordinates => X + ", " + Y;

        public bool isResourceReady = false;
        public int WeekToReady = 0;

        public int MigrantCount = 0;
        public int PeopleCount = 0;
        public int JobMax = 0;
        public int PeopleMax = 0;
        public int Space => PeopleMax - PeopleCount;
        public int WorkerSpace => JobMax - PeopleCount;
        public int MigrantSpace => PeopleMax - MigrantCount - PeopleCount;
        public string WorkString => PeopleCount + "/" + JobMax;
        public string HomeString => PeopleCount + "/" + PeopleMax;

        public System.Action OnChanged = null;

        bool isDev = true;

        /// <summary>
        /// When we clear a 'Natural' Resource this type is used to give us a generic.
        /// </summary>
        public const HomeResources ClearedType = HomeResources.Plain;

        public string HomeResourceString
        {
            get
            {
                switch (HomeResource)
                {
                    case HomeResources.House:
                    {
                        if (Space > 0)
                        {
                            return "Vacant" + " " + HomeString;
                        }
                        else
                        {
                            return HomeResources.House + " " + HomeString;
                        }
                    }
                    case HomeResources.Farm:
                    {
                        string sReturn = HomeResources.Farm + "-" + WorkString;
                        if (isResourceReady == false && PeopleCount >= 1)
                        {
                            sReturn = "Field";
                            if (isDev)
                                sReturn += "\n" + WeekToReady + "-" + WorkString;
                        }
                        return sReturn;
                    }
                    case HomeResources.Bakery:
                    {
                        string sReturn = HomeResources.Bakery + "-" + WorkString;
                        if (isResourceReady == false && PeopleCount >= 1)
                        {
                            sReturn = "Baking";
                            if (isDev)
                                sReturn += "\n" + WeekToReady + "-" + WorkString;
                        }
                        return sReturn;
                    }
                }
                return HomeResource.ToString();
                //return HomeResource.ToString() + "-" + PeopleCount;
            }
        }

        /// <summary>
        /// Given our basic 'Contested' State, we own this Square of 'Node'
        /// We than run some generic randomness to give our map texture.
        /// </summary>
        /// <param name="homeStyle"></param>
        /// <param name="ownerID"></param>
        /// <param name="k"></param>
        public HomeSquare(HomeStyle homeStyle, string ownerID, int k)
        {
            this.HomeStyle = homeStyle;

            this.K = k;
            int x = K % 20;
            int y = Mathf.RoundToInt(K / 20f);
            X = x - 1;
            Y = y - 1;

            int random = Random.Range(0, 100);
            if (random <= 10)
            {
                HomeResource = HomeResources.River;
            }
            else if (random <= 35)
            {
                HomeResource = HomeResources.Forest;
            }
            else if (random <= 55)
            {
                HomeResource = HomeResources.Rocks;
            }
            else if (random <= 75)
            {
                HomeResource = HomeResources.Plain;
            }
            else
            {
                HomeResource = HomeResources.Rocks;
            }
            OwnerID = ownerID;

            Name = K + " " + HomeResource + " " + X + ", " + Y;

            Week.OnWeekTicked += SquareWeekTick;
        }

        /// <summary>
        /// Each 'Node' of data, ticks and gives us resources/taxes based on the occupants.
        /// </summary>
        void SquareWeekTick()
        {
            // WorkerSpace must be equal to 0
            // Basically are we fully employed.
            if (WorkerSpace <= 0)
            {
                switch (HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        WeekToReady--;
                        if (WeekToReady <= 0)
                        {
                            isResourceReady = true;
                        }
                        OnChanged?.Invoke();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Once we cilck on something to 'Clear' out the 'Node'
        /// This function gives us its 'Natural' Resources, Water, Wood
        /// Destroyin the enviroment in the process.  Making a cleared 'Plain'
        /// </summary>
        /// <param name="homeResource"></param>
        public void Build(HomeResources homeResource)
        {
            if (HomeResource == HomeResources.Plain)
            {
                HomeResource = homeResource;
                PeopleMax = 1;
                switch (HomeResource)
                {
                    case HomeResources.Farm:
                    case HomeResources.Bakery:
                    {
                        JobMax = 1;
                        break;
                    }
                }
                //MoveIn();
                WorkResource();
                OnChanged?.Invoke();
            }
        }

        public void Upgrade()
        {
            PeopleMax += 1;
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Once we clear, work a 'Node'
        /// We set its delay to do something to 2.
        /// </summary>
        public void WorkResource()
        {
            WeekToReady = 2;
            isResourceReady = false;
            OnChanged?.Invoke();
        }

        /// <summary>
        /// People will move out, if they are unhappy.
        /// </summary>
        public void MoveOut()
        {
            PeopleCount -= 1;
            if (PeopleCount < 0)
            {
                PeopleCount = 0;
            }
            OnChanged?.Invoke();
        }


        /// <summary>
        /// People will move in, based on food/taxes.
        /// This is not 'coming' from Anywhere, creates out of thin air.
        /// </summary>
        public void MoveIn()
        {
            PeopleCount += 1;
            if (PeopleCount >= PeopleMax)
            {
                PeopleCount = PeopleMax;
            }
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Public function to Gain a Worker
        /// </summary>
        public void GainWorker()
        {
            MoveIn();
        }

        /// <summary>
        /// We lose people based on Houses/Food
        /// </summary>
        public void LossWorker()
        {
            MoveOut();
        }

        /// <summary>
        /// If we have a 'Natural' resource we remove it, change its based type
        /// and Gives us the resources, but here we just set the data change.
        /// </summary>
        public void RemoveForest()
        {
            if (HomeResource == HomeResources.Forest)
            {
                HomeResource = ClearedType;
                OnChanged?.Invoke();
            }
        }

        /// <summary>
        /// If we have a 'Natural' resource we remove it, change its based type
        /// and Gives us the resources, but here we just set the data change.
        /// </summary>
        public void RemoveRocks()
        {
            if (HomeResource == HomeResources.Rocks)
            {
                HomeResource = ClearedType;
                OnChanged?.Invoke();
            }
        }
    }
}