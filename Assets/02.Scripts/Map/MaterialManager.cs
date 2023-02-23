using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] private Material customMaterial;
    [SerializeField] private Material originMaterial;


    private static MaterialManager m_instance;
    public static MaterialManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MaterialManager>();
            }

            return m_instance;
        }
    }

    private void Awake()
    {
        //singleton
        if (instance != this)
        {

            Destroy(gameObject);
        }
    }

    private void ChangeMaterial(MeshRenderer[] meshRenderer)
    {

    }
}
