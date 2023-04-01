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

    public Button currWeapon;
    public Button currArmor;

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
        currWeapon.onClick.AddListener(() => SetCurrEquipInfo(ItemTypes.Weapon));
        currArmor.onClick.AddListener(() => SetCurrEquipInfo(ItemTypes.Armor));
        SetInventory((int)itemType);
        equipButton.onClick.AddListener(() => Equip());
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
        var playerStatus = GameManager.instance.player.GetComponent<PlayerStatus>();
        itemInfo.ShowSlider(playerStatus.DefaultValue + playerStatus.EquipValue);
        // make Count in itemTypes, return if itemType >= ItemTypes.Count
        ClearInventory();
        currSlot = -1;
        itemInfo.SetEmpty();
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
        if (!string.IsNullOrEmpty(playerInventory.CurrWeapon))
            currWeapon.image.sprite = DataTableMgr.LoadIcon(DataTableMgr.GetTable<WeaponData>().Get(playerInventory.CurrWeapon).iconSpriteId);
        else
            currWeapon.image.sprite = null;

        if (!string.IsNullOrEmpty(playerInventory.CurrArmor))
            currArmor.image.sprite = DataTableMgr.LoadIcon(DataTableMgr.GetTable<ArmorData>().Get(playerInventory.CurrArmor).iconSpriteId);
        else
            currArmor.image.sprite = null;
    }

    public void SetCurrEquipInfo(ItemTypes type)
    {
        ItemData data = null;
        switch (type)
        {
            case ItemTypes.Weapon:
                if (!string.IsNullOrEmpty(playerInventory.CurrWeapon))
                    data = DataTableMgr.GetTable<WeaponData>().Get(playerInventory.CurrWeapon);
                break;
            case ItemTypes.Armor:
                if (!string.IsNullOrEmpty(playerInventory.CurrArmor))
                    data = DataTableMgr.GetTable<ArmorData>().Get(playerInventory.CurrArmor);
                break;
            default:
                return;
        }
        currSlot = -1;
        if (itemInfo != null)
            itemInfo.Set(data);
    }

    public void Equip()
    {
        if (currSlot < 0)
            return;

        switch (itemType)
        {
            case ItemTypes.Weapon:
                if (slotList[currSlot] != null && slotList[currSlot].Data != null)
                    playerInventory.SetWeapon(currSlot);
                
                break;
            case ItemTypes.Armor:
                if (slotList[currSlot] != null && slotList[currSlot].Data != null)
                    playerInventory.SetArmor(currSlot);
                break;
            default:
                return;
        }
        itemInfo.SetEmpty();
        SetInventory((int)itemType);
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