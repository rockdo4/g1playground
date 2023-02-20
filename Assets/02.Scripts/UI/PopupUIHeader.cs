using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupUIHeader : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform parentRect;

    private Vector2 rectBegin;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    private void Awake()
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        rectBegin = parentRect.anchoredPosition;
        moveBegin = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        moveOffset = eventData.position - moveBegin;
        parentRect.anchoredPosition = rectBegin + moveOffset;
    }
}
