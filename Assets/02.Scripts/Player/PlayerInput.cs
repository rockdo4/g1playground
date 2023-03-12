using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public bool LeftMove { get; private set; }
    public bool RightMove { get; private set; }
    public bool Jump { get; private set; }
    public bool Dash { get; private set; }

    private void Update()
    {
        Jump = false;
        Dash = false;

        foreach (var t in Input.touches)
        {
            var viewportPoint = Camera.main.ScreenToViewportPoint(t.position);

            if (viewportPoint.x > 0.5f && viewportPoint.y < 0.5f)
            {
                if (t.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(t.fingerId))
                    Jump = true;
            }
        }

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.A))
            LeftMove = true;
        else
            LeftMove = false;

        if (Input.GetKey(KeyCode.D))
            RightMove = true;
        else
            RightMove = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash = true;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump = true;
#endif
    }

    public void MoveLeft(bool move) => LeftMove = move;
    public void MoveRight(bool move) => RightMove = move;
    public void ExeDash() => Dash = true;
}
