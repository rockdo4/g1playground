using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class Portal : MonoBehaviour
{
    private static bool init = false;
    [SerializeField]
    private GameObject nextStage;
    [SerializeField]
    private GameObject pos;
    public bool CanUse { get; set; }

    [SerializeField] private string enterPortalClip = "Stone Debris 3_5";

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
            bool prevAuto = player.IsAuto;
            var playerController = player.GetComponent<PlayerController>();
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            var linkMover = player.GetComponent<AgentLinkMover>();
            playerController.autoToggle.isOn = false;
            playerController.AgentOff();
            playerController.AgentOnOff();
            player.GetComponent<PlayerInventory>().RefillPotions();
            player.transform.SetParent(null);

            if (agent.isOnOffMeshLink)
            {
                agent.ResetPath();
                linkMover.enabled = false;
                linkMover.enabled = true;
            }
            else if (agent.isOnNavMesh && !agent.currentOffMeshLinkData.valid)
            {
                agent.ResetPath();
            }
            player.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
            init = true;
            nextStage.gameObject.SetActive(true);
            //player.RemoveAgentLinkMover();
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
                    if (prevAuto)
                    {
                        player.autoToggle.isOn = true;
                        player.AgentOnOff();
                    }

                    

                    transform.parent.gameObject.SetActive(false);
                  

                    SoundManager.instance.PlaySoundEffect(enterPortalClip);

                    break;
                }
            }
            //player.AddAgentLinkMover();
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
        if (GameManager.instance.player.GetComponent<PlayerController>().IsAuto)
        {
            StopCoroutine(CClosePortalInaWhile());
            CanUse = true;

            return;
        }

        CanUse = false;
        StopCoroutine(CClosePortalInaWhile());
        StartCoroutine(CClosePortalInaWhile());
    }

    IEnumerator CClosePortalInaWhile()
    {
        yield return new WaitForSeconds(2f);
        CanUse = true;
    }

    private void OnDisable()
    {
        CanUse = true;
    }
}