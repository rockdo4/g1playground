using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPopUp : PopupUI
{
    public ButtonUi skill;
    //public ButtonUi equip;
    public ButtonUi item;
    public ButtonUi map;
    public ButtonUi exit;
    //public ButtonUi dungeon;
    public ButtonUi reinforce;
    public ButtonUi synthetic;
    public ButtonUi disassemble;
    public ButtonUi gambling;
    public ButtonUi setting;

    public bool isOpen;
    private void Awake()
    {
        Init();
        //skill = GetComponentInChildren<SkillButton>(true);
        ////equip = GetComponentInChildren<EquipButton>(true);
        //item = GetComponentInChildren<ItemButton>(true);
        ////map = GetComponentInChildren<MapButton>(true);
        //exit = GetComponentInChildren<ExitButton>(true);
        ////dungeon = GetComponentInChildren<DungeonButton>(true);
        //reinforce = GetComponentInChildren<EnhanceButton>(true);
        //synthetic = GetComponentInChildren<SyntheticButton>(true);
        //disassemble = GetComponentInChildren<DisassembleButton>(true);
        //gambling = GetComponentInChildren<GambleButton>(true);
        //setting = GetComponentInChildren<SettingButton>(true);
    }

    private void Init()
    {
        if (skill == null)
        {
            skill = GetComponentInChildren<SkillButton>(true);
            item = GetComponentInChildren<ItemButton>(true);
            exit = GetComponentInChildren<ExitButton>(true);
            reinforce = GetComponentInChildren<EnhanceButton>(true);
            synthetic = GetComponentInChildren<SyntheticButton>(true);
            disassemble = GetComponentInChildren<DisassembleButton>(true);
            gambling = GetComponentInChildren<GambleButton>(true);
            setting = GetComponentInChildren<SettingButton>(true);
            map = GetComponentInChildren<SettingButton>(true);
        }
    }

    public void SetTutorial()
    {
        Init();
        skill.ActivateButton(true);
        //equip.ActivateButton(true);
        item.ActivateButton(false);
        // map.ActivateButton(true);
        exit.ActivateButton(true);
        //dungeon.ActivateButton(false);
        reinforce.ActivateButton(false);
        synthetic.ActivateButton(false);
        disassemble.ActivateButton(false);
        gambling.ActivateButton(false);
        setting.ActivateButton(true);
        gameObject.SetActive(true);
    }
    public void SetDefault()
    {
        Init();
        skill.ActivateButton(true);
        //equip.ActivateButton(true);
        item.ActivateButton(false);
        exit.ActivateButton(true);
        //dungeon.ActivateButton(false);
        reinforce.ActivateButton(false);
        synthetic.ActivateButton(false);
        disassemble.ActivateButton(false);
        gambling.ActivateButton(false);
        setting.ActivateButton(true);
    }

    public void SetVillage()
    {
        Init();
        skill.ActivateButton(true);
        //equip.ActivateButton(true);
        item.ActivateButton(false);
        map.ActivateButton(true);
        exit.ActivateButton(true);
        //dungeon.ActivateButton(true);
        reinforce.ActivateButton(true);
        synthetic.ActivateButton(true);
        disassemble.ActivateButton(true);
        gambling.ActivateButton(true);
        setting.ActivateButton(true);
    }

    protected override void OnEnable()
    {
        isOpen = true;
        //Dont Clear
    }

    protected override void OnDisable()
    {
        isOpen = false;
        //Dont Clear
    }
}
