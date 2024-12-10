using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileColorManager : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material customMaterial;
    [SerializeField] private Material originMaterial;
    [SerializeField] private Material invisibleMaterial;

    [Header("Image")]
    [SerializeField] private Sprite worldMapImageRed;
    [SerializeField] private Sprite worldMapImageGreen;
    [SerializeField] private Sprite worldMapImageOrange;
    [SerializeField] private Sprite worldMapImagePurple;
    [SerializeField] private Sprite worldMapImageBrown;

    public enum ImageColor
    {
        Red,
        Green,
        Orange,
        Purple,
        Brown,
    }


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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.O))
    //    {

    //        ChangeTileMaterial(name, true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {

    //        ChangeTileMaterial(name, false);
    //    }
    //}

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

    public void ToOriginalMaterial(GameObject stage)
    {
        var tiles = stage.GetComponentsInChildren<LODGroup>();
        ///Debug.Log(tiles.Length);

        foreach (var tile in tiles)
        {
            var meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = originMaterial;
            }
        }
    }

    public void ToCustomMaterial(GameObject stage)
    {
        var tiles = stage.GetComponentsInChildren<LODGroup>();
        //Debug.Log(tiles.Length);

        foreach (var tile in tiles)
        {
            var meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = customMaterial;
            }
        }
    }

    public void ToInvisibleMaterial(GameObject gameObject)
    {
        var tiles = gameObject.GetComponentsInChildren<LODGroup>();
        ///Debug.Log(tiles.Length);

        foreach (var tile in tiles)
        {
            var meshRenderers = tile.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshRenderers)
            {
                mesh.material = invisibleMaterial;
            }
        }
    }

    public Material GetOriginMaterial()
    {
        return originMaterial;
    }

    public Material GetCustomMaterial()
    {
        return customMaterial;
    }

    public Sprite GetImageSprite(ImageColor color)
    {
        switch (color)
        {
            case ImageColor.Red:
                return worldMapImageRed;
            case ImageColor.Green:
                return worldMapImageGreen;
            case ImageColor.Orange:
                return worldMapImageOrange;
            case ImageColor.Purple:
                return worldMapImagePurple;
            case ImageColor.Brown:
                return worldMapImageBrown;
        }

        return null;
    }
}
