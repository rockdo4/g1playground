using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterProfile : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenProfilePopUp);
    }

    private void OpenProfilePopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();
        UI.Instance.popupPanel.profilePopUp.ActiveTrue();
    }
}
