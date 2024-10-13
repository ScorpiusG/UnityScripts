using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Makes the canvas automatically adjust to match the screen width or height based on ratio of current resolution versus the referenced one.
/// </summary>
public class CanvasAspectRatioAutoMatch : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    private void Start()
    {
        Adapt();
    }

    public void Adapt()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }
        if (canvasScaler == null) return;

        float screenRatioCurrent = (1f * Screen.width) / (1f * Screen.height);
        float screenRatioPreferred = (1f * canvasScaler.referenceResolution.x) / (1f * canvasScaler.referenceResolution.y);

        // Wider = Fixed height
        if (screenRatioCurrent > screenRatioPreferred + 0.0001f)
        {
            canvasScaler.matchWidthOrHeight = 1f;
        }
        // Taller = Fixed width
        else if (screenRatioCurrent < screenRatioPreferred - 0.0001f)
        {
            canvasScaler.matchWidthOrHeight = 0f;
        }
    }
}
