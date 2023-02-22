using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Image icon;
    public Button button;

    private ItemData data;

    public ItemData Data
    {
        get => data;
    }

    public void SetEmpty()
    {
        button.interactable = false;
        icon.sprite = null;
    }

    public void Set(ItemData data)
    {
        this.data = data;
        button.interactable = true;
        icon.sprite = data.iconSprite;
    }
}
