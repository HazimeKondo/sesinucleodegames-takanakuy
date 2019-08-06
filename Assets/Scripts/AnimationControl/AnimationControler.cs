using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DragonBones;

[RequireComponent(typeof(UnityArmatureComponent))]
[RequireComponent(typeof(Fallow))]
public class AnimationControler : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public string tAnimation = "";
    public float timeScale = 1;
    public bool lookLeft = false;
    public string curAnimation = "";

    private void Awake()
    {
        armature = GetComponent<UnityArmatureComponent>();
    }

    private void Update()
    {
     //   Debug.Log(curAnimation);
    }

    public void Play(string animationName, int playTimes = 0, float timeScale = 1)
    {
        armature.animation.timeScale = timeScale;

        if (!IsPlaying(animationName))
        {
            curAnimation = animationName;
            armature.animation.Play(animationName, playTimes);
        }
    }

    public void Stop()
    {
        armature.animation.Stop();
    }

    public bool IsPlaying(string animationName)
    {
        if (animationName.Equals(curAnimation))
        {
            return true;
        }
        return false;
    }

    public void Idle(int timeScale = 1, int playTimes = 0)
    {
        Play("idle", playTimes, timeScale);
    }

    public void Walk(int timeScale = 1, int playTimes = 0)
    {
        Play("walk", playTimes, timeScale);
    }

    public void ToWhip(int timeScale = 1, int playTimes = 0)
    {
        Play("whip", playTimes, timeScale);
    }

    public void Run(int timeScale = 1, int playTimes = 0)
    {
        Play("run", playTimes, timeScale);
    }

    public void Flip()
    {
        lookLeft = !lookLeft;
        Vector3 xscale = armature.transform.localScale;
        xscale.x *= -1;
        armature.transform.localScale = xscale;
    }

    public void Flip(float x, float deadZone = .2f)
    {
        // lookLeft = !lookLeft;
        Vector3 xscale = armature.transform.localScale;
        if (x > deadZone && lookLeft)
        {
            xscale.x *= -1;
            lookLeft = false;
        }
        else if (x < deadZone && !lookLeft)
        {
            xscale.x *= -1;
            lookLeft = true;
        }


        armature.transform.localScale = xscale;
    }
}
