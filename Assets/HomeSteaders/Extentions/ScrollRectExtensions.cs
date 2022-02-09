using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// https://forum.unity.com/threads/scroll-to-the-bottom-of-a-scrollrect-in-code.310919/ 
/// </summary>
public static class ScrollRectExtensions
{
    public static void ScrollToTop(this ScrollRect scrollRect)
    {
        if (scrollRect != null)
        {
            scrollRect.normalizedPosition = new Vector2(0, 1);
        }
    }
    public static void ScrollToBottom(this ScrollRect scrollRect)
    {
        if (scrollRect != null)
        {
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
    public static void ScrollToItem(this ScrollRect scrollRect, float i)
    {
        if (scrollRect == null)
        {
            return;
        }
        float fVerse = i - 1;
        if (fVerse <= 0)
        {
            fVerse = 0;
        }
        //float normalizePosition = (float)scrollRect.transform.GetSiblingIndex() / (float)scrollRect.content.transform.childCount;
        float normalizePosition = fVerse / (float)scrollRect.content.transform.childCount;
        scrollRect.verticalNormalizedPosition = 1 - normalizePosition;
    }
}