using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenBookPopUp);
    }

    private void OpenBookPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.bookPopUp.ActiveTrue();
    }
}
