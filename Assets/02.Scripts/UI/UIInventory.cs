using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public int slotCount = 102;
    public UIItemSlot uiSlotPrefab;
    public RectTransform content;

    private List<UIItemSlot> slotList = new List<UIItemSlot>();
    private PlayerInventory playerInventory;
    public ItemTypes itemType;

    public UIItemInfo itemInfo;

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
        }
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
        string[] ids = null;
        int len = 0;
        switch ((ItemTypes)itemType)
        {
            case ItemTypes.Weapon:
                ids = playerInventory.weapons;
                len = playerInventory.weapons.Length;
                for (int i = 0; i < len; ++i)
                {
                    slotList[i].Set(i, DataTableMgr.GetTable<WeaponData>().Get(ids[i]));
                }
                break;
            case ItemTypes.Armor:
                ids = playerInventory.armors;
                len = playerInventory.armors.Length;
                for (int i = 0; i < len; ++i)
                {
                    slotList[i].Set(i, DataTableMgr.GetTable<ArmorData>().Get(ids[i]));
                }
                break;
            case ItemTypes.Consumable:
                ids = playerInventory.consumes;
                len = playerInventory.consumes.Length;
                for (int i = 0; i < len; ++i)
                {
                    slotList[i].Set(i, DataTableMgr.GetTable<ConsumeData>().Get(ids[i]));
                }
                break;
        }
    }
}
