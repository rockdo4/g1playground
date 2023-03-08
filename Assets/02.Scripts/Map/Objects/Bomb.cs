using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    private float timer = 0f;
    private bool isTriggered = false;



    // Start is called before the first frame update
    void Start()
    {
        TileColorManager.instance.ToInvisibleMaterial(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TileColorManager.instance.ToOriginalMaterial(gameObject);
            isTriggered = true;
        }
    }
}
