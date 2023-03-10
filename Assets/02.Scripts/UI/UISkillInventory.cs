using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInventory : MonoBehaviour
{
    public int slotCount = 54;
    public UISkillSlot uiSlotPrefab;
    public RectTransform content;

    private List<UISkillSlot> slotList = new List<UISkillSlot>();
    private PlayerSkills playerSkills;

    public UISkillInfo skillInfo;

    private void Awake()
    {
        playerSkills = GameManager.instance.player.GetComponent<PlayerSkills>();
    }

    public void OnEnable()
    {
        Init();
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
        List<string> ids = new List<string>();
        int len = 0;
        foreach (var skill in playerSkills.allSkills)
        {
            ids.Add(skill.id);
        }
        len = ids.Count;
        var table = DataTableMgr.GetTable<SkillData>();
        for (int i = 0; i < len; ++i)
        {
            slotList[i].Set(i, table.Get(ids[i]));
        }
    }
}
