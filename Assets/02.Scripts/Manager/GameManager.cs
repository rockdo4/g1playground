using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<GameManager>();
            return m_instance;
        }
    }

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    public void Respawn()
    {
        SceneManager.LoadScene(PlayerDataManager.instance.lastMapId);
      
        StartCoroutine(CoRespawn());
    }

    IEnumerator CoRespawn()
    {
        yield return null;
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
        Debug.Log(PlayerDataManager.instance.lastPlayerPos);
    }
}
