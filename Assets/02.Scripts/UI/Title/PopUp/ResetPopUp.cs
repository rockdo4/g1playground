using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPopUp : PopupUI
{
    public void ButtonYes()
    {
        ActiveFalse();
        SaveLoadSystem.RemoveAllData();
    }

    public void ButtonNo()
    {
        ActiveFalse();
    }
}
