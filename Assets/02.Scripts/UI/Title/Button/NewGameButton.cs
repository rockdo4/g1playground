using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        string savepath = Application.persistentDataPath + "/Save/Save_Player.bin";

        if (File.Exists(savepath))
        {
            File.Delete(savepath);
        }

        SceneLoader.Instance.LoadScene("Tutorial");
        //UI.Instance.SetState(LayoutState.Tutorial);
        //SceneManager.LoadScene("Tutorial");
    }
}
