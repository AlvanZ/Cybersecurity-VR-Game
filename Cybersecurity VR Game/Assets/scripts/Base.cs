using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    public float baseHealth = 100f; // Total health of the base
    public GameplayDataManager gameplayDataManager; // Reference to GameplayDataManager script

    private void Start()
    {
        // Find the GameplayDataManager in the scene (assuming there's only one)
        gameplayDataManager = FindObjectOfType<GameplayDataManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is an enemy
        if (other.CompareTag("Enemy")) // Ensure your enemy prefabs have the tag "Enemy" assigned
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth)
            {
                enemyHealth.Die();
            }

            // Apply damage to the base
            TakeDamage(10f); // You can adjust the damage value as needed
        }
    }

    void TakeDamage(float damageAmount)
    {
        baseHealth -= damageAmount;

        // Optional: Display base health in UI or play some feedback

        if (baseHealth <= 0)
        {
            // Base is destroyed
            // Save game data before loading the outro scene
            if (gameplayDataManager)
            {
                gameplayDataManager.SaveGameData();
            }

            // Load the outro scene
            SceneManager.LoadScene("outtro");
        }
    }
}
