using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSwitchTile : MonoBehaviour
{
    private enum BSwitchType
    {
        Permanent,
        Temporary
    }

    private Animator animator;
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private BSwitchType type;
    [SerializeField] private float fadeTimer = 0.5f;

    private bool isTriggered;
    public bool IsTriggered { get { return isTriggered; } set { isTriggered = this; } }

    //private static bool isState = true;

    
    void Start()
    {
        animator = GetComponent<Animator>();        
    }

    public void SetBlocks()
    {
        if (blocks != null)
        {
            foreach (var block in blocks)
            {
                if (block.activeSelf)
                {
                    block.SetActive(false);
                }
                else
                {
                    block.SetActive(true);
                }
            }

        }
       // Debug.Log("Block");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        //Triggers Switch when pushed by Player object or Pushable objects
        if (!animator.GetBool("Trigger") && (other.tag == "Player" || other.tag == "Pushable")) 
        {
            //isState = false;
            //Debug.Log(other.tag);
            IsTriggered = true;
            animator.SetBool("Trigger", true);
            SetBlocks();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (animator.GetBool("Trigger") && type == BSwitchType.Temporary)
        {
            //isState = true;
            IsTriggered = false;
            animator.SetBool("Trigger", false);
            SetBlocks();
        }

    }
}
