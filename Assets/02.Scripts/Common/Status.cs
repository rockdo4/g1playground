using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int Str { get; protected set; }
    public int Dex { get; protected set; }
    public int Int { get; protected set; }

    // if lower stats below are determined by above properties -> below properties will be abstract
    public int AtkPower; //{ get; protected set; }
    public int SkillPower; //{  get; protected set; }
    public float CriticalChance; //{ get; protected set; }
    public float CriticalDamage; //{ get; protected set; }
    public int Defense; //{ get; protected set; }
    public int MaxHp; //{ get; protected set; }
    public int currHp;

    private void Start()
    {
        //load from dataTable, virtual / player dmg += weapon, player defense += armor
        currHp = MaxHp;
    }
}
