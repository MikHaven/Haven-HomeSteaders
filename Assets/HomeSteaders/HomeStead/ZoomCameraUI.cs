using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera to ZOOM in on the UI
/// TODO: Add in WASD and Arrow keys to move around map.
/// </summary>
public class ZoomCameraUI : MonoBehaviour
{
    public RectTransform controlElement = null;

    public float zoomFactor = 1f; // 8.3f;
    public float zoomStep = 0.3f; // 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (zoomFactor <= 8.2f)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                zoomFactor += zoomStep;
                if (zoomFactor >= 8.3f)
                {
                    zoomFactor = 8.3f;
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            zoomFactor -= zoomStep;
            if (zoomFactor <= 2.25f) // 0.25f
            {
                zoomFactor = 2.25f;
            }
        }

        controlElement.localScale = new Vector3(zoomFactor, zoomFactor, zoomFactor);
    }
}