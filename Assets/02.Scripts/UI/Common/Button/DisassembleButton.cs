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
    }

    private void OpenDisassemblePopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.disassemblePopUp.ActiveTrue();
    }
}
