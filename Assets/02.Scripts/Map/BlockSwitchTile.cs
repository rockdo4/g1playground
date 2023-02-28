using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    bool isActive = false;

    private bool isTriggered;
    public bool IsTriggered { get { return isTriggered; } set { isTriggered = this; } }

    //private static bool isState = true;
  
    void Start()
    {
        animator = GetComponent<Animator>();        
    }


    private void OnEnable()
    {
        if (isTriggered)
        {
            animator.SetBool("Trigger", true);
        }
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
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    block.SetActive(true);
                    //gameObject.GetComponent<BoxCollider>().enabled = false;
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
