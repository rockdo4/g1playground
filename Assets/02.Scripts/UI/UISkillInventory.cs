using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInventory : MonoBehaviour
{
    public int slotCount = 54;
    public UISkillSlot uiSlotPrefab;
    public RectTransform content;

    private List<UISkillSlot> slotList = new List<UISkillSlot>();

    public UISkillInfo skillInfo;

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
            //button.onClick.AddListener(() => skillInfo.Set(slot.Data));
        }
    }
}
