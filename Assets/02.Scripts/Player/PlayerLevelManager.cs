using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    private Status status;
    private PlayerInventory inventory;
    [field: SerializeField] public int Level { get; private set; }
    [SerializeField] private string levelUpClip = "UI_Level_Up";
    public int maxLevel = 20;
    public int MaxExp { get; private set; }
    private int currExp;
    public int CurrExp
    {
        get => currExp;
        set
        {
            currExp = value;
            while (currExp >= MaxExp)
            {
                var leftExp = currExp - MaxExp;
                LevelUp();
                currExp = leftExp;
            }
            SetExpUi();
        }
    }

    public void Load(int currLevel, int currExp)
    {
        Level = currLevel;
        CurrExp = currExp;
    }

    public void SetDefault()
    {
        Level = 1;
        CurrExp = 0;
    }

    private void Awake()
    {
        status = GetComponent<Status>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        SetLevel(1);
    }

    public void LevelUp()
    {
        SetLevel(Level + 1);
        GameObject player=GameManager.instance.player;
        GameObject effect = GameManager.instance.effectManager.GetEffect("Level_up");
        effect.transform.SetParent(player.transform,false);
        GameManager.instance.effectManager.ReturnEffectOnTime("Level_up", effect, 2);

        SoundManager.instance.PlaySoundEffect(levelUpClip);

        PlayerDataManager.instance.SaveLevel();
        PlayerDataManager.instance.SaveFile();

    }

    public void SetLevel(int level)
    {
        Level = level;
        if (level <= 10)
            MaxExp = level * 100;
        else
            MaxExp = level * 110;
        CurrExp = 0;
        status.id = level.ToString();
        status.LoadFromTable();
        inventory.ApplyStatus();
        status.Restore();
        SetLevelUi();
    }

    public void SetExpUi() => GameManager.instance.uiManager.PlayerExpBar(MaxExp, currExp);
    public void SetLevelUi() => GameManager.instance.uiManager.PlayerLevel(Level);
}
