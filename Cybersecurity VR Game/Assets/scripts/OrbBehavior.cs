using UnityEngine;
using System.Collections;

public class OrbBehavior : MonoBehaviour
{
    public GameObject energyBulletPrefab;
    public float shootInterval = 2f;
    public int bulletsToShoot = 3;
    public AudioClip gunshotSound;
    private AudioSource audioSource;
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        StartCoroutine(ShootAtEnemies());

    }

    IEnumerator ShootAtEnemies()
    {
        while (bulletsToShoot > 0)
        {
            if (GameManager.Instance.activeEnemies.Count > 0)
            {
                GameObject targetEnemy = GameManager.Instance.activeEnemies[Random.Range(0, GameManager.Instance.activeEnemies.Count)];
                Shoot(targetEnemy);
                bulletsToShoot--;
            }
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void Shoot(GameObject target)
    {
        audioSource.PlayOneShot(gunshotSound);

        // Instantiate energy bullet and set its target
        GameObject bullet = Instantiate(energyBulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<OrbBulletBehavior>().SetTarget(target);
    }
}
