using System.Collections;
using System.Collections.Generic;
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
    public float Timer { get { return timer; } set { timer = 0f; } }


    // Start is called before the first frame update
    void Start()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer >= stopTime)
        {
            if (massA > massB )
            {
                //Debug.Log("A");
                //timer = 0f;
                BlockBUp();
            }
            else if (massA < massB )
            {
                //Debug.Log("B");
                //timer = 0f;
                BlockAUp();
            }
            else
            {
                ResetBlockPosition();
            }
        }
        

    }

    private void FixedUpdate()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
    }

    private void BlockAUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble && blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            blockA.transform.Translate(Vector3.up * speed);
            blockB.transform.Translate(Vector3.down * speed);
            blockA.GetComponent<WeightScaler>().IsMovUp = true;
            blockB.GetComponent<WeightScaler>().IsMovDown = true;
        }
    }

    private void BlockBUp()
    {
        if (blockA.GetComponent<WeightScaler>().IsMovAble && blockB.GetComponent<WeightScaler>().IsMovAble)
        {
            blockA.transform.Translate(Vector3.down * speed);
            blockB.transform.Translate(Vector3.up * speed);
            blockA.GetComponent<WeightScaler>().IsMovDown = true;
            blockB.GetComponent<WeightScaler>().IsMovUp = true;
        }
        

    }

    private void ResetBlockPosition()
    {
        if (Mathf.Approximately(blockA.transform.position.y, blockB.transform.position.y))
        {
            timer = 0f;
            Debug.Log("AB");
            blockA.GetComponent<WeightScaler>().IsMovDown = false;
            blockA.GetComponent<WeightScaler>().IsMovUp = false;
            blockB.GetComponent<WeightScaler>().IsMovDown = false;
            blockB.GetComponent<WeightScaler>().IsMovUp = false;
        }
        if (blockA.transform.position.y != blockB.transform.position.y)
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
}
