using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fader;

public class Fade
{
    private static FadeEffect _fadeEffect;
    
    private static FadeEffect FadeFX => _fadeEffect == null ? _fadeEffect = new GameObject("FadeEffect").AddComponent<FadeEffect>(): _fadeEffect;
    
    public static void To(Color color, float duration = 0)
    {
        duration = Math.Abs(duration);
        FadeFX.StartFade(color, duration);
    }
}