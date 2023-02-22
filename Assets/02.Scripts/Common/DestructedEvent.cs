using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedEvent : IDestructible
{
    public System.Action OnDestroyEvent;

    public void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }
}
