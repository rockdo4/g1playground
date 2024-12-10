using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlocker : MonoBehaviour
{
    public CapsuleCollider gameObjectCollider;
    public Collider BlockerCollider;
    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(gameObjectCollider, BlockerCollider, true);
    }
}
