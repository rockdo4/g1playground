using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public Button hpPotion;
    public Button mpPotion;

    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;

    public TextMeshProUGUI hpPotionCount;
    public TextMeshProUGUI mpPotionCount;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI mp;
    public TextMeshProUGUI mHp;
    public TextMeshProUGUI mMp;
    //public Text level;

    public int maxHp;
    public int currHp;
    public int maxMp;
    public int currMp;

    public int currHpPotion;
    public int currMpPotion;

    private Status status;

    //public int currExp;
    //public int maxExp;
    //private int increaseExp = 15;

    //public int playerLv;

    //private void Start()
    //{
    //    status = GameManager.instance.player.GetComponent<Status>();
    //    status.currHp = status.FinalValue.maxHp;
    //    status.currMp = status.FinalValue.maxMp;
    //    //currExp = 0;
    //    //maxExp = 100;
    //    //playerLv = 1;
    //}

    private void OnEnable()
    {
        status = GameManager.instance.player.GetComponent<Status>();
        status.CurrHp = status.FinalValue.maxHp;
        status.CurrMp = status.FinalValue.maxMp;
    }

    private void Update()
    {
        hp.text = status.CurrHp.ToString() + " ";
        mHp.text = "/ " + status.FinalValue.maxHp.ToString();
        //hpBar.fillAmount = (float)status.currHp / (float)status.FinalValue.maxHp;
        hpSlider.value = (float)status.CurrHp / (float)status.FinalValue.maxHp;

        mp.text = status.CurrMp.ToString() + " ";
        mMp.text = "/ " + status.FinalValue.maxMp.ToString();
        mpSlider.value = (float)status.CurrMp / (float)status.FinalValue.maxMp;

        //level.text = playerLv.ToString();
        //expSlider.value = (float)currExp / (float)maxExp;

        hpPotionCount.text = currHpPotion.ToString();
        mpPotionCount.text = currMpPotion.ToString();
    }

    //private void ExpIncrease()
    //{
    //    if (currExp + increaseExp >= maxExp)
    //    {
    //        playerLv++;
    //        currExp = (currExp + increaseExp) - maxExp;
    //    }
    //    else
    //        currExp += increaseExp;
    //}

    public void HpPotionUse()
    {
        if(currHpPotion > 0)
        {
            if (status.FinalValue.maxHp >= status.CurrHp + 200)
            {
                status.CurrHp += 200;
            }
            else
            {
                status.CurrHp = status.FinalValue.maxHp;
            }
            currHpPotion--;
        }        
    }

    public void MpPotionUse()
    {
        if(currMpPotion > 0)
        {
            if (status.FinalValue.maxMp >= status.CurrMp + 100)
            {
                status.CurrMp += 100;
            }
            else
            {
                status.CurrMp = status.FinalValue.maxMp;
            }
            currMpPotion--;
        }        
    }
}
