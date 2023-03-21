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

    private GameObject charProfile;
    private Image characterImage;

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

    private void Awake()
    {
        charProfile = GameObject.Find("Character");
        characterImage = charProfile.GetComponent<Image>();
    }

    private void OnEnable()
    {
        status = GameManager.instance.player.GetComponent<Status>();
        inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        //level.text = playerLv.ToString();
        //expSlider.fillAmount = (float)currExp / (float)maxExp;

        hpPotionCount.text = inventory.PotionCount[0].ToString();
        mpPotionCount.text = inventory.PotionCount[1].ToString();
    }

    public void SetHPBarValue()
    {
        hp.text = status.CurrHp.ToString() + " ";
        mHp.text = "/ " + status.FinalValue.maxHp.ToString();
        hpSlider.fillAmount = (float)status.CurrHp / (float)status.FinalValue.maxHp;
    }

    public void SetMPBarValue()
    {
        mp.text = status.CurrMp.ToString() + " ";
        mMp.text = "/ " + status.FinalValue.maxMp.ToString();
        mpSlider.fillAmount = (float)status.CurrMp / (float)status.FinalValue.maxMp;
    }

    public void SetExpBarValue()
    {

    }

    public void CharImageSwap(string id)
    {
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
