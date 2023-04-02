using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerAttack;

public class PlayerAttack : MonoBehaviour
{
    [Serializable]
    public struct WeaponSet
    {
        public WeaponTypes weaponType;
        public GameObject weaponGameObject;
        public AnimatorOverrideController overrideController;
        public AnimationClip clip;
        public string attackSound;
        public string hitSound;
        public float attackRange;
        public float attackSpeed;
        public float damageTime;
        public float slowModeDuration;
        [NonSerialized] public float lastDamageTime;
        [NonSerialized] public float lastSlowModeDuration;
    }
    private Animator playerAnimator;
    private AnimatorOverrider playerAnimatorOverrider;
    public BasicAttack basicAttack;
    public PlayerAttackBox attackBox;
    public WeaponTypes currWeaponType;
    public float slowModeSpeed;
    [SerializeField] public WeaponSet[] weaponSets;
    private Dictionary<WeaponTypes, WeaponSet> weaponSetDict = new Dictionary<WeaponTypes, WeaponSet>();
    private List<WeaponTypes> weaponSetDictKeys;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAnimatorOverrider = GetComponent<AnimatorOverrider>();
        foreach (var weaponAnim in weaponSets)
        {
            var newWeaponAnim = weaponAnim;
            var allClips = newWeaponAnim.overrideController.animationClips;
            foreach (var clip in allClips)
            {
                if (string.Equals(clip.name, newWeaponAnim.clip.name))
                    newWeaponAnim.clip = clip;
            }
            weaponSetDict[newWeaponAnim.weaponType] = newWeaponAnim;
        }
        weaponSetDictKeys = weaponSetDict.Keys.ToList();
        //SetAll();
        currWeaponType = WeaponTypes.None;
    }

    public void SetAll()
    {
        foreach (var key in weaponSetDictKeys)
        {
            SetDamageTime(key);
        }
        SetWeapon(currWeaponType);
    }

    public void SetWeapon(WeaponTypes weaponType)
    {
        currWeaponType = weaponType;
        foreach (var weaponSet in weaponSetDict)
        {
            if (weaponSet.Value.weaponGameObject != null)
            {
                if (weaponSet.Value.weaponType != currWeaponType)
                    weaponSet.Value.weaponGameObject.SetActive(false);
                else
                    weaponSet.Value.weaponGameObject.SetActive(true);
            }
        }
        SetDamageTime(weaponType);
        attackBox.ResizeAttackBox(weaponSetDict[currWeaponType].attackRange);
        playerAnimatorOverrider.SetAnimations(weaponSetDict[weaponType].overrideController);
        var speed = weaponSetDict[currWeaponType].attackSpeed * weaponSetDict[currWeaponType].clip.length;
        playerAnimator.SetFloat("AttackSpeed", speed);
    }

    public void SetDamageTime(WeaponTypes type)
    {
        WeaponSet temp = weaponSetDict[type];

        AnimationEvent attackSound = new AnimationEvent();
        attackSound.time = temp.clip.length * 0.05f;
        attackSound.functionName = "AttackSound";
        attackSound.objectReferenceParameter = this;

        AnimationEvent executeAttack = new AnimationEvent();
        executeAttack.time = temp.clip.length * temp.damageTime;
        executeAttack.functionName = "ExecuteAttack";
        executeAttack.objectReferenceParameter = this;

        AnimationEvent endAttack = new AnimationEvent();
        endAttack.time = temp.clip.length;
        endAttack.functionName = "EndAttack";
        endAttack.objectReferenceParameter = this;

        temp.clip.events = null;
        temp.clip.AddEvent(attackSound);
        temp.clip.AddEvent(executeAttack);
        temp.clip.AddEvent(endAttack);

        temp.lastDamageTime = temp.damageTime;
        temp.lastSlowModeDuration = temp.slowModeDuration;

        weaponSetDict[type] = temp;
    }

    public void ApplyInspectorValues()
    {
        weaponSetDict.Clear();
        weaponSetDictKeys.Clear();
        foreach (var weaponAnim in weaponSets)
        {
            var newWeaponAnim = weaponAnim;
            var allClips = newWeaponAnim.overrideController.animationClips;
            foreach (var clip in allClips)
            {
                if (string.Equals(clip.name, newWeaponAnim.clip.name))
                    newWeaponAnim.clip = clip;
            }
            weaponSetDict[newWeaponAnim.weaponType] = newWeaponAnim;
        }
        weaponSetDictKeys = weaponSetDict.Keys.ToList();
        SetAll();
    }

    public void StartAttack()
    {
        if (currWeaponType == WeaponTypes.None)
            return;
        playerAnimator.SetBool("IsAttacking", true);
    } 
    public void EndAttack() => playerAnimator.SetBool("IsAttacking", false);

    public void ExecuteAttack()
    {
        var effect = GameManager.instance.effectManager.GetEffect("Sword Slash 1");
        var effectPos = transform.position;
        effect.transform.position = new Vector3(effectPos.x, effectPos.y + 1f, effectPos.z);
        effect.transform.forward = transform.forward;
        attackBox.ExecuteAttack();
    }

    public void StartSlowMode()
    {
        Time.timeScale = slowModeSpeed;
        StartCoroutine(CoEndSlowMode());
    }

    private IEnumerator CoEndSlowMode()
    {
        yield return new WaitForSeconds(weaponSetDict[currWeaponType].slowModeDuration * slowModeSpeed);
        Time.timeScale = 1f;
    }

    public void AttackTarget(GameObject target, Vector3 attackPos) => basicAttack.ExecuteAttack(gameObject, target, attackPos);

    public void AttackSound() => SoundManager.instance.PlaySoundEffect(weaponSetDict[currWeaponType].attackSound);
    public void HitSound() => SoundManager.instance.PlaySoundEffect(weaponSetDict[currWeaponType].hitSound);
}
