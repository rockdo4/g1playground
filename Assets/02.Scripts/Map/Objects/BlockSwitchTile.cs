using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockSwitchTile : MonoBehaviour, IResetObject
{
    private enum BSwitchType
    {
        Permanent,
        Temporary
    }

    private struct BlocksOriginStatus
    {
        public GameObject blockObject;
        public bool isOn;
    }

    private Animator animator;
    [SerializeField] private GameObject[] blocks;
    private List<BlocksOriginStatus> originBlockStatus = new List<BlocksOriginStatus>();

    [SerializeField] private BSwitchType type;

    private bool isTriggered;
    public bool IsTriggered { get { return isTriggered; } set { isTriggered = this; } }

    private bool originTrigger;

    //objects that stepping on the switch
    private List<GameObject> objects = new List<GameObject>();

    public void ResetObject()
    {
        Debug.Log("switch");
    }

    private void Awake()
    {
        originTrigger = false;
        animator = GetComponent<Animator>();
        SaveStatus();
    }

    private void SaveStatus()
    {      
       foreach (var block in blocks)
        {
            BlocksOriginStatus temp = new BlocksOriginStatus();
            temp.blockObject = block;

            if (block.activeSelf)
            {
                temp.isOn = true;

            }
            else
            {
                temp.isOn = false;
            }
            originBlockStatus.Add(temp);
        }              
    }
    
    private void Resetblock()  
    { 
        foreach(var block in originBlockStatus)
        {
            block.blockObject.SetActive(block.isOn);
        }
    }


    private void OnEnable()
    {
        
        objects.Clear();
        IsTriggered = false;
        animator.SetBool("Trigger", false);
        Resetblock();
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

    public void ActiveSelfCheck()
    {

    }
}
