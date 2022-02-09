using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Class to store data for use later.
    /// Used for Inventory and String/Int combo
    /// Used in Dictionarys for ItemTypes
    /// Links Name with Values
    /// </summary>
    [System.Serializable]
    public class KeyValue
    {
        public string Text = "";
        public int Value = 0;
        public KeyValue(string sText, int iValue)
        {
            this.Text = sText;
            this.Value = iValue;
        }
    }
}