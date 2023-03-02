using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UpDownTile : MonoBehaviour
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

    //private State state;
    private State currState;

    // Start is called before the first frame update
    void Start()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
        currState = State.Equal;
    }

    private void FixedUpdate()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;

        timer += Time.fixedDeltaTime;

        if (timer >= stopTime)
        {
            if (massA > massB)
            {
                SetState(State.BlockA);
                //BlockBUp();
            }
            else if (massA < massB)
            {
                SetState(State.BlockB);
                //BlockAUp();
            }
            else
            {
                SetState(State.Equal);
                //ResetBlockPosition();
            }
        }
    }

    private void SetState(State state)
    {
        //if (state == currState)
        //{
        //    Debug.Log("same");
        //    return;
        //}

        currState = state;

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

    private void BlockAUp()
    {
        if (blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            //Debug.Log("A");
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();
            
            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.up * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.down * speed * Time.fixedDeltaTime);

            //moveAObjects();
        }
        
    }

    private void BlockBUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble)
        {
            //Debug.Log("B");
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();
            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.down * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.up * speed * Time.fixedDeltaTime);

            //moveBObjects();
        }

    }

    private void moveAObjects()
    {
        foreach (var rbs in blockA.GetComponent<WeightScaler>().objects)
        {
            rbs.MovePosition(rbs.position + Vector3.up * speed * Time.fixedDeltaTime);       
        }

        foreach (var rbs in blockB.GetComponent<WeightScaler>().objects)
        {
            rbs.MovePosition(rbs.position + Vector3.down * speed * Time.fixedDeltaTime);    
        }
    }

    private void moveBObjects()
    {
        foreach (var rbs in blockA.GetComponent<WeightScaler>().objects)
        {
            rbs.MovePosition(rbs.position + Vector3.down * speed * Time.fixedDeltaTime);            
        }

        foreach (var rbs in blockB.GetComponent<WeightScaler>().objects)
        {
            rbs.MovePosition(rbs.position + Vector3.up * speed * Time.fixedDeltaTime);
        }
    }

    private void ResetBlockPosition()
    {
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
            if (Mathf.Abs(blockA.transform.position.y) > Mathf.Abs(blockB.transform.position.y))
            {

                BlockBUp();
            }
            else if (Mathf.Abs(blockA.transform.position.y) < Mathf.Abs(blockB.transform.position.y))
            {
                BlockAUp();
            }
        }
    }
}
