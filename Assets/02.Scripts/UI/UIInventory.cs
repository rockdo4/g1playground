using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public int slotCount;
    private int currSlot;
    public UIItemSlot uiSlotPrefab;
    public RectTransform content;

    private List<UIItemSlot> slotList = new List<UIItemSlot>();
    private PlayerInventory playerInventory;
    public ItemTypes itemType;

    public UIItemInfo itemInfo;
    public Button equipButton;

    private void Awake()
    {
        playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        Init();
    }

    public void OnEnable()
    {
        itemType = ItemTypes.Weapon;
        SetInventory((int)itemType);
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
            int slotIndex = i;
            button.onClick.AddListener(() => currSlot = slotIndex);
        }
        equipButton.onClick.AddListener(() => Equip());
        SetInventory((int)itemType);
    }

    public void ClearInventory()
    {
        foreach (var slot in slotList)
        {
            slot.SetEmpty();
        }
    }

    public void SetInventory(int itemType)
    {
        // make Count in itemTypes, return if itemType >= ItemTypes.Count
        ClearInventory();
        this.itemType = (ItemTypes)itemType;
        List<string> ids = null;
        int len = 0;
        switch ((ItemTypes)itemType)
        {
            case ItemTypes.Weapon:
                {
                    var table = DataTableMgr.GetTable<WeaponData>();
                    ids = playerInventory.Weapons;
                    len = playerInventory.Weapons.Count;
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                            slotList[i].Set(i, table.Get(ids[i]));
                    }
                }
                break;
            case ItemTypes.Armor:
                {
                    var table = DataTableMgr.GetTable<ArmorData>();
                    ids = playerInventory.Armors;
                    len = playerInventory.Armors.Count;
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                            slotList[i].Set(i, table.Get(ids[i]));
                    }
                }
                break;
            case ItemTypes.Consumable:
                {
                    var table = DataTableMgr.GetTable<ConsumeData>();
                    var consumables = playerInventory.Consumables;
                    ids = new List<string>();
                    foreach (var consumable in consumables)
                    {
                        ids.Add(consumable.id);
                    }
                    len = playerInventory.Consumables.Count;
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]))
                            slotList[i].Set(i, table.Get(ids[i]));
                    }
                }
                break;
        }
    }

    public void Equip()
    {
        string id = null;
        switch (itemType)
        {
            case ItemTypes.Weapon:
                if (slotList[currSlot] != null && slotList[currSlot].Data != null)
                {
                    playerInventory.SetWeapon(currSlot);
                    id = playerInventory.Weapons[currSlot];
                    var table = DataTableMgr.GetTable<WeaponData>();
                    if (string.IsNullOrEmpty(id))
                        slotList[currSlot].SetEmpty();
                    else
                        slotList[currSlot].Set(currSlot, table.Get(id));
                }
                break;
            case ItemTypes.Armor:
                if (slotList[currSlot] != null && slotList[currSlot].Data != null)
                {
                    playerInventory.SetArmor(currSlot);
                    id = playerInventory.Armors[currSlot];
                    var table = DataTableMgr.GetTable<ArmorData>();
                    if (string.IsNullOrEmpty(id))
                        slotList[currSlot].SetEmpty();
                    else
                        slotList[currSlot].Set(currSlot, table.Get(id));
                }
                break;
            default:
                return;
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
            button.onClick.AddListener(() => itemInfo.Set(slot.Data));
            int slotIndex = slotCount + i;
            button.onClick.AddListener(() => currSlot = slotIndex);
        }
        slotCount += count;
        SetInventory((int)itemType);
    }
}
//slotcount = 10
//new + 10
// 0 1 2 3 4 5 6 7 8 9 = 기존 슬롯 인덱스
// 10 11 12 13 14 15 16 17 18 19 = 추가 슬롯 인덱스