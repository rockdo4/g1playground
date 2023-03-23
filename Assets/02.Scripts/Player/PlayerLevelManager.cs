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
        SetLevel(1);
    }

    public void LevelUp()
    {
        SetLevel(Level + 1);
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
    }
}
