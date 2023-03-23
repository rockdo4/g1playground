using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes;

    private static SkyboxManager m_instance;
    public static SkyboxManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SkyboxManager>();
            }

            return m_instance;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        //singleton
        if (instance != this)
        {

            Destroy(gameObject);
        }

        //if (skyboxes != null)
        //{
        //    RenderSettings.skybox = skyboxes[0];
        //}
    }

    public void SetSkybox(int index)
    {
        if (!(index < 0) && skyboxes.Length > index)
        {
            RenderSettings.skybox = skyboxes[index];
        }
        else
        {
            Debug.Log("index out of range");
        }
    }
   
}
