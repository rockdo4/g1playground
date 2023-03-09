using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    protected GameObject attacker;
    public System.Action<GameObject, GameObject, Vector3> OnCollided;
    protected float lifeTime;
    protected float timer;
    public string[] detachedEffects;
    public List<GameObject> effects = new List<GameObject>();
    public string hitEffect;
    public string flashEffect;
    protected List<GameObject> attackedList = new List<GameObject>();

    protected virtual void OnTriggerStay(Collider other)
    {
        
    }
}
