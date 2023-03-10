using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDungeon : MonoBehaviour
{
    private GameObject dungeon;
    // Start is called before the first frame update
    private void OnEnable()
    {
        dungeon = GameObject.FindWithTag("Dungeon").gameObject;
     

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dungeon.GetComponent<DungeonManager>().DungeonDay.gameObject.SetActive(true);


        }
    }
}
