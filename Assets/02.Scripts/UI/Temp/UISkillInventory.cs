using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInventory : MonoBehaviour
{
    public int slotCount;
    public UISkillSlot uiSlotPrefab;
    public ScrollRect scroll;
    public RectTransform content;

    private List<UISkillSlot> slotList = new List<UISkillSlot>();
    private PlayerSkills playerSkills;
    private int currSlot;

    private int playerSkillIndex = -1;
    public UISkillInventoryPlayerSkill[] inventoryPlayerSkills = new UISkillInventoryPlayerSkill[2];
    public UISkillInfo skillInfo;
    public Button equipButton;
    public Button unequipButton;

    private void Awake()
    {
        playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
        Init();
    }

    private void OnEnable()
    {
        SetInventory();
        scroll.verticalNormalizedPosition = 1f;
    }

    public void Init()
    {
        for (int i = 0; i < slotCount; ++i)
        {
            var slot = Instantiate(uiSlotPrefab, content);
            slot.SetEmpty();
            slotList.Add(slot);

            var button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => skillInfo.Set(slot.Data));
            var count = i;
            button.onClick.AddListener(() => currSlot = count);
        }
        SetInventory();
    }

    public void ClearInventory()
    {
        foreach (var slot in slotList)
        {
            slot.SetEmpty();
        }
    }

    public void SetInventory()
    {
        // make Count in itemTypes, return if itemType >= ItemTypes.Count
        ClearInventory();
        currSlot = -1;
        SetPlayerSkillIndex(-1);
        skillInfo.SetEmpty();
        List<string> ids = playerSkills.PossessedSkills;
        int len = ids.Count;
        if (slotList.Count < len)
            SlotInstantiate(len - slotList.Count);
        var count = slotList.Count;
        var table = DataTableMgr.GetTable<SkillData>();
        for (int i = 0; i < count; ++i)
        {
            if (i < len)
                slotList[i].Set(i, table.Get(ids[i]));
            else
                slotList[i].Set(i, null);
        }
        //if (len > 0)
        //{
        //    skillInfo.Set(table.Get(ids[0]));
        //    currSlot = 0;
        //}
        SetCurrSkill();
        skillInfo.ShowCurrPlayerSkills();
    }

    private void SetCurrSkill()
    {
        var currSkill1 = playerSkills.GetCurrSkillIndex(0);
        var currSkill2 = playerSkills.GetCurrSkillIndex(1);

        foreach (var slot in slotList)
        {
            slot.IsCurrSkill(slot.index == currSkill1 || slot.index == currSkill2);
        }
    }

    public void SlotInstantiate(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var slot = Instantiate(uiSlotPrefab, content);
            slot.SetEmpty();
            slotList.Add(slot);

            var button = slot.GetComponent<Button>();
            button.onClick.AddListener(() => skillInfo.Set(slot.Data));
            var slotIndex = slotCount + i;
            button.onClick.AddListener(() => currSlot = slotIndex);
        }
        slotCount += count;
        SetInventory();
    }

    public void SetPlayerSkillIndex(int index)
    {
        if (index >= 0 && playerSkillIndex == index)
        {
            equipButton.interactable = false;
            unequipButton.interactable = false;
            inventoryPlayerSkills[playerSkillIndex].OnOffFrame(false);
            playerSkillIndex = -1;
            return;
        }
        if (index < 0)
        {
            equipButton.interactable = false;
            unequipButton.interactable = false;
        }
        else
        {
            equipButton.interactable = true;
            if (playerSkills.GetCurrSkillIndex(index) < 0)
                unequipButton.interactable = false;
            else
                unequipButton.interactable = true;
        }
        playerSkillIndex = index;
        var len = inventoryPlayerSkills.Length;
        for (int i = 0; i < len; ++i)
        {
            inventoryPlayerSkills[i].OnOffFrame(i == playerSkillIndex);
        }
    }

    public void SetPlayerSkill()
    {
        if (currSlot < 0 || playerSkillIndex < 0)
            return;

        playerSkills.SetSkill(playerSkillIndex, currSlot);
        skillInfo.ShowCurrPlayerSkills();
        SetInventory();
    }

    public void SetPlayerSkillEmpty()
    {
        playerSkills.SetEmpty(playerSkillIndex);
        skillInfo.ShowCurrPlayerSkills();
        SetInventory();
    }
}
