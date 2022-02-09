//#define CSharp

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//// https://www.youtube.com/watch?v=7OMrWvXNedw
//// Source: Craig Perko

//namespace LandEvents_Unity
//{
//    public class LandEvent : UnityEvent<Land> { }

//    public class Land : MonoBehaviour
//    {
//        public static List<MonoBehaviour> commandStack = new List<MonoBehaviour>();
//        public static LandEvent onClick = new LandEvent();

//        public void Select()
//        {
//            Debug.Log(name + " was selected.");
//            Land.onClick?.Invoke(this);
//        }

//        public void Attack(Land land)
//        {
//            Debug.Log(name + " attacking " + land.name);
//            Land.onClick.RemoveListener(Attack);
//            commandStack.Remove(this);
//        }

//        public void ReadyAttack()
//        {
//            Land.onClick.AddListener(Attack);
//            commandStack.Insert(0, this);
//            Debug.Log(name + " ready to attack.");
//        }
//    }
//}