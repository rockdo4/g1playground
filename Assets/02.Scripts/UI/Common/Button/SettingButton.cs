using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenSettingPopUp);
    }

    private void OpenSettingPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.settingPopUp.ActiveTrue();
    }
}
