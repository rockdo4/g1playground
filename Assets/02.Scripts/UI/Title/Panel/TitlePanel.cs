using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePanel : PanelUi
{
    public CrewButton crewButton;
    public NewGameButton newGameButton;
    public ContinueButton continueButton;

    private void Awake()
    {
        crewButton = GetComponentInChildren<CrewButton>(true);
        newGameButton= GetComponentInChildren<NewGameButton>(true);
        continueButton= GetComponentInChildren<ContinueButton>(true);
    }
}
