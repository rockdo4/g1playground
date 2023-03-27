using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewButton : ButtonUi
{

    public override void ClickButton()
    {
        GameManager.instance.ui.title.titlePanel.ActiveFalse();
        GameManager.instance.ui.title.crewPanel.ActiveTrue();
    }
}
