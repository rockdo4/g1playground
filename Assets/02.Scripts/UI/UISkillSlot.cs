using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour
{
    public Image icon;
    public Button button;

    public void SetEmpty()
    {
        button.interactable = false;
        icon.sprite = null;
    }
}
