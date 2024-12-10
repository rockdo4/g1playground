using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    public int index;
    public Image icon;
    public Button button;
    public Image currFrame;

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
        this.index = index;
        if (data == null)
        {
            SetEmpty();
            return;
        }
        this.data = data;
        button.interactable = true;
        icon.sprite = Resources.Load<Sprite>(DataTableMgr.GetTable<IconData>().Get(data.iconSpriteId).iconName);
    }

    public void IsCurrSkill(bool currSkill) => currFrame.gameObject.SetActive(currSkill);
}
