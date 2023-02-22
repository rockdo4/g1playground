using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1stBuild : MonoBehaviour
{
    private GameObject player;
    private MeshRenderer thisRenderer;
    public Material normal;
    public Material attack;
    public System.Action<GameObject, GameObject> OnCollided;

    public void Activate(bool active) => gameObject.SetActive(active);
    public void Normal() => thisRenderer.material = normal;
    public void Attack() => thisRenderer.material = attack;

    private void Awake()
    {
        player = transform.parent.gameObject;
        thisRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollided != null)
            OnCollided(player, other.gameObject);
    }
}
