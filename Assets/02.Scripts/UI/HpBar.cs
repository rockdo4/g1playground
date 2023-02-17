using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image expBar;
    public Text level;

    //현재
    public int currExp;
    //레벨업에 필요한
    public int maxExp;
    //몬스터 사냥시 현재 경험치 + 증가량
    private int increaseExp = 15;

    public int playerLv;

    private void Start()
    {
        currExp = 0;
        maxExp = 100;
        playerLv = 1;
    }

    private void Update()
    {
        level.text = playerLv.ToString();
        expBar.fillAmount = (float)currExp / (float)maxExp;

        if (Input.GetMouseButtonDown(0))
        {
            ExpIncrease();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ExpIncrease()
    {
        if (currExp + increaseExp >= maxExp)
        {
            playerLv++;
            currExp = (currExp + increaseExp) - maxExp;
        }
        else
            currExp += increaseExp;
    }
}
