using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiCompose : MonoBehaviour
{
    public int slotCount = 102;
    private int currSlot;
    public UIItemSlot uiItemSlotPrefab;
    public ScrollRect scroll;
    public RectTransform itemContent;

    private List<UIItemSlot> itemSlotList = new List<UIItemSlot>();
    private PlayerInventory playerInventory;
    public ItemTypes type;
    private List<UIItemSlot> selectedSlots = new List<UIItemSlot>();
    public UiComposeInfo info;
    public Button composeButton;
    public GameObject resultPopup;

    private void Awake()
    {
        playerInventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        Init();
    }

    public void OnEnable()
    {
        SetInventory((int)ReinforceSystem.Types.Weapon);
        scroll.verticalNormalizedPosition = 1f;
        resultPopup.SetActive(false);
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
            itemButton.onClick.AddListener(() => Select(itemSlot));
        }
        composeButton.onClick.AddListener(() => Compose());
        SetInventory((int)type);
    }

    public void ClearInventory()
    {
        foreach (var slot in itemSlotList)
        {
            slot.SetEmpty();
        }
        info.SetEmptyAll();
        selectedSlots.Clear();
    }

    public void SetInventory(int type)
    {
        if ((ItemTypes)type == ItemTypes.Consumable)
            return;

        if (this.type != (ItemTypes)type)
            scroll.verticalNormalizedPosition = 1f;
        this.type = (ItemTypes)type;
        ClearInventory();
        List<string> ids = null;
        int len = 0;
        int count = 0;
        switch ((ItemTypes)type)
        {
            case ItemTypes.Weapon:
                {
                    var table = DataTableMgr.GetTable<WeaponData>();
                    ids = playerInventory.Weapons;
                    len = ids.Count;
                    if (!string.IsNullOrEmpty(playerInventory.CurrWeapon))
                    {
                        if (ComposeSystem.CheckComposable(playerInventory.CurrWeapon))
                        {
                            itemSlotList[count].IsEquiped(true);
                            itemSlotList[count].Set(-1, table.Get(playerInventory.CurrWeapon));
                            ++count;
                        }
                    }
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]) && ComposeSystem.CheckComposable(ids[i]))
                        {
                            itemSlotList[count].IsEquiped(false);
                            itemSlotList[count].Set(i, table.Get(ids[i]));
                            ++count;
                        }
                    }
                }
                break;
            case ItemTypes.Armor:
                {
                    var table = DataTableMgr.GetTable<ArmorData>();
                    ids = playerInventory.Armors;
                    len = ids.Count;
                    if (!string.IsNullOrEmpty(playerInventory.CurrArmor))
                    {
                        if (ComposeSystem.CheckComposable(playerInventory.CurrArmor))
                        {
                            itemSlotList[count].IsEquiped(true);
                            itemSlotList[count].Set(-1, table.Get(playerInventory.CurrArmor));
                            ++count;
                        }
                    }
                    for (int i = 0; i < len; ++i)
                    {
                        if (!string.IsNullOrEmpty(ids[i]) && ComposeSystem.CheckComposable(ids[i]))
                        {
                            itemSlotList[count].IsEquiped(false);
                            itemSlotList[count].Set(i, table.Get(ids[i]));
                            ++count;
                        }
                    }
                }
                break;
        }
        composeButton.interactable = false;
    }

    public void Select(UIItemSlot slot)
    {
        if (selectedSlots.Count == 0)
        {
            selectedSlots.Add(slot);
            slot.button.interactable = false;
            var secondMaterial = ComposeSystem.Get2ndMaterial(slot.Data.id);
            foreach (var itemSlot in itemSlotList)
            {
                if (itemSlot.Data != null && !string.Equals(itemSlot.Data.id, secondMaterial))
                    itemSlot.button.interactable = false;
            }
            info.SetMaterial(0, slot.Data.id);
            composeButton.interactable = true;
            //if (!ComposeSystem.CheckComposable(slot.Data.id))
            //{
            //    // message
            //}
        }
        else
        {
            selectedSlots.Add(slot);
            info.SetMaterial(1, slot.Data.id);
            foreach (var itemSlot in itemSlotList)
            {
                itemSlot.button.interactable = false;
            }
        }
    }

    public void Deselect(int index)
    {
        if (index == 0)
        {
            if (selectedSlots.Count < 1)
                return;
            selectedSlots.Clear();
            info.SetEmptyAll();
            foreach (var itemSlot in itemSlotList)
            {
                if (itemSlot.Data != null)
                    itemSlot.button.interactable = true;
            }
            composeButton.interactable = false;
        }
        else
        {
            if (selectedSlots.Count < 2)
                return;
            selectedSlots.RemoveAt(index);
            info.SetEmpty(index);
            var secondMaterial = ComposeSystem.Get2ndMaterial(selectedSlots[0].Data.id);
            foreach (var itemSlot in itemSlotList)
            {
                if (itemSlot != selectedSlots[0] && itemSlot.Data != null && string.Equals(itemSlot.Data.id, secondMaterial))
                    itemSlot.button.interactable = true;
                else
                    itemSlot.button.interactable = false;
            }
        }
    }

    public void Compose()
    {
        if (selectedSlots.Count < 2)
        {
            StartCoroutine(CoShowPopup("재료가 부족합니다"));
            return;
        }
        var data = ComposeSystem.GetData(selectedSlots[0].Data.id);
        var indexs = new int[2];
        indexs[0] = selectedSlots[0].index;
        indexs[1] = selectedSlots[1].index;
        ComposeSystem.Compose(type, data, indexs);
        info.SetEmptyAll();
        SetInventory((int)type);
    }

    public IEnumerator CoShowPopup(string text)
    {
        resultPopup.SetActive(true);
        resultPopup.GetComponentInChildren<TextMeshProUGUI>().text = text;
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
