using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundPanel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        UI.Instance.popupPanel.menuPopUp.GetComponent<UIPopupAnimator>().MenuPopUpExit();

    }
}
