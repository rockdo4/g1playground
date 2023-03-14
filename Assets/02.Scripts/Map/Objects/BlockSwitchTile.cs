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
    //[SerializeField] private float fadeTimer = 0.5f;

    //bool isActive = false;

    private bool isTriggered;
    public bool IsTriggered { get { return isTriggered; } set { isTriggered = this; } }

    private List<GameObject> objects = new List<GameObject>();
  
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
        //Triggers Switch when pushed by ObjectMass objects
        if (!animator.GetBool("Trigger") && (other.GetComponent<ObjectMass>() != null)) 
        {
            objects.Add(other.gameObject);
            IsTriggered = true;
            animator.SetBool("Trigger", true);
            SetBlocks();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (animator.GetBool("Trigger") && type == BSwitchType.Temporary)
        {
            if (objects.Contains(other.gameObject))
            {
                objects.Remove(other.gameObject);
            }

            if (objects.Count <= 0)
            {
                //Debug.Log("exit");
                //isState = true;
                IsTriggered = false;
                animator.SetBool("Trigger", false);
                SetBlocks();
            }
            
        }

    }
}
