using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConsumeSlot : MonoBehaviour
{
    public int index;
    public Image icon;
    public Button button;
    public TextMeshProUGUI count;

    private PlayerInventory inventory;

    private ItemData data;

    public ItemData Data
    {
        get => data;
    }

    private void Awake()
    {
        inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
    }

    public void SetEmpty()
    {
        data = null;
        button.interactable = false;
        icon.sprite = null;
        count.text = string.Empty;
    }

    public void Set(int index, ItemData data)
    {
        if (data == null)
            SetEmpty();
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        count.text = inventory.Consumables.Count.ToString();
    }
}
