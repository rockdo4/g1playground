using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOnOff : MonoBehaviour
{
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        if(!toggle.isOn)
        {
            toggle.image.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            toggle.image.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
