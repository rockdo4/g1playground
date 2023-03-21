using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTile : MonoBehaviour
{
    public bool Istriggered { get; set; }
    [SerializeField] private float damagePercentage = 20f;
    [SerializeField] private float delay = 2f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        var status = other.GetComponent<Status>();
        if (status != null && timer >= delay) 
        {
            timer = 0f;

            //add player damage here//
            status.CurrHp -= (int)damagePercentage;
        }
    }
}
