using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLayout : Layout
{
    public PanelUi titlePanel;
    public PanelUi crewPanel;

    private void Awake()
    {
        titlePanel = GetComponentInChildren<TitlePanel>(true);
        crewPanel = GetComponentInChildren<CrewPanel>(true);
    }
}
