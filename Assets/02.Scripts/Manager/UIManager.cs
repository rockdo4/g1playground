using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIButtonManager btManager;
    public UIPopupManager popupManager;
    public CharacterStatUI charStatUI;

    public Stack<GameObject> popupStack;

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
}
