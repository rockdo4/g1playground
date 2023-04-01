using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiDisassemble : MonoBehaviour
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
    public UiDisassembleInfo info;
    public Button reinforceButton;
    public GameObject resultPopup;

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
            itemButton.onClick.AddListener(() => info.Set(type, itemSlot.Data.id));
            itemButton.onClick.AddListener(() => currSlot = slotIndex);

            var skillSlot = Instantiate(uiSkillSlotPrefab, skillContent);
            skillSlot.SetEmpty();
            skillSlotList.Add(skillSlot);
            var skillButton = skillSlot.GetComponent<Button>();
            skillButton.onClick.AddListener(() => info.Set(type, skillSlot.Data.id));
            skillButton.onClick.AddListener(() => currSlot = slotIndex);
        }
        reinforceButton.onClick.AddListener(() => Disassemble());
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
        currSlot = -1;
        List<string> ids = null;
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

    public void Deselct()
    {
        if (currSlot < 0)
            return;
        currSlot = -1;
        info.SetEmpty();
    }

    public void Disassemble()
    {
        if (currSlot == -1)
            return;

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
        ReinforceSystem.Disassemble(type, index);
        currSlot = -1;
        info.SetEmpty();
        StartCoroutine(CoShowResult());
        SetInventory((int)type);
    }

    private IEnumerator CoShowResult()
    {
        resultPopup.SetActive(true);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (stopwatch.Elapsed.TotalSeconds < 1)
        {
            yield return null;
        }
        stopwatch.Stop();
        resultPopup.SetActive(false);
    }
}
