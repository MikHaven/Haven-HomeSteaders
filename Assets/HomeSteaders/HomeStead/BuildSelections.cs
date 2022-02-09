using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HomeStead
{
    /// <summary>
    /// Class to manage building on our 'Node' of map square.
    /// </summary>
    public class BuildSelections : MonoBehaviour
    {
        public static BuildSelections instance = null;
        void Awake()
        {
            instance = this;
            Clear();
        }

        /// <summary>
        /// Where our UI sits, its content, is cleared and rebuilt sometimes.
        /// </summary>
        public RectTransform contentPanel;

        /// <summary>
        /// Clears our Content of all previous data, in preperation for new UI data.
        /// </summary>
        public static void Clear()
        {
            instance.contentPanel.Clear();
        }

        /// <summary>
        /// Given various inputs, we build or select the object we want to place.
        /// Click on a 'Node' then click on a 'Building'
        /// </summary>
        /// <param name="nameText"></param>
        /// <param name="cost0Value"></param>
        /// <param name="cost1Value"></param>
        /// <param name="onClicked"></param>
        public static void CreateBuild(string nameText, string cost0Value, string cost1Value, System.Action onClicked)
        {
            GameObject go = Resources.CreateBuild(instance.contentPanel);
            BuildSelectionGO buildGO = go.GetComponent<BuildSelectionGO>();
            buildGO.nameObject.text = nameText;
            buildGO.cost0Object.text = cost0Value;
            buildGO.cost1Object.text = cost1Value;

            buildGO.GetComponent<Button>().onClick.AddListener(() => onClicked.Invoke());
            buildGO.GetComponent<Button>().onClick.AddListener(() => BuildSelections.Clear());
        }
    }
}