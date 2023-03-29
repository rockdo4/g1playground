using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    public int index;
    public Image icon;
    public Button button;

    private SkillData data;

    public SkillData Data
    {
        get => data;
    }

    public void SetEmpty()
    {
        data = null;
        button.interactable = false;
        icon.sprite = null;
    }

    public void Set(int index, SkillData data)
    {
        if (data == null)
            SetEmpty();
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
    }
}
