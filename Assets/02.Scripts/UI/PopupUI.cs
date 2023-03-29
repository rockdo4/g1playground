using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : MonoBehaviour
{
    public void ActiveTrue()
    { 
        gameObject.SetActive(true);
    }
    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    protected virtual void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
