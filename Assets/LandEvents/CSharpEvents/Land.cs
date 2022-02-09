//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//// https://www.youtube.com/watch?v=7OMrWvXNedw
//// Source: Craig Perko

//namespace LandEvents_CSharp
//{
//    public class Land : MonoBehaviour
//    {
//        public delegate void SharpLandEvent(Land land);
//        public static SharpLandEvent onClick;

//        public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();
//        //public static LandEvent onClick = new LandEvent();

//        public void Select()
//        {
//            Debug.Log(name + " was selected.");
//            Land.onClick?.Invoke(this);
//        }

//        public void Attack(Land land)
//        {
//            Debug.Log(name + " attacking " + land.name);
//            Land.onClick -= Attack;
//            commandStack.Remove(this);
//        }

//        public void ReadyAttack()
//        {
//            Land.onClick += Attack;
//            commandStack.Insert(0, this);
//            int count = Land.onClick.GetInvocationList().Length;
//            Debug.Log($"{count} listeners");
//            Debug.Log(name + " ready to attack.");
//        }
//    }
//}