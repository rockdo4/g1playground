using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenSize : MonoBehaviour
{

    public AspectRatioFitter fit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //가로/세로 모드 변경시 화면의 비율 맞추기
        float ratio = (float)Screen.width / (float)Screen.height;
        fit.aspectRatio = ratio;
    }
}
