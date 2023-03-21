using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [Header("MiniMap UI")]
    [SerializeField] private Image miniMapVertical;
    [SerializeField] private Image miniMapHorizontal;

    private static MiniMap m_instance;
    public static MiniMap instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MiniMap>();
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

    // Start is called before the first frame update
    void Start()
    {
        MiniMapData data = null;
        data = DataTableMgr.GetTable<MiniMapData>().Get(0.ToString());

        var image = data.miniMapSprite.bounds.size;

        if (image.x >= image.y)
        {
            miniMapVertical.gameObject.SetActive(true);
            miniMapHorizontal.gameObject.SetActive(false);
            miniMapVertical.sprite = data.miniMapSprite;
            
        }
        else
        {
            miniMapHorizontal.gameObject.SetActive(true);
            miniMapVertical.gameObject.SetActive(false);
            miniMapHorizontal.sprite = data.miniMapSprite;            
        }
        
    }

    public void SetMiniMap(int id)
    {
        MiniMapData data = null;
        data = DataTableMgr.GetTable<MiniMapData>().Get(id.ToString());

        var image = data.miniMapSprite.bounds.size;

        if (image.x >= image.y)
        {
            miniMapVertical.gameObject.SetActive(true);
            miniMapHorizontal.gameObject.SetActive(false);
            miniMapVertical.sprite = data.miniMapSprite;            
        }
        else
        {
            miniMapHorizontal.gameObject.SetActive(true);
            miniMapVertical.gameObject.SetActive(false);
            miniMapHorizontal.sprite = data.miniMapSprite;
            
        }
    }
}
