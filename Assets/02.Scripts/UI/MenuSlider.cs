using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlider : MonoBehaviour
{
    private Scrollbar scrollbar;
    //public CharacterProfile characterProfile;
    //public SkillButton skillButton;
    //public EquipButton equipButton;
    public EnhanceButton enhanceButton;
    //public MapButton mapButton;
    //public ItemButton itemButton;
    public DisassembleButton disassembleButton;
    public SyntheticButton syntheticButton;
    //public BookButton bookButton;
    public GambleButton gambleButton;
    //public SettingButton settingButton;

    //ButtonUi nowState;
    private void Awake()
    {
        scrollbar = GetComponentInChildren<Scrollbar>();
        //characterProfile = GetComponentInChildren<CharacterProfile>(true);
        //skillButton = GetComponentInChildren<SkillButton>(true);
        //equipButton = GetComponentInChildren<EquipButton>(true);
        enhanceButton = GetComponentInChildren<EnhanceButton>(true);
        //mapButton = GetComponentInChildren<MapButton>(true);
        //itemButton = GetComponentInChildren<ItemButton>(true);
        disassembleButton = GetComponentInChildren<DisassembleButton>(true);
        syntheticButton = GetComponentInChildren<SyntheticButton>(true);
        //bookButton = GetComponentInChildren<BookButton>(true);
        gambleButton = GetComponentInChildren<GambleButton>(true);
        //settingButton = GetComponentInChildren<SettingButton>(true);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void AllClear()
    {
        UI.Instance.popupPanel.AllClose();
    }
    //public ButtonUi curUiButton;
    //public void SetState(ButtonUi temp)
    //{
        
    //    switch (temp)
    //    {
    //        case CharacterProfile:
    //            break;
    //        case SkillButton:
    //            break;
    //        case EquipButton:
    //            UI.Instance.popupPanel.equipPopUp.ActiveTrue();
    //            break;
    //        case EnhanceButton:
    //            break;
    //        case MapButton:
    //            break;
    //        case ItemButton:
    //            break;
    //        case DisassembleButton:
    //            break;
    //        case SyntheticButton:
    //            break;
    //        case GambleButton:
    //            break;
    //        case SettingButton:
    //            break;
    //    }
    //    curUiButton = temp;
    //}

    // Update is called once per frame
    void Update()
    {

    }
    public void ActiveTrue() => gameObject.SetActive(true);
    public void ActiveFalse() => gameObject.SetActive(false);
}
