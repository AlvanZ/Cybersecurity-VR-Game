using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    public float damage = 100f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect; // Drag a prefab for visual effects of explosion
    private Rigidbody rb;
    private void Start()
    {
        Destroy(gameObject, 5f); // Destroy the arrow after 5 seconds

        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        // Show explosion effect
        if (explosionEffect)
        {

            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Detect all the colliders in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Damage all enemies in the explosion radius
        foreach (Collider nearbyObject in colliders)
        {
            EnemyHealth enemy = nearbyObject.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Modify this if you want damage fall-off based on distance
            }
        }

        // Destroy the grenade
        Destroy(gameObject);
    }
}
