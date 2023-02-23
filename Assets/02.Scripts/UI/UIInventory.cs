using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public int slotCount = 102;
    public UIItemSlot uiSlotPrefab;
    public RectTransform content;

    private List<UIItemSlot> slotList = new List<UIItemSlot>();
    private DataTable<ItemData> itemTable;
    //private DataTable<SkillData> skillTable;

    public UIItemInfo itemInfo;

    private void Awake()
    {
        itemTable = DataTableMgr.GetTable<ItemData>();
        //skillTable = DataTableMgr.GetTable<SkillData>();
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
            button.onClick.AddListener(() => itemInfo.Set(slot.Data));
        }
        var itemIds = itemTable.GetAllIds();
        for (int i = 0; i < 50; ++i)
        {
            var index = Random.Range(0, itemIds.Count);
            slotList[i].Set(itemTable.Get(itemIds[index]));
        }
    }
}
