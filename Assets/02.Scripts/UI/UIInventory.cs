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
    private PlayerInventory playerInventory;
    private DataTable<ItemData> itemTable;
    public ItemTypes itemType;
    //private DataTable<SkillData> skillTable;

    public UIItemInfo itemInfo;

    private void Awake()
    {
        itemTable = DataTableMgr.GetTable<ItemData>();
        //skillTable = DataTableMgr.GetTable<SkillData>();
    }

    private void Start()
    {
        playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
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
        SetInventory((int)itemType);
    }

    public void SetInventory(int itemType)
    {
        string[] ids = null;
        switch ((ItemTypes)itemType)
        {
            case ItemTypes.Weapon:
                ids = playerInventory.weapons;
                break;
            case ItemTypes.Armor:
                ids = playerInventory.armors;
                break;
            case ItemTypes.Consumable:
                // consumable
                break;
        }
        for (int i = 0; i < slotCount; ++i)
        {
            slotList[i].Set(i, itemTable.Get(ids[i]));
        }
    }
}
