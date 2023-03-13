using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cannon : MonoBehaviour
{
    [Header("Cannon")]
    [SerializeField] private GameObject cannon;
    [SerializeField] private float rotationX;
    [SerializeField] private float force = 10f;
    [SerializeField] private float delay = 3f;

    private float timer = 0f;

    [Header("CannonBall Pool")]
    [SerializeField] private CannonBall cannonBall;
    public int maxPoolSize = 50;
    public int stackDefaultCapacity = 10;

    private IObjectPool<CannonBall> cannonPool;
    public IObjectPool<CannonBall> CannonPool
    {
        get
        {
            if (cannonPool == null)
                cannonPool =
                    new ObjectPool<CannonBall>(
                        CreateCannonBall,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize);
            return cannonPool;
        }
    }

    private CannonBall CreateCannonBall()
    {
        CannonBall ball = Instantiate(cannonBall, transform.position, cannon.transform.rotation);
        ball.cannonPool = CannonPool;
        return ball;
    }

    private void OnTakeFromPool(CannonBall ball)
    {
        ball.gameObject.SetActive(true);

    }

    private void OnReturnedToPool(CannonBall ball)
    {
        ball.gameObject.SetActive(false);

    }

    private void OnDestroyPoolObject(CannonBall ball)
    {
        Destroy(ball.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        cannon.transform.Rotate(rotationX, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            timer = 0f;
            var arrow = CannonPool.Get();
            arrow.transform.position = transform.position;
            arrow.GetComponent<Rigidbody>().velocity = transform.up * force;
        }
        
    }
}
