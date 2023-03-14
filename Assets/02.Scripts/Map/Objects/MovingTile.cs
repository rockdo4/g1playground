using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour, ITriggerObject, IResetObject
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

    private bool originIsTrigger;
    private Vector3 originPos;
    private FirstMove originDir;

    public void SetObjectTrigger(bool isTrigger)
    {
        isTriggered = !isTriggered;
    }


    private void Awake()
    {
        originIsTrigger = isTriggered;
        originPos = transform.position;
        originDir = nextDir;
    }

    private void FixedUpdate()
    {
        if (isTriggered)
        {
            Move(nextDir);
        }
    }

    private void OnEnable()
    {
        isTriggered = originIsTrigger;
        transform.position = originPos;
        nextDir = originDir;
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

    public void ResetObject()
    {
        isTriggered = originIsTrigger;
        transform.position = originPos;
        nextDir = originDir;
    }
}
