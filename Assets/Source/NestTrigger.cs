using UnityEngine;

public class NestTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Nest"))
        {
            GetComponentInParent<RabbitAgent>().OnNestCollision();
        }
    }
}