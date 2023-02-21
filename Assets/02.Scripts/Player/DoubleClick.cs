using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerDownHandler 
{
    float interval = 0.3f;
    float doubleClickedTime = -1.0f;
    bool isDoubleClicked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            isDoubleClicked = true;
            doubleClickedTime = -1.0f;
            GameManager.Instance.player.IsDash = true;
            Debug.Log("double click!");
        }
        else
        {
            isDoubleClicked = false;
            doubleClickedTime = Time.time;
        }
    }
}