#define CSharp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=7OMrWvXNedw
// Source: Craig Perko

namespace LandEvents_Base
{
    public class Map : MonoBehaviour
    {
        public int LandCount = 90;
        public RectTransform contentPanel = null;
        public GameObject landPrefab = null;

        public List<string> CommandStacks;

        // Start is called before the first frame update
        void Start()
        {
            CommandStacks = new List<string>();
            for (int i = 0; i <= LandCount; i++)
            {
                GameObject go = GameObject.Instantiate(landPrefab);
                go.transform.SetParent(contentPanel);
                Land land = go.GetComponent<Land>();
                go.name = "Land " + i;
            }
            Land.commandStack = new List<MonoBehaviour>();
            Land.commandStack.Insert(0, this);
#if CSharp
            Land.onClick += LandClicked;
#else
            Land.onClick.RemoveAllListeners();
            Land.onClick.AddListener(LandClicked);
#endif
        }

        void Update()
        {
            CommandStacks = new List<string>();
            foreach(var command in Land.commandStack)
            {
                CommandStacks.Add(command.name);
            }
        }

        //public void LandClicked()
        //{
        //    Debug.Log("Some land was clicked.");
        //}

        public void LandClicked(Land land)
        {
            //if (land.commandStack[0] == this)
            if (Land.commandStack[0] == this)
            {
                // We are top of the stack.
                Debug.Log(land.name + " was clicked.");
                land.ReadyAttack();
                //Land.onClick.RemoveListener(LandClicked);
                //Land.onClick.RemoveListener(land.Attack);
            }
            else
            {
                Debug.Log(name + " not in commmand.");
                Debug.Log(Land.commandStack[0].name + " in control.");
            }
        }
    }
}