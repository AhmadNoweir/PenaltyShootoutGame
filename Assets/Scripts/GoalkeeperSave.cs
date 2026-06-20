using UnityEngine;
public class GoalkeeperSave : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            FindObjectOfType<GameManager>().ShotSaved();
        }
    }
}