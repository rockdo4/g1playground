using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIButtonManager btManager;
    public CharacterStatUI charStatUI;
    [SerializeField]
    private GameObject potion;
    private GameObject skillToggle;

    public Stack<GameObject> popupStack;

    private void Awake()
    {
        //potion = GameObject.Find("Potion");
        //potion.SetActive(false);
    }

    private void Start()
    {
        popupStack = new Stack<GameObject>();
    }

    public void PlayerHpBar(int maxhp, int currhp)
    {
        charStatUI.maxHp = maxhp;
        charStatUI.currHp = currhp;
    }

    public void PlayerMpBar(int maxMp, int currMp)
    {
        charStatUI.maxMp = maxMp;
        charStatUI.currMp = currMp;
    }

    public void AddPopUp(GameObject popup)
    {
        popupStack.Push(popup);
    }

    public void RemovePopUp()
    {
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
}
