using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhanceButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenReinforcePopUp);
        if (UI.Instance.sceneName.Equals("Tutorial", StringComparison.OrdinalIgnoreCase) || UI.Instance.sceneName.Equals("Dungeon", StringComparison.OrdinalIgnoreCase))
        {
            ActivateButton(false);
        }
    }

    private void OpenReinforcePopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.reinforcePopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
