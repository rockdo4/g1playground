using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblingResult10PopUp : PopupUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        UI.Instance.popupPanel.menuSlider.ActiveFalse();
    }

    protected override void OnDisable()
    {
        //Dont Clear
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
