using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTile : ObjectTile
{
    private Animator animator;

    [SerializeField] private GameObject triggerObject;
    [SerializeField] private bool isTrigger = false;

    private bool originTrigger;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        originTrigger = isTrigger;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator.SetBool("IsTrigger", isTrigger);
    }

    protected override void OnEnable()
    {
        isTrigger = originTrigger;
        animator.SetBool("IsTrigger", isTrigger);
        
    }

    public override void ResetObject()
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
