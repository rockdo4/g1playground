using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject nextStage;
    public bool IsClearroom { get; set; }
    private bool canUse=true;
    public bool CanUse { get; set; }
    public string GetNextStageName()
    {
        return nextStage.name;
    }

    private void OnEnable()
    {
        //CanUse = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CanUse)
            return;
        if (other.CompareTag("Player"))
        {
            nextStage.gameObject.SetActive(true);
            var nextstageportals = nextStage.GetComponentsInChildren<Portal>();
            foreach (var portal in nextstageportals)
            {

                if (portal.GetNextStageName() == transform.parent.name)
                {
                    CanUse = false;
                    portal.CanUse = false;
                    Debug.Log(portal.transform.parent.name);
                    Debug.Log(portal.CanUse);
                    other.gameObject.transform.position = portal.gameObject.transform.position;
                    GameObject.Find(portal.GetNextStageName()).SetActive(false);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanUse = true;
           
        }

    }
}