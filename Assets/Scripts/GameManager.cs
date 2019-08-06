using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    private bool _playerIsAvailable;

    public Action<int> OnTimeUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        Destroy(gameObject);
    }

    private IEnumerator GameLoop()
    {
        yield return Initialize();
        yield return Playing();
        yield return Ending();

    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(3);
    }

    private IEnumerator Playing()
    {
        _playerIsAvailable = true;
        WaitForSeconds oneSecond = new WaitForSeconds(1);
        int timeInSeconds = 0;
        
        while (_playerIsAvailable)
        {
            OnTimeUpdated.Invoke(timeInSeconds++);
            yield return oneSecond;
        }
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(3);
    }
}
