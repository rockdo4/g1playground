using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreyTile : MonoBehaviour
{
    [SerializeField] private Material customMaterial;
    private Material originMaterial;

    //MeshRenderers in objects
    private MeshRenderer[] meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponentsInChildren<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.Log("null1");
        }
        else
        {
            //set original material of tile
            originMaterial = meshRenderer[0].material;

            //set all blocks to customMaterial
            foreach (var mat in meshRenderer)
            {
                mat.material = customMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) 
        {

            ChangeTileMaterial();
        }
    }

    //change Tiles to its original Material
    private void ChangeTileMaterial()
    {
        foreach (var mat in meshRenderer)
        {
            mat.material = originMaterial;
        }
    }
}
