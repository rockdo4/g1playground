using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 cameraPosition = new Vector3(0, 5, -15);
    public Vector3 eulerAngles = new Vector3(15, 0, 0);
    public float cameraSpeed = 5f;

    private void Awake()
    {
        transform.eulerAngles = eulerAngles;
    }

    void FixedUpdate()
    {
        CameraMove();
    }

    public void CameraMove()
    {
        transform.position = Vector3.Lerp(transform.position,
           GameManager.instance.playerController.transform.position + cameraPosition,
           cameraSpeed * Time.fixedDeltaTime);


    }
}
