using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    private float damageAmount = 5f;  // The amount of damage this sword deals

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has an EnemyHealth script (or whatever script manages health for your enemies)
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth)
        {
            enemyHealth.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        
    }
}
