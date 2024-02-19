using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private float collisionEnableDelay = 1f; // Delay to enable collision

    private void Awake()
    {
        Destroy(gameObject, 5f); // Destroy the arrow after 5 seconds
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Since the arrow is being instantiated when it's fired, set up its behavior immediately
        rb.useGravity = true;
        col.isTrigger = true;
        StartCoroutine(DelayedCollision());
    }

    private IEnumerator DelayedCollision()
    {
        yield return new WaitForSeconds(collisionEnableDelay);
        col.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // You can add more behavior here, like sticking the arrow to a target, etc.
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(50);  // Example damage value
            
        }
        Destroy(gameObject);  // Optionally, destroy the arrow after it deals damage
    }
}
