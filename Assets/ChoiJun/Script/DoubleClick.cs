using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DoubleClick : MonoBehaviour, IPointerDownHandler 
{
    private PlayerInput playerInput;
    float interval = 0.3f;
    float doubleClickedTime = -1.0f;

    private void Start()
    {
        playerInput = GameManager.instance.player.GetComponent<PlayerInput>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f;
            playerInput.ExeDash();
        }
        else
        {
            doubleClickedTime = Time.time;
        }
    }
}