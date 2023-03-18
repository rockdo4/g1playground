using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTile : MonoBehaviour
{
    protected Vector3 originPos;
  
    protected virtual void OnEnable()
    {
        transform.position = originPos;
    }

    public virtual void ResetObject()
    {
        transform.position = originPos;
    }

}
