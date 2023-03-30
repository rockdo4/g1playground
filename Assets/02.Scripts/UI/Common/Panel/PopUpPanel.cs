using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpPanel : PanelUi
{
    public ButtonUi skill;
    public ButtonUi item;
    public ButtonUi map;
    public ButtonUi exit;
    public ButtonUi reinforce;
    public ButtonUi synthetic;
    public ButtonUi disassemble;
    public ButtonUi gambling;
    public ButtonUi setting;

    public PopupUI menuPopUp;
    public PopupUI exitPopUp;
    public PopupUI skillPopUp;
    public PopupUI itemPopUp;
    public PopupUI worldMapPopUp;
    public PopupUI reinforcePopUp;
    public PopupUI disassemblePopUp;
    public PopupUI syntheticPopUp;
    public PopupUI settingPopUp;
    public PopupUI stageRewardPopUp;
    public PopupUI gamblingPopUp;
    public PopupUI gamblingResult1PopUp;
    public PopupUI gamblingResult10PopUp;
    public PopupUI profilePopUp;
    public PopupUI probabilityPopUp;
    public MenuSlider menuSlider;

    private void Awake()
    {
        skill = GetComponentInChildren<SkillButton>(true);
        item = GetComponentInChildren<ItemButton>(true);
        map = GetComponentInChildren<MapButton>(true);
        exit = GetComponentInChildren<ExitButton>(true);
        reinforce = GetComponentInChildren<EnhanceButton>(true);
        synthetic = GetComponentInChildren<SyntheticButton>(true);
        disassemble = GetComponentInChildren<DisassembleButton>(true);
        gambling = GetComponentInChildren<GambleButton>(true);
        setting = GetComponentInChildren<SettingButton>(true);

        menuPopUp = GetComponentInChildren<MenuPopUp>(true);
        exitPopUp = GetComponentInChildren<ExitPopUp>(true);
        skillPopUp = GetComponentInChildren<SkillPopUp>(true);
        itemPopUp = GetComponentInChildren<ItemPopUp>(true);
        worldMapPopUp = GetComponentInChildren<WorldMapPopUp>(true);
        reinforcePopUp = GetComponentInChildren<ReinforcePopUp>(true);
        disassemblePopUp = GetComponentInChildren<DisassemblePopUp>(true);
        syntheticPopUp = GetComponentInChildren<SyntheticPopUp>(true);
        settingPopUp = GetComponentInChildren<SettingPopUp>(true);
        stageRewardPopUp = GetComponentInChildren<StageRewardPopUp>(true);
        gamblingPopUp = GetComponentInChildren<GamblingPopUp>(true);
        gamblingResult1PopUp = GetComponentInChildren<GamblingResult1PopUp>(true);
        gamblingResult10PopUp = GetComponentInChildren<GamblingResult10PopUp>(true);
        profilePopUp = GetComponentInChildren<ProfilePopUp>(true);
        probabilityPopUp = GetComponentInChildren<ProbabilityPopUp>(true);

        menuSlider =GetComponentInChildren<MenuSlider>(true);   
    }

    public void SetDefault()
    {
        skill.ActivateButton(true);
        item.ActivateButton(true);
        map.ActivateButton(true);
        exit.ActivateButton(true);
        reinforce.ActivateButton(false);
        synthetic.ActivateButton(false);
        disassemble.ActivateButton(false);
        gambling.ActivateButton(false);
        setting.ActivateButton(true);
    }

    public void SetVillage()
    {
        skill.ActivateButton(true);
        item.ActivateButton(true);
        map.ActivateButton(true);
        exit.ActivateButton(true);
        reinforce.ActivateButton(true);
        synthetic.ActivateButton(true);
        disassemble.ActivateButton(true);
        gambling.ActivateButton(true);
        setting.ActivateButton(true);
    }

    public void AllClose()
    {
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
