using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : PanelUi
{
    public MenuButton menuButton;
    public HomeButton homeButton;
    public RestartButton restartButton;

    private void Awake()
    {
        menuButton = GetComponentInChildren<MenuButton>(true);
        homeButton = GetComponentInChildren<HomeButton>(true);
        restartButton = GetComponentInChildren<RestartButton>(true);
    }
}
