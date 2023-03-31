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
    public MenuPopUp menuPopUp;
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
        menuPopUp = GetComponentInChildren<MenuPopUp>(true);
    }

    private void Start()
    {
        InitUi(sceneName);
    }
    private void InitUi(string sceneName)
    {
        if(sceneName.Equals("Tutorial", StringComparison.OrdinalIgnoreCase))
        {
            SetTutorialUi();
        }
        else if(sceneName.Equals("Village", StringComparison.OrdinalIgnoreCase))
        {
            SetVillageUi();
        }
        else if (sceneName.Equals("Dungeon", StringComparison.OrdinalIgnoreCase))
        {
            SetBattle();
        }
    }

    public void SetTutorialUi()
    {
        charaterUIPanel.ActiveTrue();
        controllerPanel.ActiveTrue();
        menuPanel.ActiveTrue();
        UI.Instance.menuPanel.restartButton.ActivateButton(false);
        UI.Instance.menuPanel.homeButton.ActivateButton(false);
        potionPanel.ActiveTrue();
        skillPanel.ActiveTrue();
        UI.Instance.skillPanel.SkillToggleOff();
        autoPanel.ActiveTrue();
        // Interactable false
        UI.Instance.autoPanel.ActivateToggle(false);
        // reinforce, disassemble, synthetic, gambling, dungeon interactabls false
        popupPanel.ActiveTrue();
        UI.Instance.popupPanel.menuPopUp.SetDefault();
    }

    public void SetVillageUi()
    {
        popupPanel.ActiveTrue();
        charaterUIPanel.ActiveTrue();
        controllerPanel.ActiveTrue();
        menuPanel.ActiveTrue();
        potionPanel.ActiveFalse();
        skillPanel.ActiveFalse();
        autoPanel.ActiveFalse();
        UI.Instance.popupPanel.menuPopUp.SetVillage();

    }

    public void SetBattle()
    {
        popupPanel.ActiveTrue();
        // reinforce, disassemble, synthetic, gambling, dungeon interactabls false
        UI.Instance.popupPanel.menuPopUp.SetDefault();
        charaterUIPanel.ActiveTrue();
        controllerPanel.ActiveTrue();
        menuPanel.ActiveTrue();
        potionPanel.ActiveTrue();
        skillPanel.ActiveTrue();
        autoPanel.ActiveTrue();
    }

    public void SetDungeon()
    {
        popupPanel.ActiveTrue();
        // reinforce, disassemble, synthetic, gambling, dungeon interactabls false
        UI.Instance.popupPanel.menuPopUp.SetDefault();
        charaterUIPanel.ActiveTrue();
        controllerPanel.ActiveTrue();
        menuPanel.ActiveFalse();
        potionPanel.ActiveTrue();
        skillPanel.ActiveTrue();
        autoPanel.ActiveTrue();
    }
}
