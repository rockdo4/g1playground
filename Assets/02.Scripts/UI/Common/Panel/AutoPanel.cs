using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoPanel : PanelUi
{
    private Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ActivateToggle(bool isinteractable)
    {
        toggle.interactable = isinteractable;
    }
}
