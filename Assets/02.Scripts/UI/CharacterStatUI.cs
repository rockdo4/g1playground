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
        status.currHp = status.FinalValue.maxHp;
        status.currMp = status.FinalValue.maxMp;
        currExp = 0;
        maxExp = 100;
        playerLv = 1;
    }

    private void Update()
    {
        hp.text = status.currHp.ToString() + " ";
        mHp.text = "/ " + status.FinalValue.maxHp.ToString();
        hpBar.fillAmount = (float)status.currHp / (float)status.FinalValue.maxHp;

        mp.text = status.currMp.ToString() + " ";
        mMp.text = "/ " + status.FinalValue.maxMp.ToString();
        mpBar.fillAmount = (float)status.currMp / (float)status.FinalValue.maxMp;

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
            if (status.FinalValue.maxHp >= status.currHp + 200)
            {
                status.currHp += 200;
            }
            else
            {
                status.currHp = status.FinalValue.maxHp;
            }
            currHpPotion--;
        }        
    }

    public void MpPotionUse()
    {
        if(currMpPotion > 0)
        {
            if (status.FinalValue.maxMp >= status.currMp + 100)
            {
                status.currMp += 100;
            }
            else
            {
                status.currMp = status.FinalValue.maxMp;
            }
            currMpPotion--;
        }        
    }
}
