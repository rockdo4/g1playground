using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewButton : ButtonUi
{

    public override void ClickButton()
    {
        UI.Instance.title.titlePanel.ActiveFalse();
        UI.Instance.title.crewPanel.ActiveTrue();
    }
}
