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
    }

    private void OpenReinforcePopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.reinforcePopUp.ActiveTrue();
    }
}
