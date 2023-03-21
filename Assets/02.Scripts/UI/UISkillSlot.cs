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
        button.interactable = false;
    }

    public void Set(int index, SkillData data)
    {
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
    }
}
