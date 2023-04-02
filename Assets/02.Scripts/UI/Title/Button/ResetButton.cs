using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : ButtonUi
{
    public void ClickButton()
    {
        SaveLoadSystem.RemoveAllData();
    }
}
