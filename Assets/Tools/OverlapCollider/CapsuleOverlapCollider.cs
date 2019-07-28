namespace OverlapColliders
{
    using System.Linq;
    using UnityEngine;
    using System.Collections.Generic;
    
    public class CapsuleOverlapCollider : OverlapCollider
    {
        public enum DirAxisEnum
        {
            XAxis,
            YAxis,
            ZAxis
        }

        //Selected axis
        public DirAxisEnum Axis = DirAxisEnum.YAxis;
        
        public float Height = 2f;
        public float Radius = 0.5f;

        protected override List<Collider> Overlapping()
        {
            float height = GetHeight();
            float radius = GetRadius();
            
            Vector3 dir = Vector3.zero;
            switch (Axis)
            {
                case DirAxisEnum.XAxis: dir = transform.right; break;
                case DirAxisEnum.YAxis: dir = transform.up; break;
                case DirAxisEnum.ZAxis: dir = transform.forward; break;
            }
            float usedHeight = (radius * 2 >= height) ? 0 : (height / 2) - radius;

            return Physics.OverlapCapsule(transform.position + GetOffset() + (dir * usedHeight),
                transform.position + GetOffset() - (dir * usedHeight), radius, LayerMaskToCollide).ToList();
        }

        /// <summary>
        /// Return height scaled according to axis selected
        /// </summary>
        public float GetHeight()
        {
            switch (Axis)
            {
                case DirAxisEnum.XAxis:
                    return Mathf.Abs(Height * transform.lossyScale.x);
                case DirAxisEnum.YAxis:
                    return Mathf.Abs(Height * transform.lossyScale.y);
                case DirAxisEnum.ZAxis:
                    return Mathf.Abs(Height * transform.lossyScale.z);
                default: return 0;

            }
        }

        /// <summary>
        /// Return radius scaled according to axis selected and biggests objects axis scale
        /// </summary>
        public float GetRadius()
        {
            switch (Axis)
            {
                case DirAxisEnum.XAxis:
                    return Mathf.Abs(Radius) * Mathf.Max(Mathf.Abs(transform.lossyScale.y), Mathf.Abs(transform.lossyScale.z));
                case DirAxisEnum.YAxis:
                    return Mathf.Abs(Radius) * Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.z));
                case DirAxisEnum.ZAxis:
                    return Mathf.Abs(Radius) * Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
                default: return 0;
            }
        }

        #region DRAW GIZMOS

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (!this.enabled) return;
            Draw();
        }

        private void Draw()
        {
            float height = GetHeight();
            Vector3 dir;
            if (Axis == DirAxisEnum.XAxis)
            {
                dir = transform.right;
            }
            else if (Axis == DirAxisEnum.ZAxis)
                dir = transform.forward;
            else
                dir = transform.up;

            DrawCapsule(GetOffset() + transform.position + (dir * height / 2), GetOffset() +
                                                                               transform.position - (dir * height / 2), Color.blue, GetRadius());
        }

        private void DrawCapsule(Vector3 start, Vector3 end, Color color, float radius = 1)
        {
            Vector3 up = (end - start).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            Color oldColor = Gizmos.color;
            Gizmos.color = color;

            float height = (start - end).magnitude;
            float sideLength = Mathf.Max(0, (height * 0.5f) - radius);
            Vector3 middle = (end + start) * 0.5f;

            start = middle + ((start - middle).normalized * sideLength);
            end = middle + ((end - middle).normalized * sideLength);

            //Radial circles
            DrawCircle(start, up, color, radius);
            DrawCircle(end, -up, color, radius);

            //Side lines
            Gizmos.DrawLine(start + right, end + right);
            Gizmos.DrawLine(start - right, end - right);

            Gizmos.DrawLine(start + forward, end + forward);
            Gizmos.DrawLine(start - forward, end - forward);

            for (int i = 1; i < 26; i++)
            {

                //Start endcap
                Gizmos.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + start, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + start, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + start, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + start);
                Gizmos.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + start, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + start);

                //End endcap
                Gizmos.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + end, Vector3.Slerp(right, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + end, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + end, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + end);
                Gizmos.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + end, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + end);
            }

            Gizmos.color = oldColor;
        }

        private void DrawCircle(Vector3 position, Vector3 up, Color color, float radius = 1.0f)
        {
            up = ((up == Vector3.zero) ? Vector3.up : up).normalized * radius;
            Vector3 forward = Vector3.Slerp(up, -up, 0.5f);
            Vector3 right = Vector3.Cross(up, forward).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4();

            matrix[0] = right.x;
            matrix[1] = right.y;
            matrix[2] = right.z;

            matrix[4] = up.x;
            matrix[5] = up.y;
            matrix[6] = up.z;

            matrix[8] = forward.x;
            matrix[9] = forward.y;
            matrix[10] = forward.z;

            Vector3 lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)));
            Vector3 nextPoint = Vector3.zero;

            Color oldColor = Gizmos.color;
            Gizmos.color = (color == default(Color)) ? Color.white : color;

            for (var i = 0; i < 91; i++)
            {
                nextPoint.x = Mathf.Cos((i * 4) * Mathf.Deg2Rad);
                nextPoint.z = Mathf.Sin((i * 4) * Mathf.Deg2Rad);
                nextPoint.y = 0;

                nextPoint = position + matrix.MultiplyPoint3x4(nextPoint);

                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }

            Gizmos.color = oldColor;
        }
#endif

        #endregion
    }
}