using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerLevelManager), true)]
public class LevelButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply Level"))
        {
            PlayerLevelManager leveManager = (PlayerLevelManager)target;
            leveManager.SetLevel(leveManager.Level);
        }
    }
}
