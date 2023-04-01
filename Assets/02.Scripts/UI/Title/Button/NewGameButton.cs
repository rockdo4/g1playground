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
        string savepath2= Application.persistentDataPath + "/Save/Save_Stage.bin";
        string savepath3= Application.persistentDataPath + "/Save/Save_Dungeon.bin";
        if (File.Exists(savepath))
        {
            File.Delete(savepath);
        }
        if (File.Exists(savepath2))
        {
            File.Delete(savepath2);
        }
        if (File.Exists(savepath3))
        {
            File.Delete(savepath3);
        }
        SceneLoader.Instance.LoadScene("Tutorial");
        //UI.Instance.SetState(LayoutState.Tutorial);
        //SceneManager.LoadScene("Tutorial");
    }
}
