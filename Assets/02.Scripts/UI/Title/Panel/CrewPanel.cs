using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewPanel : PanelUi
{
    public void ClickPanel()
    {
        GameManager.instance.ui.title.titlePanel.ActiveTrue();
        GameManager.instance.ui.title.crewPanel.ActiveFalse();
    }
}
