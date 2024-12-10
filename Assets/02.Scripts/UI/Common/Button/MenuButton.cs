using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : ButtonUi
{
    public MenuPopUp menu;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OpenCloseMenuPopUp()
    {
        if(menu.isOpen)
        {
            UI.Instance.popupPanel.menuPopUp.GetComponent<UIPopupAnimator>().MenuPopUpExit();
        }
        else
            UI.Instance.popupPanel.menuPopUp.ActiveTrue();
    }
}
