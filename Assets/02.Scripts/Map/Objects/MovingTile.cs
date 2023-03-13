using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour, ITriggerObject
{
    private enum FirstMove
    {
        A,
        B,
    }

    [SerializeField] private GameObject movePointA;
    [SerializeField] private GameObject movePointB;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool isTriggered = true;

    [SerializeField] private FirstMove nextDir;

    public void SetObjectTrigger(bool isTrigger)
    {
        isTriggered = isTrigger;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (isTriggered)
        {
            Move(nextDir);
        }
    }

    private void Move(FirstMove move)
    {
        switch (move)
        {
            case FirstMove.A:
                MoveA();
                break;
            case FirstMove.B:
                MoveB();
                break;
        }
    }

    private void MoveA()
    {
        
        if (Vector3.Distance(transform.position, movePointA.transform.position) >= 1f) 
        {
            var dir = movePointA.transform.position - transform.position;
            dir.z = 0;
            dir.Normalize();

            gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.GetComponent<Rigidbody>().position + dir * speed * Time.fixedDeltaTime);
        }
        else
        {
            nextDir = FirstMove.B;
        }
    }

    private void MoveB()
    {
        if (Vector3.Distance(transform.position, movePointB.transform.position) >= 1f)
        {
            var dir = movePointB.transform.position - transform.position;
            dir.z = 0;
            dir.Normalize();

            gameObject.GetComponent<Rigidbody>().MovePosition(gameObject.GetComponent<Rigidbody>().position + dir * speed * Time.fixedDeltaTime);
        }
        else
        {
            nextDir = FirstMove.A;
        }
    }
}
