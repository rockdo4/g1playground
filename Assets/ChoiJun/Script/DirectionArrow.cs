using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DirectionArrow : MonoBehaviour, IPointerDownHandler
{
    public enum Direction
    {
        Left,
        Right,
    }
    private PlayerInput playerInput;
    private EventTrigger eventTrigger;
    public Direction direction;
    float interval = 0.3f;
    float doubleClickedTime = -1.0f;

    private void Start()
    {
        playerInput = GameManager.instance.player.GetComponent<PlayerInput>();
        eventTrigger = GetComponent<EventTrigger>();
        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        exitEntry.eventID = EventTriggerType.PointerExit;
        switch (direction)
        {
            case Direction.Left:
                enterEntry.callback.AddListener((data) => playerInput.MoveLeft(true));
                exitEntry.callback.AddListener((data) => playerInput.MoveLeft(false));
                break;
            case Direction.Right:
                enterEntry.callback.AddListener((data) => playerInput.MoveRight(true));
                exitEntry.callback.AddListener((data) => playerInput.MoveRight(false));
                break;
        }
        eventTrigger.triggers.Add(enterEntry);
        eventTrigger.triggers.Add(exitEntry);
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