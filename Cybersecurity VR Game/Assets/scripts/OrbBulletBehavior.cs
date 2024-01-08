using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbBulletBehavior : MonoBehaviour
{
    public float speed = 500f;
        public float damage = 25f; // Damage dealt to enemies

    private GameObject target;

private void Start()
    {
        Destroy(gameObject, 10f); // Destroy the arrow after 5 seconds
    }
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
    if (enemy)
    {
        enemy.TakeDamage(damage);
    }
    
    Destroy(gameObject); // Destroy the bullet once it hits something
}

}
