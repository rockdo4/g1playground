using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTile : MonoBehaviour
{
    private Animator animator;
    //private AnimationClip animationClip;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("switch");
        if (!animator.GetBool("Trigger") && (other.tag == "Player" || other.tag == "Pushable")) 
        {
            Debug.Log(other.tag);
            animator.SetBool("Trigger", true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (animator.GetBool("Trigger"))
        {
            animator.SetBool("Trigger", false);

        }
        
    }

}
