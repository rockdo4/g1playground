using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTile : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private GameObject triggerObject;
    [SerializeField] private bool isTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsTrigger", isTrigger);
        if (triggerObject.GetComponent<ITriggerObject>() != null)
        {
            triggerObject.GetComponent<ITriggerObject>().SetObjectTrigger(isTrigger);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<ObjectMass>() != null)
        {
            isTrigger = !isTrigger;
            animator.SetBool("IsTrigger", isTrigger);
            if (triggerObject.GetComponent<ITriggerObject>() != null)
            {
                triggerObject.GetComponent<ITriggerObject>().SetObjectTrigger(isTrigger);
            }
        }
        
    }
}
