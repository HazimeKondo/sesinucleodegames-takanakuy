using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DragonBones;

[RequireComponent(typeof(UnityArmatureComponent))]
public class AnimationControler : MonoBehaviour
{
    public UnityArmatureComponent armature;
    public string tAnimation = "";
    public float timeScale = 1;

    private void Awake()
    {
        armature = GetComponent<UnityArmatureComponent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Walk();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Idle();
        }
    }

    public void Play(string animationName, int playTimes = 0, float timeScale = 1)
    {
        armature.animation.timeScale = timeScale;
        armature.animation.Play(animationName, playTimes);
    }

    public void Stop()
    {
        armature.animation.Stop();
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
        Vector3 xscale = armature.transform.localScale;
        xscale.x *= -1;
        armature.transform.localScale = xscale;
    }
}
