using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupUIButton : MonoBehaviour
{
    public PopupUI _ui;

    private void Awake()
    {
    }

    public void OpenCloseUI()
    {
        if (!_ui.gameObject.activeSelf)
            _ui.gameObject.SetActive(true);
        else
            _ui.gameObject.SetActive(false);
    }
}
