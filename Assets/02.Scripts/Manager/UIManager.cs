using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public UIButtonManager btManager;
    public CharacterStatUI charStatUI;
    public GameObject itemInventory;
    public GameObject skillInventory;

    private GameObject potion;
    private GameObject skillToggle;

    public Stack<GameObject> popupStack;

    private void Awake()
    {
        potion = GameObject.Find("Potion");
        potion.SetActive(false);
    }

    private void Start()
    {
        popupStack = new Stack<GameObject>();
    }

    public void PlayerHpBar(int maxhp, int currhp)
    {
        charStatUI.maxHp = maxhp;
        charStatUI.currHp = currhp;
        charStatUI.SetHPBarValue();
    }

    public void PlayerMpBar(int maxMp, int currMp)
    {
        charStatUI.maxMp = maxMp;
        charStatUI.currMp = currMp;
        charStatUI.SetMPBarValue();
    }

    public void PlayerExpBar(int maxExp, int currExp)
    {
        charStatUI.maxExp = maxExp;
        charStatUI.currExp = currExp;
        charStatUI.SetExpBarValue();
    }

    public void PlayerLevel(int playerlevel)
    {
        charStatUI.playerLevel = playerlevel;
        charStatUI.SetUIPlayerLevel();
    }

    public void AddPopUp(GameObject popup)
    {
        popupStack.Push(popup);
    }

    public void RemovePopUp()
    {
        if (popupStack.Count <= 0)
            return;
        popupStack.Pop();
    }

    public void PotionOn()
    {
        potion.SetActive(true);
    }

    public void PotionOff()
    {
        potion.SetActive(false);
    }

    public  void SkillToggleOn()
    {
        skillToggle.SetActive(true);
    }

    public void SkillToggleOff()
    {
        skillToggle.SetActive(false);
    }
}
