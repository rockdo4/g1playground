using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public string id;
    protected int str;
    protected int dex;
    protected int intel;
    protected int atkPower = 10;
    protected int skillPower = 20;
    protected float criticalChance = 0.5f;
    protected float criticalDamage = 1f;
    protected int defense = 2;
    protected int maxHp = 1000;
    protected int maxMp = 1000;

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

    public void LoadFromTable()
    {

    }

    private void Start()
    {
        //load from dataTable, virtual / player dmg += weapon, player defense += armor
        currHp = 1000;
        currMp = 1000;
    }
}
