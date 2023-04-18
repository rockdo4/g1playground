using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static StageController;
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
    [SerializeField] private Image blockPrefab;
    [SerializeField] private Image enemyPrefab;
    //  private List<Image> checkpointicon = new List<Image>();

    Vector2 LB;
    Vector2 RT;
    Rect rect;
    Vector2 miniMapsize;

    public int maxPoolSize = 50;
    public int stackDefaultCapacity = 10;

    List<Image> checkPointIconUsingPool = new List<Image>();
    Dictionary<Image, GameObject> blockIconUsingPool = new Dictionary<Image, GameObject>();
    Dictionary<Image, GameObject> enemiesIconUsingPool = new Dictionary<Image, GameObject>();
    Dictionary<Image, GameObject> gimmickIconUsingPool = new Dictionary<Image, GameObject>();

    private IObjectPool<Image> checkpointiconPool;
    public IObjectPool<Image> CheckPointIconPool
    {
        get
        {
            if (checkpointiconPool == null)
            {
                checkpointiconPool = new ObjectPool<Image>(CreateCheckPoint, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, stackDefaultCapacity, maxPoolSize);
            }
            return checkpointiconPool;
        }
    }

    private IObjectPool<Image> blockPool;
    public IObjectPool<Image> BlockPool
    {
        get
        {
            if (blockPool == null)
            {
                blockPool = new ObjectPool<Image>(CreateBlock, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, stackDefaultCapacity, maxPoolSize);
            }
            return blockPool;
        }
    }

    private IObjectPool<Image> enemiesPool;
    public IObjectPool<Image> EnemiesPool
    {
        get
        {
            if (enemiesPool == null)
            {
                enemiesPool = new ObjectPool<Image>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, stackDefaultCapacity, maxPoolSize);
            }
            return enemiesPool;
        }
    }

    private IObjectPool<Image> gimmickPool;
    public IObjectPool<Image> GimmickPool
    {
        get
        {
            if (gimmickPool == null)
            {
                gimmickPool = new ObjectPool<Image>(CreateBlock, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, stackDefaultCapacity, maxPoolSize);
            }
            return gimmickPool;
        }
    }

    //private IObjectPool<Image> enemiesPool;
    //public IObjectPool<Image> EnemiesPool
    //{
    //    get
    //    {
    //        if (enemiesPool == null)
    //        {
    //            enemiesPool = new ObjectPool<Image>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, stackDefaultCapacity, maxPoolSize);
    //        }
    //        return enemiesPool;
    //    }
    //}

    private Image CreateEnemy()
    {
        Image ball = Instantiate(enemyPrefab, transform);

        return ball;
    }
    private Image CreateBlock()
    {
        Image ball = Instantiate(blockPrefab, transform);

        return ball;
    }
    private Image CreateCheckPoint()
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
        checkpointiconPool = new ObjectPool<Image>(CreateCheckPoint, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        blockPool = new ObjectPool<Image>(CreateBlock, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        enemiesPool = new ObjectPool<Image>(CreateEnemy, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        gimmickPool = new ObjectPool<Image>(CreateBlock, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
        //singleton
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (MapManager.instance.outlines == null || MapManager.instance.GetCurrentChapterObject() == null || MapManager.instance.GetCurrentChapterObject().name == "Village"||MapManager.instance.GetCurrentStageObject().name=="Stage15")
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
        if (playerx == float.PositiveInfinity || playery == float.NegativeInfinity)
        {
            return;
        }

        playerPos.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * playerx - miniMapsize.x / 2, miniMapsize.y * playery - miniMapsize.y / 2);

        float normalizedPlayerY = playerPos.rectTransform.anchoredPosition.y / miniMapRect.rect.height;

        float needminus = miniMapRect.rect.height / 2;

        float pivotcenter = miniMapRect.rect.height / 4;

        if (miniMapRect.rect.height > 80)
        {
            miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, (miniMapRect.rect.height * normalizedPlayerY - needminus) * -1);
            if (Mathf.Abs(miniMapRect.anchoredPosition.y) > pivotcenter)
            {
                if (miniMapRect.anchoredPosition.y >= 0)
                    miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, pivotcenter);
                else
                    miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, pivotcenter * -1);
            }

        }
        else
            miniMapRect.anchoredPosition = new Vector2(miniMapRect.anchoredPosition.x, 0);

        foreach (var pool in blockIconUsingPool)
        {
            pool.Key.gameObject.SetActive(pool.Value.activeSelf);
            pool.Key.enabled = pool.Value.activeSelf;
        }

        foreach (var enemy in enemiesIconUsingPool)
        {
            enemy.Key.gameObject.SetActive(enemy.Value.activeSelf);
            enemy.Key.enabled = enemy.Value.activeSelf;
            float x = (enemy.Value.transform.position.x - LB.x) / rect.width;
            float y = (enemy.Value.transform.position.y - LB.y) / rect.height;
            enemy.Key.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
        }

        foreach (var gimmick in gimmickIconUsingPool)
        {
            gimmick.Key.gameObject.SetActive(gimmick.Value.activeSelf);
            if (gimmick.Value.CompareTag("Falling"))
            {
                gimmick.Key.enabled = gimmick.Value.transform.parent.gameObject.activeSelf;
            }
            else
            {
                gimmick.Key.enabled = gimmick.Value.activeSelf;
            }
            float x = (gimmick.Value.transform.position.x - LB.x) / rect.width;
            float y = (gimmick.Value.transform.position.y - LB.y) / rect.height;
            gimmick.Key.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }

    IEnumerator CSetColliderAndCheckPoint()
    {
        yield return null;
        yield return null;

        var boxes = MapManager.instance.outlines;

        float minX, maxX;
        float minY, maxY;

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

        foreach (var rel in checkPointIconUsingPool)
        {
            checkpointiconPool.Release(rel);
        }
        checkPointIconUsingPool.Clear();
        foreach (var rel in blockIconUsingPool)
        {
            blockPool.Release(rel.Key);
        }
        blockIconUsingPool.Clear();
        foreach (var rel in enemiesIconUsingPool)
        {
            enemiesPool.Release(rel.Key);
        }
        enemiesIconUsingPool.Clear();

        foreach (var rel in gimmickIconUsingPool)
        {
            gimmickPool.Release(rel.Key);
        }
        gimmickIconUsingPool.Clear();

        var currentcheckpoints = MapManager.instance.GetCurrentStageObject().GetComponentsInChildren<Checkpoint>();

        if (currentcheckpoints.Length > 0)
        {
            for (int i = 0; i < currentcheckpoints.Length; i++)
            {
                float x = (currentcheckpoints[i].transform.position.x - LB.x) / rect.width;
                float y = (currentcheckpoints[i].transform.position.y - LB.y) / rect.height;

                var temp1 = checkpointiconPool.Get();
                temp1.transform.SetParent(miniMapHorizontal.transform);
                temp1.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
                checkPointIconUsingPool.Add(temp1);

            }
        }

        var currentEnemies = MapManager.instance.GetCurrentStageObject().GetComponentsInChildren<Enemy>();
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            float x = (currentEnemies[i].transform.position.x - LB.x) / rect.width;
            float y = (currentEnemies[i].transform.position.y - LB.y) / rect.height;

            var temp1 = enemiesPool.Get();
            if (!currentEnemies[i].gameObject.activeSelf)
            {
                temp1.enabled = false;
            }
            temp1.transform.SetParent(miniMapHorizontal.transform, false);
            temp1.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
            enemiesIconUsingPool.Add(temp1, currentEnemies[i].gameObject);

        }


        if (MapManager.instance.GetCurrentStageObject().GetComponent<StageController>().unlockRequirement != UnLockRequirement.Puzzle)
            yield break;

        var currentBlock = MapManager.instance.GetCurrentStageObject().transform.Find("Floor");


        for (int i = 0; i < currentBlock.childCount; i++)
        {
            float x = (currentBlock.GetChild(i).transform.position.x - LB.x) / rect.width;
            float y = (currentBlock.GetChild(i).transform.position.y - LB.y) / rect.height;

            var temp1 = blockPool.Get();
            if (!currentBlock.GetChild(i).gameObject.activeSelf)
            {
                temp1.enabled = false;
            }
            temp1.transform.SetParent(miniMapHorizontal.transform, false);
            temp1.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);
            blockIconUsingPool.Add(temp1, currentBlock.GetChild(i).gameObject);
        }

        Transform currentStage = MapManager.instance.GetCurrentStageObject().transform;
        int childcount = currentStage.transform.childCount;

        for (int i = 0; i < childcount; i++)
        {
            if (currentStage.GetChild(i).gameObject.layer == 15)
            {
                int objecttilechildcount = currentStage.GetChild(i).childCount;
                for (int j = 0; j < objecttilechildcount; j++)
                {
                    float x = (currentStage.GetChild(i).GetChild(j).transform.position.x - LB.x) / rect.width;
                    float y = (currentStage.GetChild(i).GetChild(j).transform.position.y - LB.y) / rect.height;

                    var temp = gimmickPool.Get();
                    if (!currentStage.GetChild(i).GetChild(j).gameObject.activeSelf)
                    {
                        temp.enabled = false;
                    }
                    temp.transform.SetParent(miniMapHorizontal.transform, false);
                    temp.rectTransform.transform.localPosition = new Vector3(miniMapsize.x * x - miniMapsize.x / 2, miniMapsize.y * y - miniMapsize.y / 2);

                    gimmickIconUsingPool.Add(temp, currentStage.GetChild(i).GetChild(j).gameObject);
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
