using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    private Collider enemyBody;

    private void Awake()
    {
        enemyBody = GetComponent<Collider>();
    }
}
