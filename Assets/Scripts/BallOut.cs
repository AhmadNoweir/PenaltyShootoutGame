using UnityEngine;
public class BallOut : MonoBehaviour
{
    public float maxDistance = 30f;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (Vector3.Distance(startPos, transform.position) > maxDistance)
        {
            FindObjectOfType<GameManager>().ShotMissed();
        }
    }
}