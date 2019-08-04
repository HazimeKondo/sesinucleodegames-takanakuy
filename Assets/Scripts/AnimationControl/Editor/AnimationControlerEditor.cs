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
        EditorGUILayout.BeginHorizontal();
        t.tAnimation = EditorGUILayout.TextField("animationName", t.tAnimation);
       // Debug.Log(t.tAnimation);

        if (t.armature.animation.isPlaying)
        {
            if (GUILayout.Button("Stop"))
            {
                t.Stop();
            }
        }
        else
        {
            if (GUILayout.Button("Play"))
            {
                t.Play(t.tAnimation, 0);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

}
