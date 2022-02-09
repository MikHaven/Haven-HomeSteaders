using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HomeStead
{
    /// <summary>
    /// Data to reperent control of vaious map 'Units'
    /// Sorta like a way to contest various map pieces.
    /// Not much more was built around this concept.
    /// Player - Our 'Owned' 'Nodes'
    /// Other - Someone elses
    /// Contested - Not Implemented, but 'Node's we fight over
    /// Corner - 'No Mans Land' of data that is connecting 2-4 people's land.
    /// </summary>
    public enum HomeStyle
    {
        Player, Other, Contested, Corner
    }

    /// <summary>
    /// Our GO four our Square, one unit of Land
    /// Handles interaction, and data from our 'Character'
    /// </summary>
    public class HomeSquareGO : MonoBehaviour, IPointerClickHandler
    {
        public Image squareParent = null;
        public Image squareBase = null;
        public TMPro.TextMeshProUGUI textObject0 = null;
        public TMPro.TextMeshProUGUI textObject1 = null;

        /// <summary>
        /// Each 'Node' is a piece of Json data (Move to IDs and from Data classe)
        /// </summary>
        public HomeSquare homeSquare = null;
        /// <summary>
        /// Each 'Character' is a piece of GO with a Json data (Move to IDs and from Data classe)
        /// </summary>
        public HomeSteaderGO parentOwner = null;

        /// <summary>
        /// If we have clicked on a 'Node' It will change to a White Border, so you
        /// Can then decide to build, remove, whatever you want.
        /// </summary>
        public bool isSelected = false;

        /// <summary>
        /// How many People can make, and how much it cost to make bread
        /// </summary>
        public const int WheatBread = 4;
        /// <summary>
        /// How many People can Make, and how much Bakery earns from 'WheatBread' to make Bread
        /// </summary>
        public const int BreadWheat = 2;

        // https://answers.unity.com/questions/984280/input-axis-mouse-scrollwheel.html

        /// <summary>
        /// Once we are done, we link our data (Sometimes call Loaded)
        /// </summary>
        public void Generate()
        {
            homeSquare.OnChanged += UpdateUI;
            UpdateUI();
        }

        /// <summary>
        /// Whnever a change occurs, this is called to signify we need to update the UI
        /// </summary>
        void UpdateUI()
        {
            // Our selected adds its white border in the UI - Black if not selected (normally)
            if (isSelected)
            {
                squareParent.color = Color.white;
            }
            else
            {
                squareParent.color = Color.black;
            }
            this.textObject0.text = "";
            this.textObject1.text = "";
            switch (homeSquare.HomeStyle)
            {
                case HomeStyle.Player:
                {
                    switch (homeSquare.HomeResource)
                    {
                        case HomeResources.Forest:
                        {
                            squareBase.color = Color.green;
                            break;
                        }
                        case HomeResources.Rocks:
                        {
                            squareBase.color = Color.grey;
                            break;
                        }
                        case HomeResources.River:
                        {
                            squareBase.color = Color.blue;
                            break;
                        }
                        case HomeResources.Plain:
                        {
                            squareBase.color = Color.cyan;
                            break;
                        }
                        case HomeResources.Farm:
                        {
                            squareBase.color = Color.yellow;
                            break;
                        }
                        case HomeResources.House:
                        {
                            squareBase.color = Color.red;
                            break;
                        }
                        case HomeResources.Bakery:
                        {
                            squareBase.color = Color.magenta;
                            break;
                        }
                    }
                    //squareBase.color = new Color(33f, 33f, 33f);
                    textObject0.text = homeSquare.Coordinates;
                    textObject1.text = homeSquare.HomeResourceString;
                    break;
                }
                case HomeStyle.Other:
                {
                    squareBase.color = Color.grey;
                    //squareBase.color = new Color(56f, 16f, 66f);
                    break;
                }
                case HomeStyle.Contested:
                {
                    squareBase.color = Color.red;
                    //squareBase.color = new Color(215f, 15f, 15f);
                    break;
                }
                case HomeStyle.Corner:
                {
                    squareBase.color = Color.black;
                    //squareBase.color = new Color(0f, 0f, 0f);
                    break;
                }
            }
        }

        /// <summary>
        /// We check Unitys' Event System for clicked, SEE buttons/Events
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            //UnityEngine.Debug.Log("Pointer Clicked: " + eventData.button + " at " + homeSquare.Coordinates);

            if (Week.isUpdating)
            {
                UnityEngine.Debug.Log("Game is Updating..");
                return;
            }
            else
            {
                //UnityEngine.Debug.Log("Game is NOT updating.");
            }

            if (parentOwner != null)
            {
                parentOwner.ClickedSqaure();
            }
            else
            {
                UnityEngine.Debug.Log("ParentOwner == null");
            }

            isSelected = true;
            UpdateUI();
            //UnityEngine.Debug.Log("IsSelect == true");

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ClickedLeft();
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                ClickedMiddle();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ClickedRight();
            }
        }

        /// <summary>
        /// We can left click a 'Natural' Resources to clear it and give us Resources
        /// Turns it into a 'Plain' that we can build on.
        /// </summary>
        void ClickedLeft()
        {
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(homeSquare.OwnerID);
            if (homeSteader != null)
            {
                if (homeSteader.Moves <= 0)
                {
                    UnityEngine.Debug.Log("Left Clicked on: " + homeSquare.HomeResource + " but was out of moves.");
                    return;
                }
            }
            else
            {
                UnityEngine.Debug.Log("HomeSteader == null");
                return;
            }
            // We have moves left
            switch (homeSquare.HomeResource)
            {
                case HomeResources.Forest:
                {
                    UnityEngine.Debug.Log("Left Clicked on: " + homeSquare.HomeResource);
                    homeSteader.SpendMove();
                    homeSteader.Change(Items.Gold, 100);
                    homeSteader.Change(Items.Wood, 10);
                    homeSquare.RemoveForest();
                    UpdateUI();
                    break;
                }
                case HomeResources.Rocks:
                {
                    // We want to harvest Rocks
                    homeSteader.SpendMove();
                    homeSteader.Change(Items.Gold, 100);
                    homeSteader.Change(Items.Rock, 10);
                    homeSquare.RemoveRocks();
                    UpdateUI();
                    break;
                }
                case HomeResources.Plain:
                {
                    // What do you want to build?
                    BuildSelections.Clear();
                    BuildSelections.CreateBuild("House", "Cash: 100", "Wood: 20", () => Build(HomeResources.House));
                    BuildSelections.CreateBuild("Farm", "Cash: 100", "Wood: 5", () => Build(HomeResources.Farm));
                    BuildSelections.CreateBuild("Bakery", "Cash: 100", "Wood: 20", () => Build(HomeResources.Bakery));
                    UpdateUI();
                    break;
                }
                case HomeResources.Farm:
                {
                    // WheatSeed 1:1 WheatBread
                    // && CanAfford(WheatSeed, WheatBread);
                    if (homeSquare.isResourceReady)
                    {
                        // Do we collect Resources?
                        homeSteader.SpendMove(homeSquare.PeopleCount);
                        homeSteader.Change(Items.Wheat, homeSquare.PeopleCount * WheatBread);
                        homeSquare.WorkResource();
                        UpdateUI();
                    }
                    break;
                }
                case HomeResources.Bakery:
                {
                    if (homeSquare.isResourceReady && homeSteader.CanAfford(Items.Wheat, WheatBread))
                    {
                        UnityEngine.Debug.Log("Left Clicked: " + homeSquare.HomeResource);
                        // Do we collect Resources?
                        homeSteader.SpendMove(homeSquare.PeopleCount);
                        homeSteader.Change(Items.Bread, homeSquare.PeopleCount * BreadWheat);
                        homeSquare.WorkResource();
                        UpdateUI();
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Could not afford " + WheatBread + " Wheat");
                    }
                    break;
                }
                case HomeResources.House:
                {
                    // Do we encourage more people?
                    break;
                }
            }
        }

        /// <summary>
        /// Stub for future changes to the UI via Middle Clicking.
        /// </summary>
        public void ClickedMiddle()
        {
            UnityEngine.Debug.Log("Middle Clicked on: " + homeSquare.HomeResource);
            //switch (homeSquare.HomeResource)
            //{
            //    case HomeResources.Plain:
            //    {
            //        //Build(HomeResources.House);
            //        break;
            //    }
            //}
        }

        /// <summary>
        /// Stub for Future changes to our 'Node'
        /// </summary>
        public void ClickedRight()
        {
            UnityEngine.Debug.Log("Right Clicked on: " + homeSquare.HomeResource);
            //switch (homeSquare.HomeResource)
            //{
            //    case HomeResources.Plain:
            //    {
            //        Build(HomeResources.Farm);
            //        break;
            //    }
            //}
        }

        /// <summary>
        /// Given we have clicked on a 'Building' to place in our 'Node' this accomplishes that.
        /// First line of defense for catching if we can afford, have the resources required.
        /// </summary>
        /// <param name="homeResource"></param>
        void Build(HomeResources homeResource)
        {
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(homeSquare.OwnerID);
            if (homeSteader == null)
            {
                UnityEngine.Debug.Log("HomeSteader == null");
                return;
            }
            if (homeSteader.Moves <= 0)
            {
                UnityEngine.Debug.Log("HomeStead out of moves..");
                return;
            }
            switch (homeResource)
            {
                case HomeResources.Farm:
                case HomeResources.Bakery:
                {
                    if (homeSteader.CanBuy(homeResource))
                    {
                        homeSteader.Buy(homeResource);
                        homeSquare.Build(homeResource);
                        homeSteader.ChangeSquare(homeSquare);
                        UpdateUI();
                    }
                    break;
                }
                case HomeResources.House:
                {
                    if (homeSteader.CanBuy(HomeResources.House))
                    {
                        homeSteader.Buy(HomeResources.House);
                        homeSquare.Build(HomeResources.House);
                        homeSteader.ChangeSquare(homeSquare);
                        UpdateUI();
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Old code for detected when we enter or leave (Future ideas)
        /// </summary>
        void OnMouseEnter() { }

        /// <summary>
        /// Old code for detected when we enter or leave (Future ideas)
        /// </summary>
        void OnMouseExit() { }
    }
}