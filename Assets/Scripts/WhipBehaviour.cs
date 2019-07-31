using System.Collections;
using System.Linq;
using OverlapColliders;
using UnityEngine;
using UnityEngine.Serialization;

public class WhipBehaviour : MonoBehaviour
{
    [SerializeField] private float _range = 2;
    [SerializeField] private float _halfThickness = 0.5f;




    private void Start()
    {
        Player.Input.Keyboard.Attack.performed += ctx => { Attack(); } ;
    }
    private void Update()
    {
        //rotacionar player
    }

    [Button]
    private void Attack()
    {        
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + (transform.forward*transform.localScale.x/2), _halfThickness , Vector3.forward , out hit, _range))
        {
            hit.transform.GetComponent<CrowdBehaviour>().Hit();
        }
    }


}
