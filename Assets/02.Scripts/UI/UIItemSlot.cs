using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public int index;
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

    public void Set(int index, ItemData data)
    {
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = data.iconSprite;
    }
}
