using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    [SerializeField]
    private float upSpeed = 1f;
    private void OnEnable()
    {
        GameManager.instance.effectManager.ReturnEffectOnTime(transform.name,gameObject, 3);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,transform.position.y+upSpeed * Time.deltaTime);
    }

}
