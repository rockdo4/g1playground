using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testsearch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var collider = GameObject.Find(MapManager.instance.GetCurrentMapName());

        if (collider.GetComponentInChildren<PolygonCollider2D>() != null)
        {
            Debug.Log("collider");
        }
        //camera.GetComponent<FollowCamera>().SetCollider()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
