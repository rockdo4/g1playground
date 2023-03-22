using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public int slotCount = 102;
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
    }

    private void Start()
    {
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
                    ids = playerInventory.weapons;
                    len = playerInventory.weapons.Count;
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
                    ids = playerInventory.armors;
                    len = playerInventory.armors.Count;
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
                    var consumables = playerInventory.consumables;
                    ids = new List<string>();
                    foreach (var consumable in consumables)
                    {
                        ids.Add(consumable.id);
                    }
                    len = playerInventory.consumables.Count;
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
                    id = playerInventory.weapons[currSlot];
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
                    id = playerInventory.armors[currSlot];
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
}
