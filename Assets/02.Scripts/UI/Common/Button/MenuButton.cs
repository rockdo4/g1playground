using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void OpenMenuPopUp()
    {
        UI.Instance.popupPanel.menuPopUp.ActiveTrue();
    }
}
