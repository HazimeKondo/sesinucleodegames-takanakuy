using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMusicPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.time = Random.Range(0, audioSource.clip.length);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
