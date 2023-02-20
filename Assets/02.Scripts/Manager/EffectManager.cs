using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject[] effectPrefabs;
    private Dictionary<string, Queue<GameObject>> effectPool = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        foreach (GameObject effectPrefab in effectPrefabs)
        {
#if UNITY_EDITOR
            if (effectPrefab == null)
            {
                Debug.Log("Please Input Effect");
                return;
            }
#endif

            string effectName = effectPrefab.name;
            effectPool[effectName] = new Queue<GameObject>();
        }
    }

    public GameObject GetEffect(string effectName)
    {
        GameObject effect = null;

        if (!effectPool.ContainsKey(effectName))
        {
#if UNITY_EDITOR
            Debug.Log("Incorrect String");
#endif
            return null;
        }

        if (effectPool.ContainsKey(effectName) && effectPool[effectName].Count > 0)
        {
            effect = effectPool[effectName].Dequeue();
            effect.SetActive(true);
        }
        else
        {
            foreach (GameObject effectPrefab in effectPrefabs)
            {
                if (effectPrefab.name == effectName)
                {
                    effect = Instantiate(effectPrefab);
                    break;
                }
            }
        }

        return effect;
    }


    public void ReturnEffect(string effectName, GameObject effect)
    {
        if (!effectPool.ContainsKey(effectName))
        {
#if UNITY_EDITOR
            Debug.Log("Incorrect String");
#endif
            return;
        }

        effect.SetActive(false);
        effectPool[effectName].Enqueue(effect);
    }


    public void ReturnEffectOnTime(string effectName, GameObject effect, float destroyTime)
    {
        StartCoroutine(DelaySetfalse(effectName, effect, destroyTime));
    }

    private IEnumerator DelaySetfalse(string effectName, GameObject effect, float destroyTime)
    {
        if (!effectPool.ContainsKey(effectName))
        {
#if UNITY_EDITOR
            Debug.Log("Incorrect String");
#endif
        }

        yield return new WaitForSeconds(destroyTime);

        effect.SetActive(false);
        effectPool[effectName].Enqueue(effect);
    }

}