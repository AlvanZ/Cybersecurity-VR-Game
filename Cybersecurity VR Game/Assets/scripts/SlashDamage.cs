using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    public float damageAmount = 25f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}
