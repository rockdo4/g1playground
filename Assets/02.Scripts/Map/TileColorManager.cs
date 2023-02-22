using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorManager : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material customMaterial;
    [SerializeField] private Material originMaterial;
    public new string name;

    private static TileColorManager m_instance;
    public static TileColorManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<TileColorManager>();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           
            ChangeTileMaterial(name, true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            ChangeTileMaterial(name, false);
        }
    }

    public void ChangeTileMaterial(string name, bool isColor)
    {
        var stages = GameObject.FindGameObjectsWithTag("Stage");
        
        foreach (var stage in stages)
        {
            //Debug.Log(stage.name);
            if (stage.name == name)
            {

                if (isColor)
                {
                    ToOriginalMaterial(stage);
                }
                else
                {
                    ToCustomMaterial(stage);
                }
            }
            
        }
    }

    private void ToOriginalMaterial(GameObject stage)
    {
        var tiles = stage.GetComponentsInChildren<LODGroup>();
        Debug.Log(tiles.Length);

        foreach (var tile in tiles)
        {
            var meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = originMaterial;
            }
        }
    }

    private void ToCustomMaterial(GameObject stage)
    {
        var tiles = stage.GetComponentsInChildren<LODGroup>();
        Debug.Log(tiles.Length);

        foreach (var tile in tiles)
        {
            var meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = customMaterial;
            }
        }
    }
   
}
