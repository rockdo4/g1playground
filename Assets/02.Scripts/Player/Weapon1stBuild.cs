using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1stBuild : MonoBehaviour
{
    private GameObject player;
    public System.Action<GameObject, GameObject> OnCollided;

    public void Activate(bool active) => gameObject.SetActive(active);

    private void Awake()
    {
        player = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollided != null)
            OnCollided(player, other.gameObject);
    }
}
