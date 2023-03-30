using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButton : ButtonUi
{
    public void ClickButton()
    {
        ChangeScene();
    }
    public void ChangeScene()
    {
        SceneLoader.Instance.LoadScene("Tutorial");
        //UI.Instance.SetState(LayoutState.Tutorial);
        //SceneManager.LoadScene("Tutorial");
    }
}
