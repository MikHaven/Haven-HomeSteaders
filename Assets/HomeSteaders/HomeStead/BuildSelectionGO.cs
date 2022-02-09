using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomeStead
{
    /// <summary>
    /// UI linked data to decide what to build on our map 'unit' Square.
    /// </summary>
    public class BuildSelectionGO : MonoBehaviour
    {
        /// <summary>
        /// Our Title
        /// </summary>
        public TMPro.TextMeshProUGUI nameObject = null;
        /// <summary>
        /// Whatever data we have to display in Array 0
        /// </summary>
        public TMPro.TextMeshProUGUI cost0Object = null;
        /// <summary>
        /// Whatever data we have to display in Array 1
        /// </summary>
        public TMPro.TextMeshProUGUI cost1Object = null;

        // Start is called before the first frame update
        void Awake()
        {
            // We always remove all Listeners, as we are CODE based Button management.
            Button button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            // Adds in the Generic, so we can 'see' what we clicked.
            button.onClick.AddListener(() => ClickedBuild());
            // Additional functionality is added in more Listeners, throughout the game.
        }

        /// <summary>
        /// This is part of the Button based
        /// Does not get overwritten so far.
        /// Gives us a headsup we clicked.
        /// Additonal functionality is added on top of this through Listeners
        /// </summary>
        void ClickedBuild()
        {
            UnityEngine.Debug.Log("Clicked on: " + nameObject.text);
        }
    }
}