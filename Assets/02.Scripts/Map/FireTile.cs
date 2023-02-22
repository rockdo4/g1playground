using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTile : MonoBehaviour
{
    //give damage to player every hitRate sec
    [SerializeField] private float hitRate = 0.5f;
    [SerializeField] private float damage = 1f;

    private float timer;

    private void Start()
    {
        //set timer to hitRate to give damage immediately
        timer = hitRate;
    }

    //give damage to player when stepped on the tile
    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;

        if (other.tag == "Player" && timer >= hitRate)
        {
            timer = 0f;
            //use player onHit here

            //
            //Debug.Log("Fire");
            //Debug.Log(damage);

        }
    }
}
