using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallow : MonoBehaviour
{
    public GameObject objFalow;
    public Rigidbody rbFallow;
    [SerializeField]Vector3 offSet = new Vector3(0, .7f, 0);
    [SerializeField]AnimationControler animationControler;

    void Awake()
    {
        transform.position = objFalow.transform.position + offSet;
        animationControler = GetComponent<AnimationControler>();
        rbFallow = objFalow.GetComponent<Rigidbody>();
    }

    void LookDir()
    {
        //Debug.Log(Math.Sign(rbFallow.velocity.z));

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

    private void Update()
    {
        transform.position = objFalow.transform.position + offSet;
        LookDir();
    }
}

