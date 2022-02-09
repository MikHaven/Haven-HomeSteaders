using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeStead
{
    /// <summary>
    /// Sizes our UI to fit our children's width.
    /// Used in Scroll Views/Content to keep data scrollable and not off screen.
    /// </summary>
    public class UIContentChildWidth : MonoBehaviour
    {
        public RectTransform childContent = null;
        public float EditorWidthMax = 900;
        public float EditorWidthMin = 150;

        // Update is called once per frame
        void Update()
        {
            if (childContent.sizeDelta.x != EditorWidthMax)
            {
                RectTransform rt = GetComponent<RectTransform>();
                if (rt != null)
                {
                    Vector2 size = rt.sizeDelta;
                    if (childContent.sizeDelta.x >= EditorWidthMax)
                    {
                        size.x = EditorWidthMax;
                    }
                    else if (childContent.sizeDelta.x <= EditorWidthMin)
                    {
                        if (childContent.childCount <= 0)
                        {
                            size.x = 0f;
                        }
                        else
                        {
                            size.x = EditorWidthMin;
                        }
                    }
                    else
                    {
                        size.x = childContent.sizeDelta.x;
                    }
                    rt.sizeDelta = size;
                }
            }
        }
    }
}