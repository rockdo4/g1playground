using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public virtual void Idle() { }
    public virtual void Patrol() { }
    public virtual void Chase() { }
    public virtual void Attack() { }
    public virtual void Skill() { }
    public virtual void TakeDamage() { }
    public virtual void Groggy() { }
    public virtual void Die() { }
}