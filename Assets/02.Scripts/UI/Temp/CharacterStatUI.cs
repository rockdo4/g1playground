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
    public TextMeshProUGUI level;
    public TextMeshProUGUI exp;
    public TextMeshProUGUI mExp;

    public int maxHp;
    public int currHp;
    public int maxMp;
    public int currMp;
    public int maxExp;
    public int currExp;
    public int playerLevel;

    public int currHpPotion;
    public int currMpPotion;

    private Status status;
    private PlayerInventory inventory;
    private PlayerLevelManager levelManager;

    private void Awake()
    {
        //charProfile = GameObject.Find("Character");
        //characterImage = charProfile.GetComponent<Image>();
        status = GameManager.instance.player.GetComponent<Status>();
        inventory = GameManager.instance.player.GetComponent<PlayerInventory>();
        levelManager = GameManager.instance.player.GetComponent<PlayerLevelManager>();
    }

    private void Update()
    {
        hpPotionCount.text = inventory.PotionCount[0].ToString();
        mpPotionCount.text = inventory.PotionCount[1].ToString();
    }

    public void SetHPBarValue()
    {
        hp.text = "HP bar " + status.CurrHp.ToString() + " ";
        mHp.text = "/ " + status.FinalValue.maxHp.ToString();
        hpSlider.fillAmount = (float)status.CurrHp / (float)status.FinalValue.maxHp;
    }

    public void SetMPBarValue()
    {
        mp.text = "MP bar " + status.CurrMp.ToString() + " ";
        mMp.text = "/ " + status.FinalValue.maxMp.ToString();
        mpSlider.fillAmount = (float)status.CurrMp / (float)status.FinalValue.maxMp;
    }

    public void SetExpBarValue()
    {
        exp.text = "XP bar " + levelManager.CurrExp.ToString() + " ";
        mExp.text = "/ " + levelManager.MaxExp.ToString();
        expSlider.fillAmount = (float)levelManager.CurrExp / (float)levelManager.MaxExp;
    }

    public void SetUIPlayerLevel()
    {
        level.text = "Lv." + levelManager.Level.ToString();
    }
}
