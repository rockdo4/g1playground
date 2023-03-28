using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewPanel : PanelUi
{
    public void ClickPanel()
    {
        UI.Instance.title.titlePanel.ActiveTrue();
        UI.Instance.title.crewPanel.ActiveFalse();
    }
}
