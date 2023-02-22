using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructed : MonoBehaviour, IDestructible
{
    public void OnDestroy()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
        //objectpool return
    }
}
