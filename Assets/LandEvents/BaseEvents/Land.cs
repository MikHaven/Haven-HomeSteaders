#define CSharp

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=7OMrWvXNedw
// Source: Craig Perko

namespace LandEvents_Base
{
    public class LandEvent : UnityEvent<Land> { }

    public class Land : MonoBehaviour
    {
#if CSharp
        public delegate void LandEvent(Land land);
        //public static SharpLandEvent onClick;
        public static System.Action<Land> onClick = null;
#else
        public static LandEvent onClick = new LandEvent();
#endif

        public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();
        //public static LandEvent onClick = new LandEvent();

        public void Select()
        {
            Debug.Log(name + " was selected.");
            Land.onClick?.Invoke(this);
        }

        public void Attack(Land land)
        {
            Debug.Log(name + " attacking " + land.name);
#if CSharp
            Land.onClick -= Attack;
#else
            Land.onClick.RemoveListener(Attack);
#endif
            commandStack.Remove(this);
        }

        public void ReadyAttack()
        {
#if CSharp
            Land.onClick += Attack;
            int count = Land.onClick.GetInvocationList().Length;
            Debug.Log($"{count} listeners");
#else
            Land.onClick.AddListener(Attack);
            Debug.Log($"Unknown listeners count");
#endif
            commandStack.Insert(0, this);
            Debug.Log(name + " ready to attack.");
        }
    }
}