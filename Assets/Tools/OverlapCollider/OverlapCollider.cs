namespace OverlapColliders
{
    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;
    using System.Collections.Generic;
    
    [DefaultExecutionOrder(Int32.MinValue)]
    public abstract class OverlapCollider : MonoBehaviour
    {
        [Serializable] public class OverlapEvent : UnityEvent<Collider[]> {}

        private List<Collider> _lastUpdateColliders = new List<Collider>();
        private List<Collider> _enteringColliders = new List<Collider>();
        private List<Collider> _exitingColliders = new List<Collider>();
        private List<Collider> _stayingColliders = new List<Collider>();

        [Header("Events")]
        public OverlapEvent OnOverlapEnter;
        public OverlapEvent OnOverlapStay;
        public OverlapEvent OnOverlapExit;

        [Header("Params")]
        public LayerMask LayerMaskToCollide;
        public Vector3 Offset;

        /// <summary>
        /// Return an array of colliders that enter on overlap area in this frame
        /// </summary>
        public Collider[] EnteringColliders { get { return _enteringColliders.ToArray(); }}

        /// <summary>
        /// Return an array of colliders that exit of overlap area in this frame
        /// </summary>
        public Collider[] ExitingColliders { get { return _exitingColliders.ToArray(); }}

        /// <summary>
        /// Return an array of colliders that is inside of overlap since last frame
        /// </summary>
        public Collider[] StayingColliders { get {return _stayingColliders.ToArray(); }}
        
        private void Update()
        {
            List<Collider> actualColliders = Overlapping();

            //Event triggers
            _enteringColliders = actualColliders.Where(o => !_lastUpdateColliders.Contains(o)).ToList();
            if (_enteringColliders.Count > 0)
                OnOverlapEnter.Invoke(_enteringColliders.ToArray());
            
            _stayingColliders = actualColliders.Where(o => _lastUpdateColliders.Contains(o)).ToList();
            if (_stayingColliders.Count > 0)
                OnOverlapStay.Invoke(_stayingColliders.ToArray());

            _exitingColliders = _lastUpdateColliders.Where(o => !actualColliders.Contains(o)).ToList();
            if (_exitingColliders.Count > 0)
                OnOverlapExit.Invoke(_exitingColliders.ToArray());


            //Prepare for next update
            _lastUpdateColliders = actualColliders;
        }

        private void OnDisable()
        {
            //clearing all lists in case enabling again will behaviour as a new overlap collider
            _lastUpdateColliders.Clear();
            _enteringColliders.Clear();
            _exitingColliders.Clear();
            _stayingColliders.Clear();
        }

        /// <summary>
        /// Return offset with rotation applied
        /// </summary>
        public Vector3 GetOffset()
        {
            Vector3 temp = Vector3.zero;
            temp += transform.right * Offset.x * transform.lossyScale.x;
            temp += transform.up * Offset.y * transform.lossyScale.y;
            temp += transform.forward * Offset.z * transform.lossyScale.z;
            return temp;
        }

        /// <summary>
        /// Return a list of colliders inside of overlap area
        /// </summary>
        protected abstract List<Collider> Overlapping();
    }
}