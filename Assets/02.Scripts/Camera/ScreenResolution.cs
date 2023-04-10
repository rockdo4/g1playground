using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenResolution : MonoBehaviour
{
    void Awake()
    {
        OnSetting();
    }
    public void OnSetting()
    {

        Camera camera = GetComponent<Camera>();

        Rect rect = camera.rect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)

        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)

        {

            rect.height = scaleheight;

            rect.y = (1f - scaleheight) / 2f;

        }

        else

        {

            rect.width = scalewidth;

            rect.x = (1f - scalewidth) / 2f;

        }

        camera.rect = rect;

    }

    public void OnReset()
    {

        Camera camera = GetComponent<Camera>();

        camera.rect = new Rect(0, 0, 1, 1);

    }

    void OnEnable()
    {

#if !UNITY_EDITOR

RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;

#endif

    }

    void OnDisable()
    {

#if !UNITY_EDITOR

RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_endCameraRendering;

#endif

    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {

        GL.Clear(true, true, Color.black);

    }

    void OnPreCull()
    {

        GL.Clear(true, true, Color.black);

    }
}
