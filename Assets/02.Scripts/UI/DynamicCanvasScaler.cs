using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCanvasScaler : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }
    private void Start()
    {
        SetCanvasScale();
    }

    private void SetCanvasScale()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        bool isTablet = (SystemInfo.deviceModel.Contains("iPad") || SystemInfo.deviceModel.Contains("Tablet"));

        if (isTablet)
        {
            canvasScaler.matchWidthOrHeight = 0.5f;
        }
        else
            canvasScaler.matchWidthOrHeight = 1f;
    }
}
