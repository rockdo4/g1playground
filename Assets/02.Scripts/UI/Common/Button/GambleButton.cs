using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenGamblingPopUp);
        if (UI.Instance.sceneName.Equals("Tutorial", StringComparison.OrdinalIgnoreCase) || UI.Instance.sceneName.Equals("Dungeon", StringComparison.OrdinalIgnoreCase))
        {
            ActivateButton(false);
        }
    }

    private void OpenGamblingPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.gamblingPopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
