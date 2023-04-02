using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelUi : MonoBehaviour
{
    public virtual void ActiveTrue() => gameObject.SetActive(true);
    public void ActiveFalse() => gameObject.SetActive(false);
}
