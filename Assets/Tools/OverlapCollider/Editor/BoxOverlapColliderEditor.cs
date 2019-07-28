namespace OverlapColliders.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;
    
    [CustomEditor(typeof(BoxOverlapCollider))]
    public class BoxOverlapColliderEditor : OverlapColliderEditor
    {
        private readonly BoxBoundsHandle _boxBounds = new BoxBoundsHandle();

        protected override void DrawHandles()
        {
            BoxOverlapCollider script = (BoxOverlapCollider) target;
            
            //Rotate world so the bounds handle can fit the gizmo
            Matrix4x4 initialMatrix = Handles.matrix;
            Matrix4x4 trs = Matrix4x4.TRS(script.transform.position, script.transform.rotation, script.transform.lossyScale);
            Handles.matrix = trs;
            
            //Set bounds
            _boxBounds.size = script.Size;
            _boxBounds.center = script.Offset;
            _boxBounds.handleColor = _boxBounds.wireframeColor = Color.green;

            //Draw bounds
            EditorGUI.BeginChangeCheck();
            _boxBounds.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                //Record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(script, "Change Bounds");

                //Apply changes
                var scale = script.transform.lossyScale;
                var size = _boxBounds.size;
                Vector3 newSize = Vector3.zero;
                newSize.x = Mathf.Abs(size.x);
                newSize.y = Mathf.Abs(size.y);
                newSize.z = Mathf.Abs(size.z);
                script.Size = newSize;
                script.Offset = _boxBounds.center;
            }
            
            //Reset world
            Handles.matrix = initialMatrix;
        }
    }
}