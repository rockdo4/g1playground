using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PopupUI : MonoBehaviour, IPointerDownHandler
{
    public Button _closeButton;
    public event Action OnFocus;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (OnFocus != null)
            OnFocus();
    }
}
