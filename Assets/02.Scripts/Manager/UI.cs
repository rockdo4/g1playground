using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LayoutState
{
    None,
    Title,
    Tutorial,
    Village,
    Main,
    Weekly,
}
public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    public TitleLayout title;
    public PopUpPanel popupPanel;
    public CharacterUIPanel charaterUIPanel;
    public ControllerPanel controllerPanel;
    public MenuPanel menuPanel;
    public PotionPanel potionPanel;
    //public MiniMapPanel minimapPanel;
    public SkillPanel skillPanel;
    public AutoPanel autoPanel;
    //public SceneLoader loading;

    private LayoutState State;
    [SerializeField]
    private string sceneName;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        bool isEqual = sceneName.Equals("Title", StringComparison.OrdinalIgnoreCase);

        if (isEqual)
            title = GetComponentInChildren<TitleLayout>(true);

        popupPanel = GetComponentInChildren<PopUpPanel>(true);
        charaterUIPanel = GetComponentInChildren<CharacterUIPanel>(true);
        controllerPanel = GetComponentInChildren<ControllerPanel>(true);
        menuPanel = GetComponentInChildren<MenuPanel>(true);
        potionPanel = GetComponentInChildren<PotionPanel>(true);
        //minimapPanel = GetComponentInChildren<MiniMapPanel>(true);
        skillPanel = GetComponentInChildren<SkillPanel>(true);
        autoPanel = GetComponentInChildren<AutoPanel>(true);

    }

    public void SetState(LayoutState state)
    {
        switch (State)
        {
            case LayoutState.Title:
                title.ActiveFalse();
                break;
        }

        State = state;

        switch (state)
        {
            case LayoutState.Title:
                title.ActiveTrue();
                break;
        }
    }
}
