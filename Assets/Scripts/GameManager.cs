﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _playerIsAvailable;

    public Action OnStartPlay;
    public Action<int> OnTimeUpdated;
    public Action OnEndPlay;
    
    void Awake()
    {
//        if (Instance == null)
//        {
//            Instance = this; 
//            DontDestroyOnLoad(gameObject);
//            return;
//        }
//        
//        Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StopPlay()
    {
        _playerIsAvailable = false;
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
        
        OnStartPlay.Invoke();
        
        while (_playerIsAvailable)
        {
            OnTimeUpdated.Invoke(timeInSeconds++);
            yield return oneSecond;
        }
        
        OnEndPlay.Invoke();
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(3);
    }
}
