using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyTrigger : MonoBehaviour
{
    public float damagedDelay;
    private float timer;
    private bool isOnDelay = false;
    public int damage;

    private void Update()
    {
        if (isOnDelay)
        {
            timer += Time.deltaTime;
            if (timer > damagedDelay)
            {
                isOnDelay = false;
                timer = 0f;
            }    
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOnDelay || !other.CompareTag("EnemyBody"))
            return;
    }
}
