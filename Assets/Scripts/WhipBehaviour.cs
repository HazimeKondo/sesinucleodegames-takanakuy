using System.Collections;
using System.Linq;
using OverlapColliders;
using UnityEngine;
using UnityEngine.Serialization;

public class WhipBehaviour : MonoBehaviour
{
    [SerializeField] private float _halfRange = 2;
    [SerializeField] private float _halfThickness = 0.5f;

    private void Awake()
    {
        throw new System.NotImplementedException();
    }


    private void Attack()
    {
        Collider[] colls = Physics.OverlapBox(transform.position + transform.forward * _halfRange, new Vector3(_halfThickness ,1,_halfRange), transform.rotation);
        if (colls.Length > 0)
        {
            colls.First().GetComponent<CrowdBehaviour>().Hit();
        }
    }
    
}
