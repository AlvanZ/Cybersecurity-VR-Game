using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ShootableButton : MonoBehaviour
{
    public UnityEvent OnButtonShot;  // Event to notify when the button is shot
    public AudioClip glassBreakSound;  // Drag and drop your glass break sound here
    public int answerIndex;  // This will store the index of the answer this button represents
    public GameObject effectPrefab; // Assign the effect prefab in the inspector

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Bullet"))
    {
        print("got hit");

        // Play the glass break sound
        if (glassBreakSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(glassBreakSound);
        }
        
        if (OnButtonShot != null)
        {
            OnButtonShot.Invoke();
        }
        
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
        
        // Disable the button after everything else
        gameObject.SetActive(false);
    }
}

}
