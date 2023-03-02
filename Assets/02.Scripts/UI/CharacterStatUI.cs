using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image expBar;
    
    public Text hp;
    public Text mp;
    public Text mHp;
    public Text mMp;
    //public Text level;

    public int maxHp = 300;
    public int currHp;
    public int maxMp = 300;
    public int currMp;

    private Status status;

    //����
    public int currExp;
    //�������� �ʿ���
    public int maxExp;
    //���� ��ɽ� ���� ����ġ + ������
    private int increaseExp = 15;

    public int playerLv;

    private void Start()
    {
        status = GetComponent<Status>();
        currHp = maxHp;
        currMp = maxMp;
        currExp = 0;
        maxExp = 100;
        playerLv = 1;
    }

    private void Update()
    {
        hp.text = currHp.ToString() + " ";
        mHp.text = "/ " + maxHp.ToString();
        hpBar.fillAmount = (float)currHp / (float)maxHp;

        mp.text = currMp.ToString() + " ";
        mMp.text = "/ " + maxMp.ToString();
        mpBar.fillAmount = (float)currMp / (float)maxMp;

        //level.text = playerLv.ToString();
        expBar.fillAmount = (float)currExp / (float)maxExp;
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
