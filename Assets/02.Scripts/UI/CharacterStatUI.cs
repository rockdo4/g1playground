using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterStatUI : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image expBar;
    
    public Text hp;
    public Text mp;
    public Text mHp;
    public Text mMp;
    public Text level;

    public int maxHp = 300;
    public int currHp;
    public int maxMp = 300;
    public int currMp;

    //현재
    public int currExp;
    //레벨업에 필요한
    public int maxExp;
    //몬스터 사냥시 현재 경험치 + 증가량
    private int increaseExp = 15;

    public int playerLv;

    //public UIButtonManager ubm;

    private void Start()
    {
        //ubm = GetComponent<UIButtonManager>();
        currHp = maxHp;
        currMp = maxMp;
        currExp = 0;
        maxExp = 100;
        playerLv = 1;
    }

    private void Update()
    {
        hp.text = currHp.ToString();
        mHp.text = maxHp.ToString();
        hpBar.fillAmount = (float)currHp / (float)maxHp;

        mp.text = currMp.ToString();
        mMp.text = maxMp.ToString();
        mpBar.fillAmount = (float)currMp / (float)maxMp;

        level.text = playerLv.ToString();
        expBar.fillAmount = (float)currExp / (float)maxExp;

        if (Input.GetMouseButtonDown(0))
        {
            if(!IsPointerOverUIObject(Input.mousePosition))
            {
                ExpIncrease();
            }            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrPos = new PointerEventData(EventSystem.current);
        eventDataCurrPos.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current
        .RaycastAll(eventDataCurrPos, results);

        return results.Count > 0;
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

    public void HpPotionUse()
    {
        if (maxHp >= currHp + 200)
        {
            currHp += 200;
        }
        else
        {
            currHp = maxHp;
        }
    }

    public void MpPotionUse()
    {
        if (maxMp >= currMp + 100)
        {
            currMp += 100;
        }
        else
        {
            currMp = maxMp;
        }
    }
}
