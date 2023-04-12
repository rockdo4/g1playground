using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SkillAttack : AttackDefinition
{
    public string Group;// { get; private set; }

    [Header("Values from DataTable")]
    public string id;
    public int Reinforce;// { get; private set; }
    public int MaxReinfore;//{ get; private set; }
    public int reqMana;
    public float damageFigure;
    public float criticalChance;
    public float criticalDamage;
    [SerializeField] public Attack.CC cc;
    public float coolDown;

    [Header("Values only fix on Inspector")]
    public bool isOnOffSkill = false;
    public string fireSoundEffect;
    public string inUseSoundEffect;
    public string hitSoundEffect;

    public void SetData(string id)
    {
        var data = DataTableMgr.GetTable<SkillData>().Get(id);
        if (data == null)
            return;
        this.id = data.id;
        Reinforce = data.reinforce;
        MaxReinfore= data.maxReinfore;
        reqMana = data.reqMana;
        damageFigure = data.damageFigure;
        criticalChance = data.criticalChance;
        criticalDamage = data.criticalDamage;
        cc = data.cc;
        coolDown = data.CoolDown;
    }

    public virtual void Update() { }

    public override Attack CreateAttack(Status attacker, Status defenser)   // temporary formula
    {
        var criticalChance = attacker.FinalValue.skillCriChance + this.criticalChance;
        var isCritical = UnityEngine.Random.value < criticalChance;
        float damage = attacker.FinalValue.skillPower * damageFigure;
        if (isCritical)
            damage *= (attacker.FinalValue.skillCriDamage + this.criticalDamage);
        damage -= defenser.FinalValue.skillDef;
        if (damage < 0)
            damage = 0;
        return new Attack((int)damage, cc, isCritical);
    }

    public override void ExecuteAttack(GameObject attacker, GameObject defender, Vector3 attackPos)
    {
        if (attacker == null || defender == null)
            return;

        var aStat = attacker.GetComponent<Status>();
        var dStat = defender.GetComponent<Status>();
        if (aStat == null || dStat == null)
            return;

        var attack = CreateAttack(aStat, dStat);

        var attackables = defender.GetComponents<IAttackable>();
        foreach (var attackable in attackables)
        {
            attackable.OnAttack(attacker, attack, attackPos);
        }
    }

    public void SaveSkillData() // if skillData or SkillAttack changes, must fix this code
    {
        var path = "Assets\\Resources\\DataTables\\Skill_Table.csv";
        if (!File.Exists(path))
            Debug.Log("File not exists");
        string[] strs = File.ReadAllLines(path);
        int len = strs.Length;
        for (int i = 0; i < len; ++i)
        {
            var split = strs[i].Split(',');
            if (string.Equals(split[0], id))
            {
                var temp = split;
                strs[i] = string.Concat(
                    id, ',',
                    temp[1], ',',
                    temp[2], ',',
                    temp[3], ',',
                    temp[4], ',',
                    temp[5], ',',
                    temp[6], ',',
                    Reinforce, ',',
                    MaxReinfore, ',',
                    coolDown, ',',
                    reqMana, ',',
                    damageFigure, ',',
                    criticalChance, ',',
                    criticalDamage, ',',
                    cc.stunTime, ',',
                    cc.knockBackForce, ',',
                    cc.slowDown, ',',
                    cc.slowTime, ',',
                    cc.reduceDef, ',',
                    cc.reduceDefTime, ',',
                    temp[20], ',',
                    temp[21], ','
                    );
                break;
            }
        }

        var newStrs = string.Empty;
        for (int i = 0; i < len; ++i)
        {
            newStrs = string.Concat(newStrs, strs[i], "\n");
        }
        File.WriteAllText(path, newStrs, Encoding.Default);
    }
}
