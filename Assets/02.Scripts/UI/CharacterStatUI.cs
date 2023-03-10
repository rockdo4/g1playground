using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public Button hpPotion;
    public Button mpPotion;

    public Image hpBar;
    public Image mpBar;
    public Image expBar;

    public TextMeshProUGUI hpPotionCount;
    public TextMeshProUGUI mpPotionCount;
    public Text hp;
    public Text mp;
    public Text mHp;
    public Text mMp;
    //public Text level;

    public int maxHp;
    public int currHp;
    public int maxMp;
    public int currMp;

    public int currHpPotion;
    public int currMpPotion;

    private Status status;

    public int currExp;
    public int maxExp;
    private int increaseExp = 15;

    public int playerLv;

    private void Start()
    {
        status = GameManager.instance.player.GetComponent<Status>();
        currHp = maxHp;
        currMp = maxMp;
        currExp = 0;
        maxExp = 100;
        playerLv = 1;
    }

    private void Update()
    {
        hp.text = status.currHp.ToString() + " ";
        mHp.text = "/ " + maxHp.ToString();
        hpBar.fillAmount = (float)status.currHp / (float)maxHp;

        mp.text = status.currMp.ToString() + " ";
        mMp.text = "/ " + maxMp.ToString();
        mpBar.fillAmount = (float)status.currMp / (float)maxMp;

        //level.text = playerLv.ToString();
        expBar.fillAmount = (float)currExp / (float)maxExp;

        hpPotionCount.text = currHpPotion.ToString();
        mpPotionCount.text = currMpPotion.ToString();
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
        if(currHpPotion > 0)
        {
            if (maxHp >= status.currHp + 200)
            {
                status.currHp += 200;
            }
            else
            {
                status.currHp = maxHp;
            }
            currHpPotion--;
        }        
    }

    public void MpPotionUse()
    {
        if(currMpPotion > 0)
        {
            if (maxMp >= status.currMp + 100)
            {
                status.currMp += 100;
            }
            else
            {
                status.currMp = maxMp;
            }
            currMpPotion--;
        }        
    }
}
