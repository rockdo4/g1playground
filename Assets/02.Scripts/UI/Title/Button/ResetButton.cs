using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : ButtonUi
{
    public ResetPopUp resetPopUp;

    public void ClickButton()
    {
        resetPopUp.ActiveTrue();
        //SaveLoadSystem.RemoveAllData();
    }
}
