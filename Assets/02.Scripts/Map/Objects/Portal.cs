using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Portal : MonoBehaviour
{
    private static bool init = false;
    [SerializeField]
    private GameObject nextStage;
    public bool CanUse { get; set; }
    public string GetNextStageName()
    {
        return nextStage.name;
    }

    private void OnEnable()
    {
        if (!init)
        {
            
            CanUse = true;
        }
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!CanUse)
            return;
        if (other.CompareTag("Player") && other.GetComponent<ObjectMass>() != null)
        {
            init = true;
            nextStage.gameObject.SetActive(true);
            var nextstageportals = nextStage.GetComponentsInChildren<Portal>();
            foreach (var portal in nextstageportals)
            {
                if (portal.GetNextStageName() == transform.parent.name)
                {
                    other.gameObject.transform.position = portal.gameObject.transform.position;    
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    CanUse = false;
                    Camera.main.transform.position=portal.gameObject.transform.position;
                    MapManager.instance.SetCurrentMapName(portal.transform.parent.name);
                    transform.parent.gameObject.SetActive(false);

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