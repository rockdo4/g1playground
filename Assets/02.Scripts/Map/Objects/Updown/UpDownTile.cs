using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UpDownTile : MonoBehaviour, IResetObject
{
    public enum State
    {
        BlockA,
        BlockB,
        Equal,
    }

    [SerializeField] private GameObject blockA;
    [SerializeField] private GameObject blockB;

    [SerializeField] private float massA;
    [SerializeField] private float massB;

    [SerializeField] private float speed = 1f;
    
    [SerializeField] private float stopTime = 0.5f;
    private float timer;

    private Vector3 originPos;
    private Vector3 blockAPos;
    private Vector3 blockBPos;

    private void Awake()
    {
        originPos = transform.position;
        blockAPos = blockA.transform.position;
        blockBPos = blockB.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
    }

    private void FixedUpdate()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;

        timer += Time.fixedDeltaTime;

        if (timer >= stopTime)
        {
            if (Mathf.Approximately(massA, massB))
            {
                SetState(State.Equal);
            }
            else
            {
                if (massA > massB)
                {
                    SetState(State.BlockA);
                }
                else
                {
                    SetState(State.BlockB);
                }
            }
  
        }
    }

    private void OnEnable()
    {
        transform.position = originPos;
        blockA.transform.position = blockAPos;
        blockB.transform.position = blockBPos;
    }

    private void SetState(State state)
    {
        //currState = state;

        switch (state)
        {
            case State.BlockA:
                BlockBUp();
                break;
            case State.BlockB:
                BlockAUp();
                break;
            case State.Equal:
                ResetBlockPosition();
                break;
            
        }
    }

    //Move BlockA up BlockB Down
    private void BlockAUp()
    {
        if (blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();
            
            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.up * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.down * speed * Time.fixedDeltaTime);

            //moveAObjects();
        }
        
    }

    //Move BlockB up BlockA Down
    private void BlockBUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble)
        {
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();
            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.down * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.up * speed * Time.fixedDeltaTime);

            //moveBObjects();
        }

    }

    private void ResetBlockPosition()
    {
        //Debug.Log("equal");
        blockA.GetComponent<WeightScaler>().IsMovAble = true;
        blockB.GetComponent<WeightScaler>().IsMovAble = true;

        blockA.GetComponent<WeightScaler>().EnableTrigger();
        blockB.GetComponent<WeightScaler>().EnableTrigger();

        if (Mathf.Approximately(blockA.transform.position.y, blockB.transform.position.y))
        {
            timer = 0f;

            return;
        }
        else
        {
            if (blockA.transform.position.y > blockB.transform.position.y)
            {
                
                BlockBUp();
            }
            else if (blockA.transform.position.y < blockB.transform.position.y)
            { 
                BlockAUp();
            }
        }
    }

    public void ResetObject()
    {
        transform.position = originPos;
        blockA.transform.position = blockAPos;
        blockB.transform.position = blockBPos;
        blockA.GetComponent<WeightScaler>().ResetWeight();
        blockB.GetComponent<WeightScaler>().ResetWeight();
    }
}
