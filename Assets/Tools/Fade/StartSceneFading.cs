using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartSceneFading : MonoBehaviour
{
    [SerializeField] private float _delay = 0.2f;
    [SerializeField] private float _duration = 1f;

    private void Start()
    {
        Fade.To(Color.black,0f);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_delay);
        Fade.To(Color.clear, _duration);
    }
}