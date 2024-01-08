using UnityEngine;
using UnityEngine.UI; // Add this if you're using basic UI Text
using TMPro;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float CurrentHealth;
    public GameObject damagePopupPrefab; // Assign your damage pop-up prefab here
    public Transform popupSpawnPoint; // This is where the pop-up will appear. You can use the enemy's transform for simplicity.

    private Animator animator;
    private GameManager gameManager;
    private float popupOffset = 1f;
    private const float popupSpacing = 2f; // The space between consecutive popups

    private void Start()
    {
        CurrentHealth = MaxHealth;
        animator = GetComponent<Animator>();

        // Find the game manager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    public void TakeDamage(float damageAmount)
    {
        damageAmount = gameManager.GetDamageWithBuff(damageAmount);
        CurrentHealth -= damageAmount;
        ShowDamagePopup(damageAmount);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    void ShowDamagePopup(float damage)
{
    // Incorporate the offset in the spawn position
    Vector3 spawnPositionWithOffset = popupSpawnPoint.position + new Vector3(0, popupOffset, 0);

    GameObject popupInstance = Instantiate(damagePopupPrefab, spawnPositionWithOffset, Quaternion.identity, popupSpawnPoint);
    TMP_Text damagePopupText = popupInstance.GetComponent<TMP_Text>();
    
    damagePopupText.text = damage.ToString();

    // Make the popup face the main camera
    Vector3 directionToCamera = (popupInstance.transform.position - Camera.main.transform.position).normalized;
    popupInstance.transform.rotation = Quaternion.LookRotation(directionToCamera);

    Destroy(popupInstance, 1f); // Destroy the pop-up after 1 second.

    popupOffset += popupSpacing;
    StartCoroutine(ResetPopupOffset());
}


    private IEnumerator ResetPopupOffset()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second or the same duration as the popup life
        popupOffset = 0f;
    }
    public void Die()
    {
        //animator.SetTrigger("Die");

        // Notify the game manager that this enemy has been defeated
        if (gameManager != null)
        {
            gameManager.OnEnemyDefeated(gameObject);
        }
        // Optionally, add a delay and then destroy the enemy game object
        Destroy(gameObject); // 2 seconds delay before destroying, adjust as per your death animation length
    }
}
