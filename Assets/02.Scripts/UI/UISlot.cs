using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    public Image icon;
    public Button button;

    public void SetEmpty()
    {
        button.interactable = false;
        icon.sprite = null;
    }
}
