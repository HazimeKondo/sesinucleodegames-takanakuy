namespace OverlapColliders.Editor
{
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(OverlapCollider), true)]
    public class OverlapColliderEditor : Editor
    {
        private OverlapCollider _script;
        private bool _isEditing = false;
        private Tool _lastTool;

        private Texture2D _icon;

        private void OnEnable()
        {
            _lastTool = Tool.None;
            _script = target as OverlapCollider;
            _icon = Resources.Load("Overlap/Icons/ic_16x16_OverlapCollider") as Texture2D;
        }

        private void OnDisable()
        {
            if (_lastTool != Tool.None) Tools.current = _lastTool;
            if (_script) EditorUtility.SetDirty(_script);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(((Screen.width - 32) / 2.4f));
            EditorGUI.BeginChangeCheck();
            {
                //Button to enter/exit edit collider mode
                _isEditing = GUILayout.Toggle(_isEditing, _icon, GUI.skin.button, GUILayout.Width(30));
            }
            if (EditorGUI.EndChangeCheck())
            {
                //Hide/unhide actual tool when in/out of edit collider mode
                if (!_isEditing) Tools.current = _lastTool;
                else
                {
                    _lastTool = Tools.current;
                    Tools.current = Tool.None;
                }

                SceneView.RepaintAll();
            }

            GUILayout.Label("Edit Collider");
            EditorGUILayout.EndHorizontal();

            //Draw default inspector without script field
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            if (!_isEditing) return;

            //Prevent of clicking other objects in sceneview
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            DrawHandles();
        }

        /// <summary>
        /// Draw the handles specific of each shape
        /// </summary>
        protected virtual void DrawHandles() {}
    }
}