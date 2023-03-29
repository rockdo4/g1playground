using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenItemPopUp);
    }

    private void OpenItemPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.itemPopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }
}
