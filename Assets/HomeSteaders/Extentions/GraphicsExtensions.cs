using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GraphicsExtensions
{
    public static void SetAlpha(this UnityEngine.UI.Graphic graphic, float newAlpha)
    {
        Color color = graphic.color;
        color.a = newAlpha;
        graphic.color = color;
    }
}