using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CustomEditor(typeof(SkillAttack) , true)]
public class SkillButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save Skill Data"))
        {
            SkillAttack skill = (SkillAttack)target;
            skill.SaveSkillData();
        }
    }
}
