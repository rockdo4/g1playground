using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Cinemachine.CinemachineConfiner confiner;
    Cinemachine.CinemachineFramingTransposer composer;
    private float originSoftZoneHeight;
    private float originSoftZoneWidth;
    // Start is called before the first frame update
    private void Awake()
    {
        confiner = GetComponent<Cinemachine.CinemachineConfiner>();
        composer = GameManager.instance.followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();

        originSoftZoneHeight = composer.m_SoftZoneHeight;
        originSoftZoneWidth = composer.m_SoftZoneWidth;
    }

    public void SetCollider(PolygonCollider2D collider)
    {
        confiner.m_BoundingShape2D = collider;
    }

    public void ResetToSoftZone()
    {
        StartCoroutine(CoResetToOriginSoftZone());
    }

    IEnumerator CoResetToOriginSoftZone()
    {
        yield return null;
        composer.m_SoftZoneHeight = originSoftZoneHeight;
        composer.m_SoftZoneWidth = originSoftZoneWidth;
    }

    public void ZeroToSoftZone()
    {
        composer.m_SoftZoneWidth = 0;
        composer.m_SoftZoneHeight = 0;
    }
}
