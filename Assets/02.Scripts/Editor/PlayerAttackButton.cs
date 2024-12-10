using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CustomEditor(typeof(PlayerAttack) , true)]
public class PlayerAttackButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply Attack Settings"))
        {
            PlayerAttack attack = (PlayerAttack)target;
            attack.ApplyInspectorValues();
        }
    }
}
