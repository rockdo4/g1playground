using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Cinemachine.CinemachineConfiner confiner;

    // Start is called before the first frame update
    private void Awake()
    {
        confiner = GetComponent<Cinemachine.CinemachineConfiner>();
    }

    public void SetCollider(PolygonCollider2D collider)
    {
        confiner.m_BoundingShape2D = collider;
    }
}
