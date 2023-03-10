using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlowTime : MonoBehaviour
{
    private float time;
    public float Times { get { return time; } }
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void OnEnable()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            text.text = ((int)(time -= Time.deltaTime)).ToString();
        }
        else if(time<=0)
        {
            Time.timeScale = 0;         
            
        }
    }

    public void SetTime(float sec)
    {
        time = sec;
    }
}
