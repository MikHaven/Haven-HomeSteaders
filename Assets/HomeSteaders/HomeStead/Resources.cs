using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Editor class to hold Resources, so we can reference a single object.
    /// Singular for now, but as we expand, could include more, or broken down
    /// Into Sprites/Prefabs and so forth.
    /// </summary>
    public class Resources : MonoBehaviour
    {
        public static Resources instance = null;
        void Awake()
        {
            instance = this;
        }

        public GameObject buildSelectionPrefab = null;
        public static GameObject BuildSelectionPrefab => instance.buildSelectionPrefab;

        public static GameObject CreateBuild(RectTransform content)
        {
            GameObject go = GameObject.Instantiate(BuildSelectionPrefab);
            go.transform.SetParent(content);
            return go;
        }
    }
}