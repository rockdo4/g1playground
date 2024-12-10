using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedEvent : MonoBehaviour, IDestructible
{
    public System.Action OnDestroyEvent;

    public void OnDestroyObj()
    {
        OnDestroyEvent?.Invoke();
    }
}
