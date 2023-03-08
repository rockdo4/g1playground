using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public int index;
    public Image icon;
    public Button infoButton;

    private ItemData data;

    public ItemData Data
    {
        get => data;
    }

    public void SetEmpty()
    {
        infoButton.interactable = false;
        icon.sprite = null;
    }

    public void Set(int index, ItemData data)
    {
        this.index = index;
        this.data = data;
        infoButton.interactable = true;
        icon.sprite = data.iconSprite;
    }
}
