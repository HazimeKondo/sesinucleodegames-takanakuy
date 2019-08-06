using System.Collections;
using UnityEngine;

public class CrowdBehaviour : MonoBehaviour
{
    private Vector3 rootOrigin;
    private bool canBeHit = true;
    
    private void Start()
    {
        rootOrigin = transform.position;
        GameManager.Instance.OnEndPlay += () => canBeHit = false;
    }
    
    [Button]
    public void Advance()
    {
        StopAllCoroutines();
        StartCoroutine(ProgressRoutine(CrowdManager.Distance));
    }
    
    [Button]
    public void Hit()
    {
        if(canBeHit)
            Retreat(CrowdManager.Distance);
    }
    
    private void Retreat(float distance)
    {
        StopAllCoroutines();
        distance = Mathf.Clamp(distance, 0, Vector3.Distance(transform.position, rootOrigin));
        transform.Translate(Vector3.back*distance);
    }
    
    private IEnumerator ProgressRoutine(float distance)
    {
        Vector3 initialPos = transform.position;
        Vector3 desiredPos = initialPos + transform.forward * distance;
        float interpolation = 0;
        while (interpolation < 1)
        {
            interpolation += Time.deltaTime / CrowdManager.ProgressDuration;
            transform.position = Vector3.Lerp(initialPos,desiredPos,interpolation);
            yield return null;
            if (Vector3.Distance(rootOrigin, transform.position) >= 10)
            {
                GameManager.Instance.StopPlay();
            }
        }
        transform.position = desiredPos;
    }


}
