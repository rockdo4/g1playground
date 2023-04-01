    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTile : MonoBehaviour
{
    public bool Istriggered { get; set; }
    private float damagePercentage = 0.2f;
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
        var status = other.GetComponent<PlayerStatus>();
        if (status != null && timer >= delay) 
        {
            timer = 0f;

            //add player damage here//
            status.CurrHp -= (int)(status.FinalValue.maxHp * damagePercentage);
            var cc = other.GetComponent<AttackedCC>();
            if (cc != null)
                cc.ExeKnockBack(transform.position, 7f);
        }
    }
}
