using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenExitPopUp);
    }

    private void OpenExitPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.exitPopUp.ActiveTrue();
    }
}
