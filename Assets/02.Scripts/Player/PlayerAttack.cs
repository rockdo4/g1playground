using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Serializable]
    public struct WeaponAnim
    {
        public WeaponTypes weaponType;
        public AnimationClip clip;
        public float attackSpeed;
        public float damageTime;
        public float slowModeStart;
        public float slowModeEnd;
        [NonSerialized] public float lastDamageTime;
        [NonSerialized] public float lastSlowModeStart;
        [NonSerialized] public float lastSlowModeEnd;
    }
    private Animator playerAnimator;
    public BasicAttack basicAttack;
    public PlayerAttackBox attackBox;
    public WeaponTypes currWeaponType;
    public float slowModeSpeed;
    [SerializeField] public WeaponAnim[] weaponAnims;
    private Dictionary<WeaponTypes, WeaponAnim> weaponAnimDict = new Dictionary<WeaponTypes, WeaponAnim>();

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        foreach (var weaponAnim in weaponAnims)
        {
            weaponAnimDict[weaponAnim.weaponType] = weaponAnim;
        }
        var allClips = playerAnimator.runtimeAnimatorController.animationClips;
        int len = weaponAnimDict.Count;
        foreach (var weaponAnimPair in weaponAnimDict)
        {
            var weaponAnim = weaponAnimPair.Value;
            foreach (var clip in allClips)
            {
                if (string.Equals(clip.name, weaponAnim.clip.name))
                    weaponAnim.clip = clip;
            }
        }
    }

    private void Update()
    {
        playerAnimator.SetFloat("AttackSpeed", weaponAnimDict[currWeaponType].attackSpeed); // set weapon temporarily, need to set current weapon's attackSpeed
        foreach (var weaponAnim in weaponAnimDict)
        {
            SetDamageTime(weaponAnim.Value);
        }
    }

    public void SetDamageTime(WeaponAnim weaponAnim)
    {
        if (Mathf.Approximately(weaponAnim.lastDamageTime, weaponAnim.damageTime) &&
            Mathf.Approximately(weaponAnim.lastSlowModeStart, weaponAnim.slowModeStart) &&
            Mathf.Approximately(weaponAnim.lastSlowModeEnd, weaponAnim.slowModeEnd))
            return;

        AnimationEvent startAttackEvent = new AnimationEvent();
        startAttackEvent.time = weaponAnim.clip.length * weaponAnim.slowModeStart;
        startAttackEvent.functionName = "ExecuteAttack";
        startAttackEvent.objectReferenceParameter = this;

        AnimationEvent endAttackEvent = new AnimationEvent();
        endAttackEvent.time = weaponAnim.clip.length * weaponAnim.slowModeEnd;
        endAttackEvent.functionName = "EndAttackExecution";
        endAttackEvent.objectReferenceParameter = this;

        AnimationEvent applyDamage = new AnimationEvent();
        applyDamage.time = weaponAnim.clip.length * weaponAnim.damageTime;
        applyDamage.functionName = "ApplyDamage";
        applyDamage.objectReferenceParameter = this;

        AnimationEvent endAttack = new AnimationEvent();
        endAttack.time = weaponAnim.clip.length;
        endAttack.functionName = "EndAttack";
        endAttack.objectReferenceParameter = this;

        weaponAnim.clip.events = null;
        weaponAnim.clip.AddEvent(startAttackEvent);
        weaponAnim.clip.AddEvent(endAttackEvent);
        weaponAnim.clip.AddEvent(applyDamage);
        weaponAnim.clip.AddEvent(endAttack);

        weaponAnim.lastDamageTime = weaponAnim.damageTime;
        weaponAnim.lastSlowModeStart = weaponAnim.slowModeStart;
        weaponAnim.lastSlowModeEnd = weaponAnim.slowModeEnd;
    }

    public void StartAttack() => playerAnimator.SetBool("IsAttacking", true);
    public void EndAttack() => playerAnimator.SetBool("IsAttacking", false);
    public void ExecuteAttack()
    {
        var effect = GameManager.instance.effectManager.GetEffect("Sword Slash 1");
        var effectPos = transform.position;
        effect.transform.position = new Vector3(effectPos.x, effectPos.y + 1f, effectPos.z);
        effect.transform.forward = transform.forward;
        Time.timeScale = slowModeSpeed;
    }
    public void EndAttackExecution() => Time.timeScale = 1f;
    public void ApplyDamage() => attackBox.ExecuteAttack();
    public void AttackTarget(GameObject target, Vector3 attackPos) => basicAttack.ExecuteAttack(gameObject, target, attackPos);
}
