using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "StraightSpell", menuName = "SkillAttack/StraightSpell")]
public class StraightSpell : SkillAttack
{
    public string projectileId;
    public int maxCount;
    public float interval;

    public void Fire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        var aStat = attacker.GetComponent<Status>();
        MonoBehaviour monoBehaviour = attacker.GetComponent<MonoBehaviour>();
        if (aStat == null || aStat.currMp < reqMana)
            return; 
        aStat.currMp -= reqMana;
        monoBehaviour.StartCoroutine(CoFire(attacker, startPos, direction));
    }

    private IEnumerator CoFire(GameObject attacker, Vector3 startPos, Vector3 direction)
    {
        int count = 0;
        while (true)
        {
            var projectile = GameManager.instance.projectileManager.Get(projectileId);
            projectile.OnCollided = ExecuteAttack;
            projectile.Fire(attacker, startPos, direction, range, lifeTime, false, false);
            if (++count >= maxCount)
                break;
            yield return new WaitForSeconds(interval);
        }
    }
}
