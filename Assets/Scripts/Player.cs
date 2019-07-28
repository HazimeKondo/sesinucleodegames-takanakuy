using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;

    private PlayerControls _input;
    public static PlayerControls Input => _instance._input;
    
    private void Awake()
    {
        _instance = this;
        _input = new PlayerControls();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
