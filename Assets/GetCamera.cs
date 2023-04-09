using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCamera : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;

    private void OnEnable()
    {
        StartCoroutine(AttachCam());
    }

    IEnumerator AttachCam()
    {
        yield return null;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Camera camera = Camera.main;
        canvas.worldCamera = camera;
    }

    private void Update()
    {
        if (canvas.worldCamera == null)
        {
            Camera camera = Camera.main;
            canvas.worldCamera = camera;
        }
    
    }
}
