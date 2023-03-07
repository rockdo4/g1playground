using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator playerAnimator;
    public AnimationClip[] attackAnimClips;
    public BasicAttack basicAttack;
    public PlayerAttackBox attackBox;
    public float attackSpeed = 1f;
    public float startTimeRate = 0.3f;
    public float endTimeRate = 0.5f;
    private float lastStartTimeRate;
    private float lastEndTimeRate;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerAnimator.SetFloat("AttackSpeed", attackSpeed);
        var tempClips = attackAnimClips;
        var allClips = playerAnimator.runtimeAnimatorController.animationClips;
        attackAnimClips = new AnimationClip[tempClips.Length];
        int i = 0;
        foreach (var clip in allClips)
        {
            foreach (var tempClip in tempClips)
            {
                if (string.Equals(clip.name, tempClip.name))
                {
                    attackAnimClips[i++] = clip;
                    break;
                }
            }
        }

        SetDamageTime(startTimeRate, endTimeRate);
    }

    public void StartAttack() => playerAnimator.SetBool("IsAttacking", true);
    public void ExecuteAttack() => attackBox.ExecuteAttack();
    public void EndAttackExecution() => attackBox.EndAttackExecution();
    public void SetDamageTime(float startTime, float endTime)
    {
        if (Mathf.Approximately(lastStartTimeRate, startTime) && Mathf.Approximately(lastEndTimeRate, endTime))
            return;
        foreach (var attackAnimClip in attackAnimClips)
        {
            AnimationEvent startAttackEvent = new AnimationEvent();
            startAttackEvent.time = attackAnimClip.length * startTime;
            startAttackEvent.functionName = "ExecuteAttack";
            startAttackEvent.objectReferenceParameter = this;

            AnimationEvent endAttackEvent = new AnimationEvent();
            endAttackEvent.time = attackAnimClip.length * endTime;
            endAttackEvent.functionName = "EndAttackExecution";
            endAttackEvent.objectReferenceParameter = this;

            attackAnimClip.events = null;
            attackAnimClip.AddEvent(startAttackEvent);
            attackAnimClip.AddEvent(endAttackEvent);
        }
        lastStartTimeRate = startTime;
        lastEndTimeRate = endTime;
    }
    public void AttackTarget(GameObject target, Vector3 attackPos) => basicAttack.ExecuteAttack(gameObject, target, attackPos);
}
