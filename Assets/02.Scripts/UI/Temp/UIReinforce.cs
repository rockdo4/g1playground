using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIReinforce : MonoBehaviour
{
    public int slotCount = 102;
    private int currSlot;
    public UIItemSlot uiItemSlotPrefab;
    public UISkillSlot uiSkillSlotPrefab;
    public GameObject ItemInventory;
    public GameObject skillInventory;
    public RectTransform itemContent;
    public RectTransform skillContent;
    private List<UIItemSlot> itemSlotList = new List<UIItemSlot>();
    private List<UISkillSlot> skillSlotList = new List<UISkillSlot>();
    private PlayerInventory playerInventory;
    private PlayerSkills playerSkills;
    public ReinforceSystem.Types type;
    public UiReinforceInfo info;
    public Button reinforceButton;

    private void Awake()
    {
        playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        Init();
    }

    public void OnEnable()
    {
        SetInventory((int)ReinforceSystem.Types.Weapon);
    }

    public void Init()
    {
        for (int i = 0; i < slotCount; ++i)
        {
            int slotIndex = i;

            var itemSlot = Instantiate(uiItemSlotPrefab, itemContent);
            itemSlot.SetEmpty();
            itemSlotList.Add(itemSlot);
            var itemButton = itemSlot.GetComponent<Button>();
            itemButton.onClick.AddListener(() => CheckReinforcable(itemSlot.Data.id));
            itemButton.onClick.AddListener(() => currSlot = slotIndex);

            var skillSlot = Instantiate(uiSkillSlotPrefab, skillContent);
            skillSlot.SetEmpty();
            skillSlotList.Add(skillSlot);
            var skillButton = skillSlot.GetComponent<Button>();
            skillButton.onClick.AddListener(() => CheckReinforcable(skillSlot.Data.id));
            skillButton.onClick.AddListener(() => currSlot = slotIndex);
        }
        reinforceButton.onClick.AddListener(() => Reinforce());
        SetInventory((int)type);
    }

    public void ClearInventory()
    {
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
            case ReinforceSystem.Types.Armor:
                skillContent.gameObject.SetActive(false);
                itemContent.gameObject.SetActive(true);
                foreach (var slot in itemSlotList)
                {
                    slot.SetEmpty();
                }
                break;
            case ReinforceSystem.Types.Skill:
                itemContent.gameObject.SetActive(false);
                skillContent.gameObject.SetActive(true);
                foreach (var slot in skillSlotList)
                {
                    slot.SetEmpty();
                }
                break;
        }
        info.SetEmpty();
    }

    public void SetInventory(int type)
    {
        this.type = (ReinforceSystem.Types)type;
        ClearInventory();
        ShowInventory(this.type);
        List<string> ids = null;
        currSlot = -1;
        int len = 0;
        int count = 0;
        switch ((ReinforceSystem.Types)type)
        {
            case ReinforceSystem.Types.Weapon:
                {
                    var table = DataTableMgr.GetTable<WeaponData>();
                    ids = playerInventory.Weapons;
                    len = ids.Count;
                    if (!string.IsNullOrEmpty(playerInventory.CurrWeapon))
                    {
                        itemSlotList[count].IsEquiped(true);
                        itemSlotList[count].Set(-1, table.Get(playerInventory.CurrWeapon));
                        ++count;
                    }
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                        {
                            itemSlotList[count].IsEquiped(false);
                            itemSlotList[count].Set(i, table.Get(ids[i]));
                            ++count;
                        }
                    }
                    if (itemSlotList.Count > 0)
                    {
                        currSlot = 0;
                        CheckReinforcable(itemSlotList[currSlot].Data.id);
                    }
                    else
                        reinforceButton.interactable = false;
                }
                break;
            case ReinforceSystem.Types.Armor:
                {
                    var table = DataTableMgr.GetTable<ArmorData>();
                    ids = playerInventory.Armors;
                    len = ids.Count;
                    if (!string.IsNullOrEmpty(playerInventory.CurrArmor))
                    {
                        itemSlotList[count].IsEquiped(true);
                        itemSlotList[count].Set(-1, table.Get(playerInventory.CurrArmor));
                        ++count;
                    }
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                        {
                            itemSlotList[count].IsEquiped(false);
                            itemSlotList[count].Set(i, table.Get(ids[i]));
                            ++count;
                        }
                    }
                    if (itemSlotList.Count > 0)
                    {
                        currSlot = 0;
                        CheckReinforcable(itemSlotList[currSlot].Data.id);
                    }
                    else
                        reinforceButton.interactable = false;
                }
                break;
            case ReinforceSystem.Types.Skill:
                {
                    var table = DataTableMgr.GetTable<SkillData>();
                    ids = playerSkills.PossessedSkills;
                    len = ids.Count;
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                            skillSlotList[i].Set(i, table.Get(ids[i]));
                    }
                    if (skillSlotList.Count > 0)
                    {
                        currSlot = 0;
                        CheckReinforcable(skillSlotList[currSlot].Data.id);
                    }
                    else
                        reinforceButton.interactable = false;
                }
                break;
        }
    }

    private void ShowInventory(ReinforceSystem.Types type)
    {
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
            case ReinforceSystem.Types.Armor:
                ItemInventory.SetActive(true);
                skillInventory.SetActive(false);
                break;
            case ReinforceSystem.Types.Skill:
                ItemInventory.SetActive(false);
                skillInventory.SetActive(true);
                break;
        }
    }

    public void CheckReinforcable(string id)
    {
        if (info.Set(type, id))
        {
            reinforceButton.interactable = true;
            return;
        }
        reinforceButton.interactable = false;
    }

    public void Deselect()
    {
        if (currSlot < 0)
            return;
        currSlot = -1;
        info.SetEmpty();
    }

    public void Reinforce()
    {
        if (currSlot < 0)
            return;
        if (!ReinforceSystem.CheckMaterials(type, itemSlotList[currSlot].Data.id))
        {
            info.ShowPopUp("재료가 부족합니다");
            return;
        }
        int index = 0;
        switch (type)
        {
            case ReinforceSystem.Types.Weapon:
            case ReinforceSystem.Types.Armor:
                index = itemSlotList[currSlot].index;
                break;
            case ReinforceSystem.Types.Skill:
                index = skillSlotList[currSlot].index;
                break;
        }
        ReinforceSystem.Reinforce(type, index);
        info.SetEmpty();
        SetInventory((int)type);
    }
}
