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
        ClosePortalInaWhile();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!CanUse && other.GetComponent<ObjectMass>() != null)
        {
            CanUse = false;
            return;
        }

        if (!CanUse)
            return;

        if (other.CompareTag("Player") && other.GetComponent<ObjectMass>() != null)
        {
            var player = other.GetComponent<PlayerController>();
            player.AgentOnOff();
            init = true;
            nextStage.gameObject.SetActive(true);
            nextStage.GetComponent<StageController>().PrevStageName = MapManager.instance.GetCurrentMapName();
            var nextstageportals = nextStage.GetComponentsInChildren<Portal>();
            foreach (var portal in nextstageportals)
            {
                if (portal.GetNextStageName() == transform.parent.name)
                {
                    other.gameObject.transform.position = portal.GetPos();
                    other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    var playerStat = other.GetComponent<Status>();
                    playerStat.CurrHp = playerStat.FinalValue.maxHp;
                    playerStat.CurrMp = playerStat.FinalValue.maxMp;
                    other.GetComponent<PlayerInventory>()?.RefillPotions();
                    //other.GetComponent<PlayerController>().AgentOnOff();

                    Camera.main.transform.position = portal.gameObject.transform.position;
                    MapManager.instance.SetCurrentMapName(portal.transform.parent.name);
                    MapManager.instance.SetcurrentChapterName(portal.transform.parent.parent.name);
                    player.AgentOnOff();
                    if (portal.transform.parent.name == "Village")
                    {
                        GameManager.instance.uiManager.PotionOff();
                    }
                    else
                    {
                        GameManager.instance.uiManager.PotionOn();
                    }

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

    public void ClosePortalInaWhile()
    {
        Debug.Log(transform.parent.name);
        CanUse = false;
        Debug.Log("started");
        StopCoroutine(CClosePortalInaWhile());
        StartCoroutine(CClosePortalInaWhile());
    }

    IEnumerator CClosePortalInaWhile()
    {
        Debug.Log("co");

        yield return new WaitForSeconds(2f);
        CanUse = true;
        Debug.Log("true");
    }

    private void OnDisable()
    {
        CanUse = true;
    }
}