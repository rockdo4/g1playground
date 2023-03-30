using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPopUp : PopupUI
{
    public bool isOpen;

    protected override void OnEnable()
    {
        isOpen = true;
    }
    protected override void OnDisable()
    {
        isOpen = false;
    }
}
