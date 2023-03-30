using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : PanelUi
{
    public Skill1Toggle skill1Toggle;
    public Skill2Toggle skill2Toggle;
    private void Awake()
    {
        skill1Toggle = GetComponentInChildren<Skill1Toggle>();
        skill2Toggle = GetComponentInChildren<Skill2Toggle>();  
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SkillToggleOff()
    {
        skill1Toggle.ActivateToggle(false);
        skill2Toggle.ActivateToggle(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
