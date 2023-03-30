using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : ButtonUi
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        button.onClick.AddListener(OpenSkillPopUp);
    }

    private void OpenSkillPopUp()
    {
        MenuClose();
        UI.Instance.popupPanel.AllClose();

        UI.Instance.popupPanel.skillPopUp.ActiveTrue();
        UI.Instance.popupPanel.menuSlider.ActiveTrue();
    }

    public override void ActivateButton(bool isinteractable)
    {
        base.ActivateButton(isinteractable);
    }

}
