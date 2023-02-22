using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public int slotCount = 102;
    public UISlot uiSlotPrefab;
    public RectTransform content;

    private List<UISlot> slotList = new List<UISlot>();

    public UIItemInfo itemInfo;

    private void Awake()
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
            //button.onClick.AddListener(() => itemInfo.Set(slot.Data));
        }
    }
}
