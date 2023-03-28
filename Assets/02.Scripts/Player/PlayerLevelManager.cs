using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    private Status status;
    private PlayerInventory inventory;
    public int Level; // { get; private set; }
    public int maxLevel = 20;
    public int MaxExp { get; private set; }
    public int CurrExp { get; private set; }

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
        SetExpUi();
        SetLevelUi();
    }

    public void GetExp(int exp)
    {
        CurrExp += exp;
        if (CurrExp >= MaxExp)
        {
            var leftExp = CurrExp - MaxExp;
            LevelUp();
            CurrExp = leftExp;
        }
        SetExpUi();
    }

    public void SetExpUi() => GameManager.instance.uiManager.PlayerExpBar(MaxExp, CurrExp);
    public void SetLevelUi() => GameManager.instance.uiManager.PlayerLevel(Level);
}
