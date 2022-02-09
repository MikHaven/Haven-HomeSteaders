using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Our HomerSteader GO, which keeps track of our data for our "Character"
    /// </summary>
    public class HomeSteaderGO : MonoBehaviour
    {
        public GameObject squarePrefab = null;

        public string HomerSteader = "Micaiah";
        public string homeSteaderID;

        public List<HomeSquareGO> allSquares = null;

        // Start is called before the first frame update
        void Start()
        {
            // This is a Prototype way to Create Data, based on our Scene
            // Future will have to create/load all the data, then ask our 
            // Game/User what data they want to load, and play with.
            // Micaiah Logs in, pulls the data, and game loads it all so he can have a play Session.
            allSquares = new List<HomeSquareGO>();
            homeSteaderID = HomeSteaders.CreateHomeSteader(HomerSteader);
            GenerateSquares();
            HomeSteaders.instance.OnSelected(homeSteaderID);
        }

        /// <summary>
        /// We link our Json data in our 'Character' to our UI data
        /// </summary>
        void GenerateSquares()
        {
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(homeSteaderID);
            foreach(HomeSquare square in homeSteader.homeSquares)
            {
                CreateSquare(square.K);
            }
        }

        /// <summary>
        /// We respond to the clicked on something, but turn it off for all others.
        /// </summary>
        public void ClickedSqaure()
        {
            BuildSelections.Clear();
            foreach(HomeSquareGO homeGO in allSquares)
            {
                homeGO.isSelected = false;
                homeGO.homeSquare.OnChanged.Invoke();
            }
        }

        /// <summary>
        /// Given our Index value, we put data from our 'Character'
        /// </summary>
        /// <param name="i"></param>
        void CreateSquare(int i)
        {
            GameObject go = GameObject.Instantiate(squarePrefab, HomeSteadPanel.CurrentPanel);
            go.transform.position = Vector3.zero;
            go.transform.localScale = Vector3.one;

            HomeSquareGO homeGO = go.GetComponent<HomeSquareGO>();
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(homeSteaderID);
            homeGO.homeSquare = homeSteader.homeSquares[i];
            homeGO.Generate();
            homeGO.parentOwner = this;
            allSquares.Add(homeGO);
        }
    }
}