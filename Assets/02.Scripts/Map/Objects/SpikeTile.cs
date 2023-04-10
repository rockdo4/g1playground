using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTile : MonoBehaviour
{
    public bool Istriggered { get; set; }
    private float damagePercentage = 0.2f;
    [SerializeField] private float delay = 2f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        var status = other.GetComponent<PlayerStatus>();
        if (status != null && timer >= delay)
        {
            timer = 0f;

            var damage = (int)(status.FinalValue.maxHp * damagePercentage);
            Attack.CC newCC = Attack.CC.None;
            newCC.knockBackForce = 7f;
            var attack = new Attack(damage, newCC, false);
            var attackables = other.GetComponents<IAttackable>();
            foreach (var attackable in attackables)
            {
                attackable.OnAttack(gameObject, attack, other.transform.position);
            }
        }
    }
}
