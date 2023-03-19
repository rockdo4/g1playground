using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public Button hpPotion;
    public Button mpPotion;

    public Image hpSlider;
    public Image mpSlider;
    public Image expSlider;

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
    private PlayerInventory inventory;

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
        inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        hp.text = status.CurrHp.ToString() + " ";
        mHp.text = "/ " + status.FinalValue.maxHp.ToString();
        hpSlider.fillAmount = (float)status.currHp / (float)status.FinalValue.maxHp;

        mp.text = status.CurrMp.ToString() + " ";
        mMp.text = "/ " + status.FinalValue.maxMp.ToString();
        mpSlider.fillAmount = (float)status.currMp / (float)status.FinalValue.maxMp;

        //level.text = playerLv.ToString();
        //expSlider.fillAmount = (float)currExp / (float)maxExp;

        hpPotionCount.text = inventory.PotionCount[0].ToString();
        mpPotionCount.text = inventory.PotionCount[1].ToString();
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
}
