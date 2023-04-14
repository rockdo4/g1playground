using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillInventoryPlayerSkill : MonoBehaviour
{
    public Image currSkillFrame;

    public void OnOffFrame(bool onOff) => currSkillFrame.gameObject.SetActive(onOff);
}
