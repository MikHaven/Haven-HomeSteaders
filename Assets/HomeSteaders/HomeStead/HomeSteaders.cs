using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Master Data class to keep track of all our HomeSteamers
    /// Will expand to save/load data, and build Game Elements from loaded/played data.
    /// </summary>
    public class HomeSteaders : MonoBehaviour
    {
        public static HomeSteaders instance = null;
        void Awake()
        {
            instance = this;
            Steaders = new List<HomeSteader>();
        }

        public List<HomeSteader> Steaders = null;

        public TMPro.TextMeshProUGUI characterDisplay;
        public TMPro.TextMeshProUGUI itemDisplay;
        public TMPro.TextMeshProUGUI thingDisplay;

        public string currentHomeSteaderID;

        public KeyValue[] keyValueItemsArray = null;
        public KeyValue[] keyValueThingArray = null;

        public static string CreateHomeSteader(string steaderName)
        {
            HomeSteader homeSteader = new HomeSteader(steaderName);
            instance.Steaders.Add(homeSteader);
            return homeSteader.ID;
        }

        public static string GetHomeSteaderID(string ownerName)
        {
            foreach (HomeSteader hs in instance.Steaders)
            {
                if (hs.Name == ownerName)
                {
                    return hs.ID;
                }
            }
            return null;
        }

        public static HomeSteader GetHomeSteader(string ownerID)
        {
            foreach (HomeSteader hs in instance.Steaders)
            {
                if (hs.ID == ownerID)
                {
                    return hs;
                }
            }
            return null;
        }

        public void OnSelected(string steaderID)
        {
            currentHomeSteaderID = steaderID;
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(currentHomeSteaderID);
            if (homeSteader != null)
            {
                homeSteader.OnChanged += OnSteaderChanged;
                characterDisplay.text = homeSteader.Name + " $" + homeSteader.Cash;
                homeSteader.OnChanged?.Invoke();
            }
        }

        void OnSteaderChanged()
        {
            HomeSteader homeSteader = HomeSteaders.GetHomeSteader(currentHomeSteaderID);
            if (homeSteader != null)
            {
                characterDisplay.text = homeSteader.Name + " $" + homeSteader.Cash + " Moves: " + homeSteader.Moves;
                string itemReadout = "";
                int i = 0;
                keyValueItemsArray = homeSteader.ItemsArray;
                foreach (KeyValue value in keyValueItemsArray)
                {
                    itemReadout += value.Text + ": " + value.Value;
                    itemReadout += "\n";
                    i++;
                }
                itemDisplay.text = itemReadout;

                int j = 0;
                string thingReadout = "";
                keyValueThingArray = homeSteader.ThingsArray;
                foreach (KeyValue value in keyValueThingArray)
                {
                    thingReadout += value.Text + ": " + value.Value;
                    thingReadout += "\n";
                    j++;
                }
                thingDisplay.text = thingReadout;
            }
        }
    }
}