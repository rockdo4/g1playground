using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Attack
{
    [Serializable]
    public struct CC
    {
        public float knockBackForce;
        public float stunTime;
        public float slowDown;
        public float slowTime;
        public float reduceDef;
        public float reduceDefTime;

        public static CC None
        {
            get
            {
                CC newCC;
                newCC.knockBackForce = 0f;
                newCC.stunTime = 0f;
                newCC.slowDown = 0f;
                newCC.slowTime = 0f;
                newCC.reduceDef = 0f;
                newCC.reduceDefTime = 0f;
                return newCC;
            }
        }
    }
    public int Damage { get; private set; }
    public CC Cc { get; private set; }
    public bool IsCritical { get; private set; }

    // addtional option

    public Attack(int damage, CC cc, bool isCritical)
    {
        Damage = damage;
        Cc = cc;
        IsCritical = isCritical;
    }
}
