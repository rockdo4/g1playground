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

    public PlayerController playerController;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

    //player respawn func
    //public void Respawn()
    //{
    //    SceneManager.LoadScene(PlayerDataManager.instance.lastMapId);
    //    StartCoroutine(CoRespawn());
    //}

    //IEnumerator CoRespawn()
    //{
    //    yield return null;
    //    var player = GameObject.FindGameObjectWithTag("Player");
    //    if (player != null) { 
    //        player.transform.position = PlayerDataManager.instance.lastPlayerPos;
    //        //Todo: Fill player HP
    //        //Todo: Fill player MP
    //    }

    //}
}
