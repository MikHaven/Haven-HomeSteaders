using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://unity3d.com/learn/tutorials/topics/scripting/extension-methods

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class TransformExtensions
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Clears the child Gameobjects from the transform, destroying them.
    /// Clear without DestroyImmediate
    /// Destroy Immediate doesn't seem to call OnDestory
    /// </summary>
    /// <param name="trans"></param>
    public static void Clear(this Transform trans)
    {
        if (trans == null)
            return;
        while (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            child.SetParent(null, false);
            UnityEngine.Object.Destroy(child.gameObject);
            //Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Clears the child Gameobjects from the transform, destroying them Immediately
    /// This doesn't call OnDestory
    /// </summary>
    /// <param name="trans"></param>
    public static void ClearNow(this Transform trans)
    {
        if (trans == null)
            return;
        while (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            UnityEngine.Object.DestroyImmediate(child.gameObject);
            //Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Clear without DestroyImmediate
    /// Destroy Immediate doesn't seem to call OnDestory
    /// </summary>
    /// <param name="trans"></param>
    public static void Clear(this RectTransform trans)
    {
        if (trans == null)
            return;

        while (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            child.SetParent(null, false);
            UnityEngine.Object.Destroy(child.gameObject);
            //Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Clears RectTransform by DestroyImmediate
    /// This doesn't call OnDestory
    /// </summary>
    /// <param name="trans"></param>
    public static void ClearNow(this RectTransform trans)
    {
        if (trans == null)
            return;

        while (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            child.SetParent(null, false);
            UnityEngine.Object.DestroyImmediate(child.gameObject);
            //Destroy(child.gameObject);
        }
    }
}