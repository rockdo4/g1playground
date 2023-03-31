using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUi : MonoBehaviour
{
    public Button button;

    protected virtual void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void MenuClose()
    {
        UI.Instance.popupPanel.menuPopUp.ActiveFalse();
    }

    public void PopUpClose()
    {
    }

    public virtual void ActivateButton(bool isinteractable)
    {
        button.interactable = isinteractable;
    }
}
