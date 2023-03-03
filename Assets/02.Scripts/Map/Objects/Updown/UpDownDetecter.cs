using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownDetecter : MonoBehaviour
{
    [SerializeField] private WeightScaler block;
    [SerializeField] private Collider colliderTrigger;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" || other.tag == "Pushable")
        {
            block.IsMovAble = false;
            colliderTrigger.enabled = false;
        }
        if (other.tag == "Player" )
        {
            //Debug.Log(other.tag);
            block.IsMovAble = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Pushable")
        {
            block.IsMovAble = true;

        }
    }

    public void EnableTrigger()
    {
        colliderTrigger.enabled = true;
    }
}
