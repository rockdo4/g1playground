using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenMapPopUp);
    }

    private void OpenMapPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.worldMapPopUp.ActiveTrue();
    }
}
