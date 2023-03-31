using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisassembleButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenDisassemblePopUp);
        if (UI.Instance.sceneName.Equals("Tutorial", StringComparison.OrdinalIgnoreCase) || UI.Instance.sceneName.Equals("Dungeon", StringComparison.OrdinalIgnoreCase))
        {
            ActivateButton(false);
        }
    }

    private void OpenDisassemblePopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.disassemblePopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
