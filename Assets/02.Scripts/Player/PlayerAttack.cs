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
    public struct WeaponAnim
    {
        public WeaponTypes weaponType;
        public AnimationClip clip;
        public float attackSpeed;
        public float damageTime;
        public float slowModeDuration;
        [NonSerialized] public float lastDamageTime;
        [NonSerialized] public float lastSlowModeDuration;
    }
    private Animator playerAnimator;
    public BasicAttack basicAttack;
    public PlayerAttackBox attackBox;
    public WeaponTypes currWeaponType;
    public float slowModeSpeed;
    [SerializeField] public WeaponAnim[] weaponAnims;
    private Dictionary<WeaponTypes, WeaponAnim> weaponAnimDict = new Dictionary<WeaponTypes, WeaponAnim>();
    private List<WeaponTypes> weaponAnimDictKeys;

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
        weaponAnimDictKeys = weaponAnimDict.Keys.ToList();
        foreach (var key in weaponAnimDictKeys)
        {
            SetDamageTime(key);
        }
    }

    private void Update()
    {
        playerAnimator.SetFloat("AttackSpeed", weaponAnimDict[currWeaponType].attackSpeed); // set weapon temporarily, need to set current weapon's attackSpeed
        foreach (var key in weaponAnimDictKeys)
        {
            SetDamageTime(key);
        }
    }

    public void SetDamageTime(WeaponTypes type)
    {
        WeaponAnim temp = weaponAnimDict[type];
        if (!(Mathf.Approximately(temp.lastDamageTime, temp.damageTime) &&
            Mathf.Approximately(temp.lastSlowModeDuration, temp.slowModeDuration)))
        {
            AnimationEvent executeAttack = new AnimationEvent();
            executeAttack.time = temp.clip.length * temp.damageTime;
            executeAttack.functionName = "ExecuteAttack";
            executeAttack.objectReferenceParameter = this;

            AnimationEvent endAttack = new AnimationEvent();
            endAttack.time = temp.clip.length;
            endAttack.functionName = "EndAttack";
            endAttack.objectReferenceParameter = this;

            temp.clip.events = null;
            temp.clip.AddEvent(executeAttack);
            temp.clip.AddEvent(endAttack);

            temp.lastDamageTime = temp.damageTime;
            temp.lastSlowModeDuration = temp.slowModeDuration;
        }
        weaponAnimDict[type] = temp;
    }

    public void StartAttack() => playerAnimator.SetBool("IsAttacking", true);
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
        yield return new WaitForSeconds(weaponAnimDict[currWeaponType].slowModeDuration * slowModeSpeed);
        Time.timeScale = 1f;
    }
    public void AttackTarget(GameObject target, Vector3 attackPos) => basicAttack.ExecuteAttack(gameObject, target, attackPos);
}
