using UnityEngine;
using System.Collections;

public class OrbBulletBehavior2 : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 10f;
    public float initialDuration = 0.5f; // Duration for which the bullet moves in a random direction.
    private Vector3 randomDirection;
    private Transform target;
    private bool isHoming = false;

    private void Start()
    {
        target = FindClosestEnemy();
        randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
Destroy(gameObject,5f);
        StartCoroutine(StartHoming());
    }

    private void Update()
    {
        if (isHoming && target != null)
        {
            Vector3 moveDirection = (target.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        else
        {
            transform.position += randomDirection * speed * Time.deltaTime;
        }
    }
public void SetTarget(GameObject targetGameObject)
    {
        target = targetGameObject.transform;
    }
    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }

    private IEnumerator StartHoming()
    {
        yield return new WaitForSeconds(initialDuration);
        isHoming = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Assuming the enemy has a script called EnemyHealth to manage its health.
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            Destroy(gameObject); // Destroy the bullet after it hits the enemy.
        }
    }
}
