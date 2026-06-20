using UnityEngine;
using System.Collections;
public class Goalkeeper : MonoBehaviour
{
    public float minDiveDistance = 0.5f;
    public float maxDiveDistance = 1f;
    public float diveSpeed = 1f;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }
    public void Dive()
    {
        int choice = Random.Range(0, 3);
        float distance = Random.Range(minDiveDistance, maxDiveDistance);
        Vector3 target = startPosition;
        if (choice == 0)
            target = startPosition - transform.right * distance;
        else if (choice == 1)
            target = startPosition + transform.right * distance;
        else
            target = startPosition;
        StopAllCoroutines();
        StartCoroutine(Move(target));
    }
    IEnumerator Move(Vector3 target)
    {
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(transform.position, target, t);
            t += Time.deltaTime * diveSpeed;
            yield return null;
        }
    }
    public void ResetGoalkeeper()
    {
        StopAllCoroutines();
        transform.position = startPosition;
    }
}