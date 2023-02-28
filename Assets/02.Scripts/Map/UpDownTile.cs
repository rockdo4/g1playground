using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class UpDownTile : MonoBehaviour
{
    [SerializeField] private GameObject blockA;
    [SerializeField] private GameObject blockB;

    [SerializeField] private float massA;
    [SerializeField] private float massB;

    [SerializeField] private float speed = 1f;
    public float Speed { get { return speed; } }

    [SerializeField] private float stopTime = 0.5f;
    private float timer;

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
            if (massA > massB)
            {
                BlockBUp();
            }
            else if (massA < massB)
            {
                BlockAUp();
            }
            else
            {
                ResetBlockPosition();
            }
        }

    }

    private void BlockAUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble && blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();

            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.up * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.down * speed * Time.fixedDeltaTime);
            
            moveAObjects();
        }
        
    }

    private void BlockBUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble && blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            Rigidbody rbA = blockA.GetComponent<Rigidbody>();
            Rigidbody rbB = blockB.GetComponent<Rigidbody>();  

            blockA.GetComponent<Rigidbody>().MovePosition(rbA.position + Vector3.down * speed * Time.fixedDeltaTime);
            blockB.GetComponent<Rigidbody>().MovePosition(rbB.position + Vector3.up * speed * Time.fixedDeltaTime);

            moveBObjects();
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
        if (Mathf.Approximately(blockA.transform.position.y, blockB.transform.position.y))
        {
            timer = 0f;
            //Debug.Log("AB");
            blockA.GetComponent<WeightScaler>().IsMovDown = false;
            blockA.GetComponent<WeightScaler>().IsMovUp = false;
            blockB.GetComponent<WeightScaler>().IsMovDown = false;
            blockB.GetComponent<WeightScaler>().IsMovUp = false;

            return;
        }

        if (blockA.transform.position.y != blockB.transform.position.y) 
        {
            blockA.GetComponent<WeightScaler>().IsMovAble = true;
            blockB.GetComponent<WeightScaler>().IsMovAble = true;

            if (Mathf.Abs(blockA.transform.position.y) > Mathf.Abs(blockB.transform.position.y)) 
            {
                
                //Debug.Log("A");
                BlockBUp();
            }
            else if (Mathf.Abs(blockA.transform.position.y) < Mathf.Abs(blockB.transform.position.y))
            {
                //Debug.Log("B");
                BlockAUp();
            }

        }

    }
}
