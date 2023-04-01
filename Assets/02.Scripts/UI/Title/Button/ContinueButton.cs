using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ContinueButton : ButtonUi
{
    public void ClickButton()
    {
        
        SceneLoader.Instance.LoadScene("Scene02");

    }



}
