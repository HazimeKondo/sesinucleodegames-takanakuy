namespace OverlapColliders
{
    using System.Linq;
    using UnityEngine;
    using System.Collections.Generic;
    
    public class SphereOverlapCollider : OverlapCollider
    {
        //Radius of the sphere
        public float Radius = 0.5f;

        protected override List<Collider> Overlapping()
        {
            return Physics.OverlapSphere(transform.position + GetOffset(), GetRadius(), LayerMaskToCollide).ToList();
        }

        /// <summary>
        /// Return radius scaled according to the biggest object axis scale
        /// </summary>
        public float GetRadius()
        {
            float[] values = new float[] {Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y), Mathf.Abs(transform.lossyScale.z)};
            float mod = Mathf.Max(values);
            return Mathf.Abs(Radius) * mod;
        }

        #region DRAW GIZMOS

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!this.enabled) return;
            DrawSphere();
        }

        private void DrawSphere()
        {
            Matrix4x4 matrix = Gizmos.matrix;
            Matrix4x4 trs = Matrix4x4.TRS(transform.position + GetOffset(), transform.rotation, Vector3.one * GetRadius());
            Gizmos.matrix = trs;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Vector3.zero, 1);
            Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
            Gizmos.DrawSphere(Vector3.zero, 1);
            Gizmos.matrix = matrix;
        }
#endif

        #endregion
    }
}