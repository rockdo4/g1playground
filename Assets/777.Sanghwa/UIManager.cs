using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIPopupManager popupManager;
    public UIButtonManager btManager;

    private void Awake()
    {
        Instance = this;
    }
}
