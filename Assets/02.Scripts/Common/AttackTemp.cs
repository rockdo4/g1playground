using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTemp : MonoBehaviour
{
    private MeshRenderer thisRenderer;
    private bool isOnCoolDown = false;
    public float coolDown = 2f;
    private float timer = 0f;
    public float attackDuration = 1f;
    public int blinkCount = 2;

    void Awake()
    {
        thisRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isOnCoolDown)
        {
            timer += Time.deltaTime;
            if (timer > coolDown)
            {
                isOnCoolDown = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isOnCoolDown && other.CompareTag("Enemy"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        isOnCoolDown = true;
        timer = 0f;
        StartCoroutine(CoAttack());
    }

    private IEnumerator CoAttack()
    {
        var maxCount = blinkCount * 2;
        var cycle = attackDuration / maxCount;
        var count = 0;
        while (count < maxCount)
        {
            ++count;
            if (!thisRenderer.enabled)
                thisRenderer.enabled = true;
            else
                thisRenderer.enabled = false;
            yield return new WaitForSeconds(cycle);
        }
        thisRenderer.enabled = true;
    }
}
