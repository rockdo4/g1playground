using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class MiniMap : MonoBehaviour
{
    [Header("MiniMap UI")]
    //  [SerializeField] private Image miniMapVertical;
    [SerializeField] private Image miniMapHorizontal;
    [SerializeField] private RectTransform miniMapRect;
    [SerializeField] private Image playerPos;
    private GameObject player;
    private static MiniMap m_instance;
    [SerializeField] private GameObject minimapObject;
    [SerializeField] private RectTransform mask;
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

    private void Update()
    {
        if (MapManager.instance.outlines == null || MapManager.instance.GetCurrentMapName() == "Village") 
        {
            minimapObject.SetActive(false);
            return;
        }
        else
        {
            minimapObject.SetActive(true);
        }
            

        var collider = MapManager.instance.currentMapCollider;
        var boxes = MapManager.instance.outlines;
        var points=collider.points;
        float minX, maxX;
        float minY, maxY;
        Vector2 LB;
        Vector2 RT;
        //for (var i = 0; i < points.Length; i++)
        //{
        //    points[i] = collider.transform.TransformPoint(points[i]);
        //}
        //minX = points[0].x;
        //minY= points[0].y;
        //maxX = points[0].x;
        //maxY = points[0].y;

        //normalized
        float normalizedPlayerY = playerPos.rectTransform.anchoredPosition.y/ miniMapRect.rect.height; 

        float needminus=miniMapRect.rect.height/2;

        miniMapRect.anchoredPosition= new Vector2(miniMapRect.anchoredPosition.x, (miniMapRect.rect.height * normalizedPlayerY-needminus)*-1 );

        if (boxes.Count <= 0)
            return;
        minX = boxes[0].transform.position.x;
        minY = boxes[0].transform.position.y;
        maxX = boxes[0].transform.position.x;
        maxY = boxes[0].transform.position.y;

        float gap = 2f;
        foreach (var point in boxes)
        {
            if (point.transform.position.x < minX)
            {
                minX = point.transform.position.x;
            }
            if (point.transform.position.x > maxX)
            {
                maxX = point.transform.position.x;
            }
            if (point.transform.position.y < minY)
            {
                minY = point.transform.position.y;
            }
            if (point.transform.position.y > maxY)
            {
                maxY = point.transform.position.y;
            }
        }

        LB =new Vector2(minX-gap, minY-gap-1);
        RT=new Vector2(maxX+gap, maxY+gap);

        var rect = new Rect(LB + ((RT - LB) / 2), RT - LB);

        //normalized
        float playerx =(player.transform.position.x-LB.x)/rect.width;
        float playery= (player.transform.position.y-LB.y) / rect.height;

        var miniMapsize = miniMapHorizontal.rectTransform.sizeDelta;

        playerPos.rectTransform.transform.localPosition= new Vector3(miniMapsize.x * playerx-miniMapsize.x/2,miniMapsize.y*playery - miniMapsize.y / 2);

    }

    
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;

        //MiniMapData data = DataTableMgr.GetTable<MiniMapData>().Get(0.ToString());

        //var sprite = Resources.Load<Sprite>(data.miniMapId);
        //var image = sprite.bounds.size;

        //if (image.x >= image.y)
        //{
        //    miniMapVertical.gameObject.SetActive(true);
        //    miniMapHorizontal.gameObject.SetActive(false);
        //    miniMapVertical.sprite = sprite;

        //}
        //else
        //{
        //    miniMapHorizontal.gameObject.SetActive(true);
        //    miniMapVertical.gameObject.SetActive(false);
        //    miniMapHorizontal.sprite = sprite;            
        //}

    }

    public void SetMiniMap(int id)
    {
        if (DataTableMgr.GetTable<MiniMapData>().Get(id.ToString()) == null)
        {
            return;
        }
        MiniMapData data = DataTableMgr.GetTable<MiniMapData>().Get(id.ToString());

        var sprite = Resources.Load<Sprite>(data.miniMapId);

        float ratio = sprite.textureRect.width / sprite.textureRect.height;

        miniMapHorizontal.gameObject.SetActive(true);
        miniMapHorizontal.sprite = sprite;
        var size = miniMapHorizontal.rectTransform.sizeDelta;
        size.y = size.x / ratio;
        miniMapHorizontal.rectTransform.sizeDelta = size;

        minimapObject.SetActive(true);


    }
}
