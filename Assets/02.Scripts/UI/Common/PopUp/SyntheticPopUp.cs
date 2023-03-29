using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyntheticPopUp : PopupUI
{
    protected override void OnDisable()
    {
        base.OnDisable();
        UI.Instance.popupPanel.menuSlider.ActiveFalse();
    }
}
