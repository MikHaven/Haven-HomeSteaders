using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Keeps track of the UI we are currently using.
    /// Turns off the UI, was meant to toggle between TWO UI
    /// But came up with a work around.
    /// Not used in Scaling, more like 150 vs 75 size UI elements.
    /// </summary>
    public class HomeSteadPanel : MonoBehaviour
    {
        public static HomeSteadPanel instance = null;
        void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Our Full panel view
        /// Full Size UI, vs scaling, this was meant as a Large/Small sizes
        /// </summary>
        public RectTransform fullPanel;
        /// <summary>
        /// Was meant as a smaller version of our Game View.
        /// More strategic view or something.
        /// </summary>
        //public RectTransform startPanel;

        /// <summary>
        /// Used to get the game Panel our Game is played in.
        /// </summary>
        public static RectTransform CurrentPanel => GetCurrentPanel();

        /// <summary>
        /// Was meant to toggle between two modes, but stuck with Full Mode for now.
        /// </summary>
        public bool isFull = true;

        //static RectTransform GetCurrentPanel() => (instance.isFull) ? instance.fullPanel : instance.startPanel;
        static RectTransform GetCurrentPanel() => instance.fullPanel;

        void Update()
        {
            // Was meant as a Toggle, but just turns off our UI as is the only type of UI
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isFull = !isFull;
            }
            // Toggles between our Current (only) UI and a theoretically tutorial/test ui
            fullPanel.gameObject.SetActive(isFull);
            //startPanel.gameObject.SetActive(isFull == false);
        }
    }
}