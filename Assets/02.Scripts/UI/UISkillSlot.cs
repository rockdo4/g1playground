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
        icon.sprite = null;
    }

    public void Set(int index, SkillData data)
    {
        this.index = index;
        this.data = data;
        button.interactable = true;
        icon.sprite = data.iconSprite;
    }
}
