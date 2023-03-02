using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    private int str;
    private int dex;
    //private int int;
    private int atkPower;
    public int skillPower;
    public float criticalChance;
    public float criticalDamage;
    public int defense;
    public int maxHp;
    public int maxMp;

    public virtual int Str { get; protected set; }
    public virtual int Dex { get; protected set; }
    public virtual int Int { get; protected set; }
    public virtual int AtkPower { get; protected set; }
    public virtual int SkillPower { get; protected set; }
    public virtual float CriticalChance { get; protected set; }
    public virtual float CriticalDamage { get; protected set; }
    public virtual int Defense { get; protected set; }
    public virtual int MaxHp { get; protected set; }
    public virtual int MaxMp { get; protected set; }
    public int currHp;
    public int currMp;

    public void LoadFromTable(string id)
    {
        // load from table
    }

    private void Start()
    {
        //load from dataTable, virtual / player dmg += weapon, player defense += armor
        currHp = MaxHp;
        currMp = MaxMp;
    }
}
