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
    [SerializeField] private float _interval = 2;

    [SerializeField] private float _radius = 10;
    [SerializeField] private float _resolution = 15;
    [SerializeField] private GameObject _crowdPrefab;

    private LineRenderer[] _groundLines;

    private List<CrowdBehaviour> _crowds = new List<CrowdBehaviour>();

    private void Awake()
    {
        _instance = this;
        GenerateCrowds();
    }

    private void Start()
    {
        
        
        GameManager.Instance.OnTimeUpdated += (t) =>
        {
            if (t != 0)
            {
                if (t % 5 == 0)
                {
                    _interval *= 0.9f;
                    _progressDuration *= 0.9f;
                }

                if (t % 8 == 0)
                {
                    _distance *= 1.1f;
                }
            }
        };

        GameManager.Instance.OnStartPlay += () => StartCoroutine(AdvanceRoutine());
        GameManager.Instance.OnEndPlay += StopAllCoroutines;
    }

    private void GenerateCrowds()
    {
        _groundLines = GetComponentsInChildren<LineRenderer>();
        
        Vector3 radiusDir = transform.forward;
        Quaternion angle = Quaternion.Euler(0, 360 / _resolution, 0);
        for (int i = 0; i < _resolution; i++)
        {
            _crowds.Add(Instantiate(_crowdPrefab, transform.position + radiusDir *_radius, Quaternion.LookRotation(-radiusDir))
                .GetComponent<CrowdBehaviour>());

            _groundLines[0].positionCount++;
            _groundLines[0].SetPosition(i,transform.position + radiusDir *_radius);

            _groundLines[1].positionCount++;
            _groundLines[1].SetPosition(i,transform.position + radiusDir *(_radius-10));
            
            radiusDir = angle * radiusDir;
        }

        _groundLines[0].positionCount++;
        _groundLines[0].SetPosition(_groundLines[0].positionCount-1,_groundLines[0].GetPosition(0));
        _groundLines[1].positionCount++;
        _groundLines[1].SetPosition(_groundLines[0].positionCount-1,_groundLines[1].GetPosition(0));        
    }

    private void AdvanceRandomCrowd()
    {
        _crowds[Random.Range(0, _crowds.Count)].Advance();
    }

    private IEnumerator AdvanceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);
            AdvanceRandomCrowd();
        }
    }
}