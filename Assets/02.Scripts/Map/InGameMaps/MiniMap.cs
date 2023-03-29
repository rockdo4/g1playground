using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
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
    [SerializeField] private Image checkpointPrefab;
    //  private List<Image> checkpointicon = new List<Image>();

    List<Image> shouldrelease = new List<Image>();
    Vector2 LB;
    Vector2 RT;
    Rect rect;
    Vector2 miniMapsize;

    public int maxPoolSize = 50;
    public int stackDefaultCapacity = 10;
    private IObjectPool<Image> checkpointiconPool;
    public IObjectPool<Image> CheckpointiconPool
    {
        get
        {
            if (checkpointiconPool == null)
                checkpointiconPool =
                    new ObjectPool<Image>(
                        CreateCannonBall,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize);
            return checkpointiconPool;
        }
    }

    private Image CreateCannonBall()
    {
        Image ball = Instantiate(checkpointPrefab, transform);

        return ball;
    }

    private void OnTakeFromPool(Image ball)
    {
        ball.gameObject.SetActive(true);

    }

    private void OnReturnedToPool(Image ball)
    {
        ball.gameObject.SetActive(false);

    }

    private void OnDestroyPoolObject(Image ball)
    {
        Destroy(ball.gameObject);
    }


    private string recentMapName = "village";
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
        checkpointiconPool = new ObjectPool<Image>(CreateCannonBall, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);

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

    

        //normalized
        float playerx = (player.transform.position.x - LB.x) / rect.width;
        float playery = (player.transform.position.y - LB.y) / rect.height;

         miniMapsize = miniMapHorizontal.rectTransform.sizeDelta;

        playerPos.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * playerx - miniMapsize.x / 2, miniMapsize.y * playery - miniMapsize.y / 2);



        //checkpoint

      
    }

    IEnumerator CSetCheckPoint()
    {
        yield return null;
        if (recentMapName != MapManager.instance.GetCurrentMapName())
        {
            foreach (var rel in shouldrelease)
            {
                checkpointiconPool.Release(rel);
            }
            shouldrelease.Clear();

            recentMapName = MapManager.instance.GetCurrentMapName();
            var currentcheckpoints = GameObject.Find(MapManager.instance.GetCurrentChapterName()).transform.Find(MapManager.instance.GetCurrentMapName()).GetComponentsInChildren<Checkpoint>();

            if (currentcheckpoints.Length > 0)
            {
                for (int i = 0; i < currentcheckpoints.Length; i++)
                {
                    float x = (currentcheckpoints[i].transform.position.x - LB.x) / rect.width;
                    float y = (currentcheckpoints[i].transform.position.y - LB.y) / rect.height;

                    var temp1 = checkpointiconPool.Get();
                    temp1.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
                    temp1.transform.SetParent(miniMapHorizontal.transform, false);
                    shouldrelease.Add(temp1);

                }
            }
        }
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

    IEnumerator CSetColliderAndCheckPoint()
    {
        yield return null;
        yield return null;

        var boxes = MapManager.instance.outlines;

        float minX, maxX;
        float minY, maxY;

        //normalized
        float normalizedPlayerY = playerPos.rectTransform.anchoredPosition.y / miniMapRect.rect.height;

        float needminus = miniMapRect.rect.height / 2;

        float pivotcenter = miniMapRect.rect.height / 4;

        if (miniMapRect.rect.height > 80)
        {
            miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, (miniMapRect.rect.height * normalizedPlayerY - needminus) * -1);
            if (Mathf.Abs(miniMapRect.anchoredPosition.y) >= pivotcenter)
            {
                if (miniMapRect.anchoredPosition.y >= 0)
                    miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, pivotcenter);
                else
                    miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, pivotcenter * -1);

            }
        }

        if (boxes.Count <= 0)
            yield break;
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

        LB = new Vector2(minX - gap, minY - gap - 1);
        RT = new Vector2(maxX + gap, maxY + gap);

        rect = new Rect(LB + ((RT - LB) / 2), RT - LB);


        if (recentMapName != MapManager.instance.GetCurrentMapName())
        {
            foreach (var rel in shouldrelease)
            {
                checkpointiconPool.Release(rel);
            }
            shouldrelease.Clear();

            recentMapName = MapManager.instance.GetCurrentMapName();
            var currentcheckpoints = MapManager.instance.GetCurrentStageObject().GetComponentsInChildren<Checkpoint>();

            if (currentcheckpoints.Length > 0)
            {
                for (int i = 0; i < currentcheckpoints.Length; i++)
                {
                    float x = (currentcheckpoints[i].transform.position.x - LB.x) / rect.width;
                    float y = (currentcheckpoints[i].transform.position.y - LB.y) / rect.height;

                    var temp1 = checkpointiconPool.Get();
                    temp1.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
                    temp1.transform.SetParent(miniMapHorizontal.transform, false);
                    shouldrelease.Add(temp1);

                }
            }
        }
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



        StartCoroutine(CSetColliderAndCheckPoint());



    }
}
