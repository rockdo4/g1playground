using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1stBuild : MonoBehaviour
{
    private GameObject player;
    public System.Action<GameObject, GameObject> OnCollided;
    private List<GameObject> attackedList = new List<GameObject>();

    public void Activate(bool active) => gameObject.SetActive(active);

    private void Awake()
    {
        player = transform.parent.gameObject;
    }

    private void OnEnable()
    {
        attackedList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnCollided != null)
        {
            if (attackedList.Contains(other.gameObject))
                return;
            attackedList.Add(other.gameObject);
            OnCollided(player, other.gameObject);
        }
    }
}
