using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    private static CrowdManager _instance;
    
    [SerializeField] private float _progressDuration = 1;
    public static float ProgressDuration => _instance._progressDuration;
    
    [SerializeField] private float _distance = 1;
    public static float Distance => _instance._distance;

    [SerializeField] private float _radius = 10;
    [SerializeField] private float _resolution = 15;
    [SerializeField] private GameObject _crowdPrefab;
    
    private List<CrowdBehaviour> _crowds = new List<CrowdBehaviour>();
    
    private void Awake()
    {
        _instance = this;
        GenerateCrowds();
    }

    private void Start()
    {
        StartCoroutine(AdvanceRoutine());
    }

    private void GenerateCrowds()
    {
        Vector3 radiusDir = transform.forward * _radius;
        Quaternion angle = Quaternion.Euler(0, 360 / _resolution, 0);
        for (int i = 0; i < _resolution; i++)
        {
            _crowds.Add(Instantiate(_crowdPrefab,transform.position+radiusDir,Quaternion.LookRotation(-radiusDir)).GetComponent<CrowdBehaviour>());
            radiusDir = angle * radiusDir;
        }
    }

    private void AdvanceRandomCrowd()
    {
        _crowds[Random.Range(0,_crowds.Count)].Advance();
    }

    private IEnumerator AdvanceRoutine()
    {
        WaitForSeconds interval = new WaitForSeconds(2);
        while (true)
        {
            yield return interval;
            AdvanceRandomCrowd();
        }
    }
}
