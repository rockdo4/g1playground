using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTile : MonoBehaviour, IResetObject
{
    private Animator animator;

    [SerializeField] private GameObject triggerObject;
    [SerializeField] private bool isTrigger = false;

    private bool originTrigger;

    public void ResetObject()
    {
        isTrigger = originTrigger;
        animator.SetBool("IsTrigger", isTrigger);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        originTrigger = isTrigger;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator.SetBool("IsTrigger", isTrigger);
        //if (triggerObject.GetComponent<ITriggerObject>() != null)
        //{
        //    triggerObject.GetComponent<ITriggerObject>().SetObjectTrigger(isTrigger);
        //}
        
    }

    private void OnEnable()
    {
        isTrigger = originTrigger;
        animator.SetBool("IsTrigger", isTrigger);
        
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
