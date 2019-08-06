using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DragonBones;

[CustomEditor(typeof(AnimationControler), true)]
public class AnimationControlerEditor : Editor
{
    private AnimationControler t;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (t == null) t = target as AnimationControler;

        if (t.armature == null) t.armature = t.GetComponent<UnityArmatureComponent>();

        //  Test();
        ActionButton();
    }

    private void ActionButton()
    {
        if (GUILayout.Button("Flip"))
        {
          //  t.Flip();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Idle"))
        {
            t.Idle();
        }

        if (GUILayout.Button("Walk"))
        {
            t.Walk();
        }
        EditorGUILayout.EndHorizontal();
        t.timeScale = EditorGUILayout.FloatField(t.timeScale);
        EditorGUILayout.BeginHorizontal();
        t.tAnimation = EditorGUILayout.TextField("Animation Name", t.tAnimation);
        
        // Debug.Log(t.tAnimation);     
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Stop"))
        {
            t.Stop();
        }

        if (GUILayout.Button("Play"))
        {
            t.Play(t.tAnimation, 0, t.timeScale);
        }

        EditorGUILayout.EndHorizontal();
    }

}
