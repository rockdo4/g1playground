using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public int index;
    public Image icon;
    public Button button;
    public Image currFrame;

    private ItemData data;

    public ItemData Data
    {
        get => data;
    }

    public void SetEmpty()
    {
        data = null;
        button.interactable = false;
        icon.sprite = null;
        icon.color = Color.clear;
    }

    public void Set(int index, ItemData data)
    {
        if (data == null)
            SetEmpty();
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
        icon.color = Color.white;
    }

    public void IsEquiped(bool equiped)
    {
        if (equiped)
            currFrame.gameObject.SetActive(true);
        else
            currFrame.gameObject.SetActive(false);
    }
}
