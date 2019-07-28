using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#region Extended Editor...
#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(XYRangeAttribute))]
public class XYRangeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Vector2:
                return 48;
            default:
                break;
        }
        return 16;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        XYRangeAttribute att = attribute as XYRangeAttribute;
        switch (property.propertyType)
        {
            case SerializedPropertyType.Vector2:
                Vector2 tempV2 = property.vector2Value;
                GUI.BeginGroup(position);
                GUI.Label(new Rect(0, 0, position.width, position.height/3), label.text,EditorStyles.boldLabel);
                GUI.Label(new Rect(0, 16, 16, position.height/3), att.Min.ToString());
                tempV2.x = EditorGUI.FloatField(new Rect(18, 16, 60, position.height/3), tempV2.x);
                tempV2.y = EditorGUI.FloatField(new Rect(position.width - 71, 16, 60, position.height/3), tempV2.y);
                GUI.Label(new Rect(position.width - 9, 16, 16, position.height/3), att.Max.ToString());
                EditorGUI.MinMaxSlider(new Rect(0, 32, position.width, position.height), ref tempV2.x, ref tempV2.y, att.Min, att.Max);
                GUI.EndGroup();
                property.vector2Value = tempV2;
                break;

            case SerializedPropertyType.Vector2Int:
                Vector2 tempV2int = property.vector2IntValue;
                EditorGUI.BeginChangeCheck();
                EditorGUI.MinMaxSlider(position, ref tempV2int.x, ref tempV2int.y, att.Min, att.Max);
                if (GUI.changed)
                {
                    tempV2int.x = Mathf.RoundToInt(tempV2int.x);
                    tempV2int.y = Mathf.RoundToInt(tempV2int.y);
                    property.vector2IntValue = new Vector2Int((int)tempV2int.x, (int)tempV2int.y);
                }
                break;

            default:
                GUI.BeginGroup(position);
                GUI.Label(new Rect(0, 0, Screen.width * 0.4f, 20), property.name);
                GUI.Label(new Rect(Screen.width * 0.4f, 0, Screen.width * 0.4f, 20), "Use XYRange with Vector2 only");
                GUI.EndGroup();
                break;
        }
    }
}

public static class ExtendedEditor
{
    public static void DrawDefaultMethods(this Editor editor)
    {
        string[] ignoreMethods = new string[] { };
        var script = editor.target;

        List<MethodInfo> methods =
            editor.target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) // Instance methods, both public and private/protected
        //.Where(x => x.DeclaringType == editor.target.GetType()) // Only list methods defined in our own class
        .Where(x => !ignoreMethods.Any(n => n == x.Name)) // Don't list methods in the ignoreMethods array (so we can exclude Unity specific methods, etc.
        .Select(x => x).ToList();

        foreach (var method in methods)
        {
            string name;
            //ButtonAttribute buttonAtt = (ButtonAttribute)Attribute.GetCustomAttribute(target.GetType().GetMethod(method).GetCustomAttributes(true).OfType<ButtonAttribute>().FirstOrDefault(), typeof(ButtonAttribute));
            var buttonAtt = method.GetCustomAttributes(typeof(ButtonAttribute), true).FirstOrDefault();
            name = (buttonAtt != null) ? " [Button]" : "";
            // name = target.GetType().GetMethod(method).GetCustomAttributes

            if (buttonAtt != null)
            {
                var argList = method.GetParameters().ToList();
                var argValue = argList.Select(x => x.DefaultValue).ToList();

                GUILayout.BeginHorizontal();
                if (argList.Count > 0)
                {
                    GUILayout.BeginVertical();
                    for (int i = 0; i < argList.Count; i++)
                    {
                        if (argList[i].ParameterType.Equals(typeof(int)))
                        {
                            argValue[i] = EditorGUILayout.IntField(argList[i].Name, (int)argValue[i]);
                        }
                    }
                    GUILayout.EndVertical();
                }
                if (GUILayout.Button(method.Name))
                {
                    if (argList.Count == 0)
                    {
                        try
                        {
                            Debug.Log("Method \"" + method.Name + "\" returned: " + method.Invoke(script, null));

                        }
                        catch
                        {
                            ((MonoBehaviour)script).SendMessage(method.Name, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                    else
                    {
                        MethodButtonWindow.ShowWindow(script, method, argList, argValue);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }

    public class MethodButtonWindow : EditorWindow
    {
        object _script;
        MethodInfo _method;
        List<ParameterInfo> _argList;
        List<object> _argValue;
        public static void ShowWindow(object script, MethodInfo method, List<ParameterInfo> argList, List<object> argValue)
        {
            MethodButtonWindow newWindow = GetWindow<MethodButtonWindow>(method.Name);
            newWindow._script = script;
            newWindow._method = method;
            newWindow._argList = argList;
            newWindow._argValue = argValue;
        }


        private void OnGUI()
        {
            for (int i = 0; i < _argList.Count; i++)
            {
                if (_argList[i].ParameterType == typeof(int))
                {
                    _argValue[i] = EditorGUILayout.IntField((int)_argValue[i]);
                }
                else if(_argList[i].ParameterType == typeof(float))
                {
                    _argValue[i] = EditorGUILayout.FloatField((float)_argValue[i]);
                }
                else if (_argList[i].ParameterType == typeof(string))
                {
                    _argValue[i] = EditorGUILayout.TextField((string)_argValue[i]);
                }
                //else if (_argList[i].ParameterType == typeof(bool))
                //{
                //    _argValue[i] = EditorGUILayout.Toggle((bool)_argList[i].DefaultValue);
                //}
                else
                {
                    try
                    {
                        _argValue[i] = EditorGUILayout.ObjectField((UnityEngine.Object)_argValue[i],_argList[i].ParameterType,true);
                    }
                    catch
                    {
                        GUILayout.Label("Argument type not supported");
                    }
                }
            }
            if (GUILayout.Button("OK"))
            {
                Debug.Log("Method \"" + _method.Name + "\" returned: " + _method.Invoke(_script, _argValue.ToArray()));
                this.Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                this.Close();
            }
        }
    }

}
#endif
#endregion

#region Attributes used in Extended Editor Methods

#region [Button]
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute { }
#endregion

#region [XYRange]
[AttributeUsage(AttributeTargets.Field)]
public class XYRangeAttribute : PropertyAttribute
{
    public float Min;
    public float Max;

    public XYRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}


#endregion

#endregion