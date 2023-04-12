using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPanel : PanelUi
{
    public MenuPopUp menuPopUp;
    public PopupUI exitPopUp;
    public PopupUI skillPopUp;
    public PopupUI itemPopUp;
    public PopupUI worldMapPopUp;
    public PopupUI reinforcePopUp;
    public PopupUI disassemblePopUp;
    public PopupUI syntheticPopUp;
    public PopupUI settingPopUp;
    public PopupUI stageRewardPopUp;
    public PopupUI stageDeathPopUp;
    public PopupUI gamblingPopUp;
    public PopupUI gamblingResult1PopUp;
    public PopupUI gamblingResult10PopUp;
    public PopupUI profilePopUp;
    public PopupUI probabilityPopUp;
    public PopupUI[] messagePopUp;
    public MenuSlider menuSlider;

    public override void ActiveTrue()
    {
        base.ActiveTrue();
        Init();
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (menuPopUp != null)
            return;
        menuPopUp = GetComponentInChildren<MenuPopUp>(true);
        menuPopUp.ActiveFalse();
        exitPopUp = GetComponentInChildren<ExitPopUp>(true);
        skillPopUp = GetComponentInChildren<SkillPopUp>(true);
        itemPopUp = GetComponentInChildren<ItemPopUp>(true);
        worldMapPopUp = GetComponentInChildren<WorldMapPopUp>(true);
        reinforcePopUp = GetComponentInChildren<ReinforcePopUp>(true);
        disassemblePopUp = GetComponentInChildren<DisassemblePopUp>(true);
        syntheticPopUp = GetComponentInChildren<SyntheticPopUp>(true);
        settingPopUp = GetComponentInChildren<SettingPopUp>(true);
        stageRewardPopUp = GetComponentInChildren<StageRewardPopUp>(true);
        stageDeathPopUp = GetComponentInChildren<StageDeathPopUp>(true);
        gamblingPopUp = GetComponentInChildren<GamblingPopUp>(true);
        gamblingResult1PopUp = GetComponentInChildren<GamblingResult1PopUp>(true);
        gamblingResult10PopUp = GetComponentInChildren<GamblingResult10PopUp>(true);
        profilePopUp = GetComponentInChildren<ProfilePopUp>(true);
        probabilityPopUp = GetComponentInChildren<ProbabilityPopUp>(true);
        menuSlider = GetComponentInChildren<MenuSlider>(true);
    }

    public void AllClose()
    {
        Init();
        menuPopUp.ActiveFalse();
        exitPopUp.ActiveFalse();
        skillPopUp.ActiveFalse();
        itemPopUp.ActiveFalse();
        worldMapPopUp.ActiveFalse();
        reinforcePopUp.ActiveFalse();
        disassemblePopUp.ActiveFalse();
        syntheticPopUp.ActiveFalse();
        settingPopUp.ActiveFalse();
        stageRewardPopUp.ActiveFalse();
        gamblingPopUp.ActiveFalse();
        gamblingResult1PopUp.ActiveFalse();
        gamblingResult10PopUp.ActiveFalse();
        profilePopUp.ActiveFalse();
        probabilityPopUp.ActiveFalse();
    }

    void Update()
    {

    }
}
