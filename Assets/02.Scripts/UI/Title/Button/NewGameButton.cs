using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : ButtonUi
{
    public override void ClickButton()
    {
        ChangeScene();
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
