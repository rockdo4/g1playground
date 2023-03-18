using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : ObjectTile
{
    public enum Direction
    {
        Left,
        Right,
    }

    [SerializeField] private float speed = 0.5f;
    [SerializeField] private Direction direction;

    private List<Rigidbody> movingObjects = new List<Rigidbody>();    
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        SetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingObjects.Count > 0) 
        {
            foreach (var massObj in movingObjects)
            {
                if (Mathf.Abs(massObj.velocity.x) <= 7f)
                {
                    massObj.velocity += new Vector3(dir.x * speed, 0, 0);
                }
                
            }
        }
        
    }

    private void OnDisable()
    {
        movingObjects.Clear();
    }

    public override void ResetObject()
    {
        movingObjects.Clear();
    }

    private void SetDirection()
    {
        switch (direction)
        {
            case Direction.Left:
                dir = Vector3.left;
                break;
            case Direction.Right:
                dir = Vector3.right;
                break;
        }
        //Debug.Log(dir);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            //collision.rigidbody.velocity = new Vector3(dir.x * speed, 0, 0);
            movingObjects.Add(collision.rigidbody);
        }        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (movingObjects.Contains(collision.rigidbody))
        {
            movingObjects.Remove(collision.rigidbody);
        }
    }
}
