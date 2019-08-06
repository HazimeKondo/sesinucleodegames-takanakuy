using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallow : MonoBehaviour
{
    public GameObject objFalow;
    public Rigidbody rbFallow;
    [SerializeField]AnimationControler animationControler;

    void Awake()
    {
        transform.position = new Vector3(objFalow.transform.position.x, objFalow.transform.position.y + .5f, objFalow.transform.position.z);
        animationControler = GetComponent<AnimationControler>();
        rbFallow = objFalow.GetComponent<Rigidbody>();
    }

    void LookDir()
    {
        Debug.Log(Math.Sign(rbFallow.velocity.z));

        if (Math.Sign(rbFallow.velocity.x) > 0 && animationControler.lookLeft)
        {
            animationControler.Flip();
        }

        if (Math.Sign(rbFallow.velocity.x) < 0 && !animationControler.lookLeft)
        {
            animationControler.Flip();
        }
        
        if (Mathf.Abs(rbFallow.velocity.x) > 0 || Mathf.Abs(rbFallow.velocity.z) > 0)
        {
            animationControler.Walk();
        }
        else
        {
            animationControler.Idle();
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(objFalow.transform.position.x, objFalow.transform.position.y + .5f, objFalow.transform.position.z);
        LookDir();
    }
}

