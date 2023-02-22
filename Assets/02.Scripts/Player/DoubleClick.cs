using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerDownHandler 
{
    float interval = 0.3f;
    float doubleClickedTime = -1.0f;
    public Action DoubleClicked;

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f;
            GameManager.instance.playerController.Dash();
            Debug.Log("double click!");
        }
        else
        {
            doubleClickedTime = Time.time;
        }
    }
}