using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenEquipPopUp);
    }

    private void OpenEquipPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.equipPopUp.ActiveTrue();
    }
}
