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

    // Start is called before the first frame update
    void Start()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
    }

    // Update is called once per frame
    void Update()
    {
        if (massA > massB)
        {
            Debug.Log("A");
            timer = 0f;
            blockA.transform.Translate(Vector3.down * speed);
            blockB.transform.Translate(Vector3.up * speed);
            blockA.GetComponent<WeightScaler>().IsMovDown = true;
            blockB.GetComponent<WeightScaler>().IsMovUp = true;
        }
        else if (massA < massB)
        {
            Debug.Log("B");
            timer = 0f;
            blockA.transform.Translate(Vector3.up * speed);
            blockB.transform.Translate(Vector3.down * speed);
            blockA.GetComponent<WeightScaler>().IsMovUp = true;
            blockB.GetComponent<WeightScaler>().IsMovDown = true;
        }
        else
        {
            timer += Time.deltaTime;
            
            ResetBlockPosition();
        }

    }

    private void FixedUpdate()
    {
        massA = blockA.GetComponent<WeightScaler>().CalculatedMass;
        massB = blockB.GetComponent<WeightScaler>().CalculatedMass;
    }

    private void MoveBlock()
    {

    }

    private void ResetBlockPosition()
    {
        if (blockA.transform.position.y != blockB.transform.position.y && timer >= stopTime) 
        {
            Debug.Log("AB");

            if (blockA.transform.position.y > blockB.transform.position.y)
            {
                blockA.transform.Translate(Vector3.down * speed);
                blockB.transform.Translate(Vector3.up * speed);
                blockA.GetComponent<WeightScaler>().IsMovDown = true;
                blockB.GetComponent<WeightScaler>().IsMovUp = true;
            }
            else
            {
                blockA.transform.Translate(Vector3.up * speed);
                blockB.transform.Translate(Vector3.down * speed);
                blockA.GetComponent<WeightScaler>().IsMovUp = true;
                blockB.GetComponent<WeightScaler>().IsMovDown = true;
            }
        }
        else
        {
            blockA.GetComponent<WeightScaler>().IsMovDown = false;
            blockA.GetComponent<WeightScaler>().IsMovUp = false;
            blockB.GetComponent<WeightScaler>().IsMovDown = false;
            blockB.GetComponent<WeightScaler>().IsMovUp = false;
        }
    }
}
