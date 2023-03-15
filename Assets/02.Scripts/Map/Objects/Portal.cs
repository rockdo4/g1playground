using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Portal : MonoBehaviour
{
    private static bool init = false;
    [SerializeField]
    private GameObject nextStage;
    [SerializeField]
    private GameObject pos;
    public bool CanUse { get; set; }
    public string GetNextStageName()
    {
        return nextStage.name;
    }

    public Vector3 GetPos()
    {
        return pos.transform.position;
    }
    private void OnEnable()
    {
        CanUse = true;
        if (!init)
        {

            CanUse = true;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!CanUse&& other.GetComponent<ObjectMass>() != null)
        {
            CanUse = false;
            return;
        }

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
                    other.gameObject.transform.position = portal.GetPos();
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    portal.CanUse = true;

                    CanUse = true;
                    Camera.main.transform.position = portal.gameObject.transform.position;
                    MapManager.instance.SetCurrentMapName(portal.transform.parent.name);
                    MapManager.instance.SetcurrentChapterName(portal.transform.parent.parent.name);

                    transform.parent.gameObject.SetActive(false);

                    break;
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

    private void OnDisable()
    {
        CanUse = true;
        Debug.Log("leb");
    }
}