using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedEffect : MonoBehaviour, IStunnable
{
    public void OnStunned(float stunTime)
    {
        // stunned effect play for stunTime, get from effectmanager
    }
}
