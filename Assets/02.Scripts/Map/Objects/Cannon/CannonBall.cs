using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CannonBall : MonoBehaviour
{
    public IObjectPool<CannonBall> cannonPool { get; set; }

    private GameObject effect;
    [SerializeField] private string effectName = "Fire_explosion_earth";

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("Pushable"))
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            cannonPool.Release(this);

            //set explosion effect
            effect = GameManager.instance.effectManager.GetEffect(effectName);
            effect.transform.position = transform.position;
            float destroyTime = effect.GetComponent<ParticleSystem>().main.duration;
            GameManager.instance.effectManager.ReturnEffectOnTime(effectName, effect, destroyTime);
        }
    }
}
