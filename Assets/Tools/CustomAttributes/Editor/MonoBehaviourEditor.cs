using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{

    List<MethodInfo> methods;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.DrawDefaultMethods();
    }
}