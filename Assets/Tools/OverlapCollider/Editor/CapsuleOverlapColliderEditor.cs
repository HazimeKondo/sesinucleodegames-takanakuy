namespace OverlapColliders.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;
    
    [CustomEditor(typeof(CapsuleOverlapCollider))]
    public class CapsuleOverlapColliderEditor : OverlapColliderEditor
    {
        private readonly CapsuleBoundsHandle _boxBounds = new CapsuleBoundsHandle();

        protected override void DrawHandles()
        {
            CapsuleOverlapCollider script = (CapsuleOverlapCollider) target;
            
            //TODO there is a problem with bound position when rotate and offsets the collider
            
            //Rotate world so the bounds handle can fit the gizmo and selected axis
            Matrix4x4 initialMatrix = Handles.matrix;
            Matrix4x4 trs = Matrix4x4.TRS(script.transform.position, script.transform.rotation, Vector3.one);
            Handles.matrix = trs;
            
            //Set bounds
            _boxBounds.radius = script.GetRadius();
            _boxBounds.height = script.GetHeight();
            _boxBounds.center = script.Offset;
            switch (script.Axis)
            {
                case CapsuleOverlapCollider.DirAxisEnum.XAxis: _boxBounds.heightAxis = CapsuleBoundsHandle.HeightAxis.X; break;
                case CapsuleOverlapCollider.DirAxisEnum.YAxis: _boxBounds.heightAxis = CapsuleBoundsHandle.HeightAxis.Y; break;
                case CapsuleOverlapCollider.DirAxisEnum.ZAxis: _boxBounds.heightAxis = CapsuleBoundsHandle.HeightAxis.Z; break;
            }
            _boxBounds.handleColor = _boxBounds.wireframeColor = Color.green;

            //Draw bounds
            EditorGUI.BeginChangeCheck();
            _boxBounds.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                //Record the target object before setting new values so changes can be undone/redone
                Undo.RecordObject(script, "Change Bounds");

                //Apply changes
                script.Radius = NewRadius(script, _boxBounds.radius);
                script.Height = NewHeight(script, _boxBounds.height);
                script.Offset = _boxBounds.center;
            }
            
            //Reset World
            Handles.matrix = initialMatrix;
        }
        
        public float NewHeight(CapsuleOverlapCollider script,float height)
        {
            switch (script.Axis)
            {
                case CapsuleOverlapCollider.DirAxisEnum.XAxis:
                    return Mathf.Abs(height / script.transform.lossyScale.x);
                case CapsuleOverlapCollider.DirAxisEnum.YAxis:
                    return Mathf.Abs(height / script.transform.lossyScale.y);
                case CapsuleOverlapCollider.DirAxisEnum.ZAxis:
                    return Mathf.Abs(height / script.transform.lossyScale.z);
                default: return 0;
            }
        }
        
        public float NewRadius(CapsuleOverlapCollider script, float radius)
        {
            switch (script.Axis)
            {
                case CapsuleOverlapCollider.DirAxisEnum.XAxis:
                    return Mathf.Abs(radius) / Mathf.Max(Mathf.Abs(script.transform.lossyScale.y), Mathf.Abs(script.transform.lossyScale.z));
                case CapsuleOverlapCollider.DirAxisEnum.YAxis:
                    return Mathf.Abs(radius) / Mathf.Max(Mathf.Abs(script.transform.lossyScale.x), Mathf.Abs(script.transform.lossyScale.z));
                case CapsuleOverlapCollider.DirAxisEnum.ZAxis:
                    return Mathf.Abs(radius) / Mathf.Max(Mathf.Abs(script.transform.lossyScale.x), Mathf.Abs(script.transform.lossyScale.y));
                default: return 0;
            }
        }
    }
}