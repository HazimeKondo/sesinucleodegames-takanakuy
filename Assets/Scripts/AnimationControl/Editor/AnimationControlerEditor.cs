using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationControler), true)]
public class AnimationControlerEditor : Editor
{
    private AnimationControler t;
    int _animationIndex = -1;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (t == null)
        {
            t = target as AnimationControler;
        }
       
        Test();
        ActionButton();
    }

    private void ActionButton()
    {

        if (GUILayout.Button("Idle"))
        {
            t.Idle();
        }

        if (GUILayout.Button("Walk"))
        {
            t.Walk();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Whip"))
        {
            t.ToWhip();
        }
        if (GUILayout.Button("Stop"))
        {
            t.Stop();
        }
        EditorGUILayout.EndHorizontal();
    }

    void Test()
    {
        var _animationNames = t.armature.animation.animationNames;      

        // Animation
        if (_animationNames != null && _animationNames.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();
            List<string> anims = new List<string>(t.armature.animation.animationNames);
            anims.Insert(0, "<None>");
            int animationIndex = EditorGUILayout.Popup("Animation", _animationIndex + 1, anims.ToArray()) - 1;
            if (animationIndex != _animationIndex)
            {
                _animationIndex = animationIndex;
                if (animationIndex >= 0)
                {
                    t.armature.animationName = _animationNames [animationIndex];
                    var animationData =  t.armature.animation.animations[ t.armature.animationName];
                    t.armature.animation.Play(t.armature.animationName, 0);
                    
                }
                else
                {
                    t.armature.animationName = null;
                   
                    t.armature.animation.Stop();
                }              
            }

            if (_animationIndex >= 0)
            {
                if (t.armature.animation.isPlaying)
                {
                    if (GUILayout.Button("Stop"))
                    {
                        t.armature.animation.Stop();
                    }
                }
                else
                {
                    if (GUILayout.Button("Play"))
                    {
                        t.armature.animation.Play(null, 0); 
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
