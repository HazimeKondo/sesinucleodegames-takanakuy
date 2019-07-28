namespace OverlapColliders
{
    using System.Linq;
    using UnityEngine;
    using System.Collections.Generic;
    
    public class BoxOverlapCollider : OverlapCollider
    {
        //Sizes of cube
        public Vector3 Size = Vector3.one;

        protected override List<Collider> Overlapping()
        {
            return Physics.OverlapBox(transform.position + GetOffset(), GetSize() / 2,
                transform.rotation, LayerMaskToCollide).ToList();
        }
        
        /// <summary>
        /// Return the sizes with object scale applied
        /// </summary>
        public Vector3 GetSize()
        {
            Vector3 scaled;
            scaled.x = Mathf.Abs(Size.x * transform.lossyScale.x);
            scaled.y = Mathf.Abs(Size.y * transform.lossyScale.y);
            scaled.z = Mathf.Abs(Size.z * transform.lossyScale.z);
            return scaled;
        }

        #region DRAW GIZMOS

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!this.enabled) return;
            DrawSquare();
        }

        private void DrawSquare()
        {
            Matrix4x4 matrix = Gizmos.matrix;
            Matrix4x4 trs = Matrix4x4.TRS(transform.position + GetOffset(), transform.rotation, GetSize());
            Gizmos.matrix = trs;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = matrix;
        }
#endif
        #endregion
    }
}