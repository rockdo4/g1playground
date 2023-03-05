using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;

    // The Start of the cable will be the transform of the Gameobject that has this component.
    // The Transform of the Gameobject where the End of the cable is. This needs to be assigned in the inspector.
    [SerializeField] private Transform Apoint;
    [SerializeField] private Transform Bpoint;

    [SerializeField] Transform endPointTransform;

    
    // These are used later for calculations
    int pointsInLineRenderer;
    Vector3 vectorFromStartToEnd;
    Vector3 sagDirection;
    float swayValue;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
