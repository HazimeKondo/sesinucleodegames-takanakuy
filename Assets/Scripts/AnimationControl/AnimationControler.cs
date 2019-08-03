using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using DragonBones;

public class AnimationControler : MonoBehaviour
{
    public UnityArmatureComponent armature;
    //public List<string> _animationNames = Armature.

    public void Play(string animationName, int playTimes = 0, int timeScale = 1)
    {
        armature.animation.timeScale = timeScale;
        armature.animation.Play(animationName, playTimes);
       // var a=armature.animation.animationNames;
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

    [ContextMenu("aaaaa")]
    public void Run(int timeScale = 1, int playTimes = 0)
    {
        Play("run", playTimes, timeScale);
    }
}
