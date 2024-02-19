using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed;
    public float damage;
    public float lifetime = 10f; // How long the bullet lasts before automatically destroying

public void SetBulletProperties(float bulletSpeed, float bulletDamage)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
    }    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits an enemy
        EnemyHealth enemy = collision.transform.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            
        }
        // Destroy bullet on collision
        Destroy(gameObject);
    }
}
