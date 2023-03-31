using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyntheticButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenSyntheticPopUp);
        if (UI.Instance.sceneName.Equals("Tutorial", StringComparison.OrdinalIgnoreCase) || UI.Instance.sceneName.Equals("Dungeon", StringComparison.OrdinalIgnoreCase))
        {
            ActivateButton(false);
        }
    }

    private void OpenSyntheticPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.syntheticPopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
