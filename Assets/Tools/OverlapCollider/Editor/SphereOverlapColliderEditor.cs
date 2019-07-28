namespace OverlapColliders.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;
    
    [CustomEditor(typeof(SphereOverlapCollider))]
    public class SphereOverlapColliderEditor : OverlapColliderEditor
    {
        private SphereBoundsHandle _boxbounds = new SphereBoundsHandle();

        protected override void DrawHandles()
        {
            SphereOverlapCollider script = (SphereOverlapCollider) target;
            
            //TODO there is a problem with bound position when rotate and offsets the collider
            
            //Rotate world so the bounds handle can fit the gizmo
            Matrix4x4 initialMatrix = Handles.matrix;
            Matrix4x4 trs = Matrix4x4.TRS(script.transform.position, script.transform.rotation, Vector3.one);
            Handles.matrix = trs;
            
            //Set bounds
            _boxbounds.radius = script.GetRadius();
            _boxbounds.center = script.Offset;
            _boxbounds.handleColor = _boxbounds.wireframeColor = Color.green;

            //Draw bounds
            EditorGUI.BeginChangeCheck();
            _boxbounds.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                //Record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(script, "Change Bounds");

                //Apply changes
                var scale = script.transform.lossyScale;
                var radius = _boxbounds.radius;
                float[] values = {Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z)};
                float mod = Mathf.Max(values);
                script.Radius = Mathf.Abs(radius) / mod;
                script.Offset = _boxbounds.center;
            }
            
            //Reset world
            Handles.matrix = initialMatrix;
        }
    }
}