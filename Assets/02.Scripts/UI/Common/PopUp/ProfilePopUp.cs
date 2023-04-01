using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePopUp : PopupUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        UI.Instance.popupPanel.menuSlider.ActiveFalse();
    }

}