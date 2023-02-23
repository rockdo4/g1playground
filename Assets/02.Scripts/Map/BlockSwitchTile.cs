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
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (isActive)
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
                }
                else
                {
                    block.SetActive(true);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Triggers Switch when pushed by Player object or Pushable objects
        if (!animator.GetBool("Trigger") && (other.tag == "Player" || other.tag == "Pushable"))
        {
            //Debug.Log(other.tag);
            animator.SetBool("Trigger", true);
            isActive = true;
            SetBlocks();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (animator.GetBool("Trigger") && type == BSwitchType.Temporary)
        {
            animator.SetBool("Trigger", false);
            isActive = false;
            SetBlocks();
        }

    }
}
