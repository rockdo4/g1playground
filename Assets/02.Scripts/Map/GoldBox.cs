using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBox : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isFirst = false;
    private void OnEnable()
    {
      
    }

    //IEnumerator CSetIsFirst()
    //{
    //    yield return null;
    //    yield return null;

    //    isFirst = MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().IsClear;
    //    Debug.Log($"goldbox{isFirst}");
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var currentStage = MapManager.instance.GetCurrentStageObject().GetComponent<StageController>();

            Debug.Log($"IsFirst is {isFirst}");
            currentStage.SetReward(isFirst);
            transform.gameObject.SetActive(false);
        }
    }
}
