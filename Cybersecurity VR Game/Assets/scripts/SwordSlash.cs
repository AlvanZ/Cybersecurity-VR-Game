using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwordSlash : MonoBehaviour
{
    public GameObject slashParticleSystemPrefab;
    public Transform swordHandle; // The base/handle of the katana
    public Transform swordTip; // The transform for the sword's tip
    private bool isSlashing = false;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;  // Flag to check if the sword is being held
    public AudioClip gunshotSound;
    private AudioSource audioSource;

    private Vector3 lastSpawnPoint;  // Last position where a particle was spawned
    private Vector3 lastPosition;  // Last position of the sword in the previous frame
    public float spawnDistance = 0.1f;  // Minimum distance the tip has to travel to spawn a particle

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    private void Update()
    {
        // Check if the katana is held and Fire1 is pressed
        if (isHeld && Input.GetButtonDown("Fire1"))
        {
            isSlashing = true;
            lastSpawnPoint = swordTip.position;
            lastPosition = transform.position;
        }
        // Check if Fire1 is released
        else if (Input.GetButtonUp("Fire1"))
        {
            isSlashing = false;
        }

        if (isSlashing)
        {
            // Calculate the distance traveled by the sword tip since the last spawn
            float distance = Vector3.Distance(swordTip.position, lastSpawnPoint);

            if (distance >= spawnDistance)
            {
                audioSource.PlayOneShot(gunshotSound);
                LaunchSlash();
                lastSpawnPoint = swordTip.position;
            }
        }

        lastPosition = transform.position;  // Update the last position for the next frame
    }

    void LaunchSlash()
{
    Vector3 slashDirection = (transform.position - lastPosition).normalized;

    // Check if the slashDirection is a zero vector
    if (slashDirection == Vector3.zero)
    {
        // Provide a default direction if needed (e.g., forward)
        slashDirection = transform.forward;
    }

    slashDirection = AdjustForNearbyTargets(slashDirection);

    GameObject slash = Instantiate(slashParticleSystemPrefab, transform.position, Quaternion.LookRotation(slashDirection));
    Rigidbody rb = slash.GetComponent<Rigidbody>();
    if (rb)
    {
        rb.velocity = slashDirection * 25f; // adjust the speed as required
    }
    Destroy(slash, 5f);
}


Vector3 AdjustForNearbyTargets(Vector3 originalDirection)
{
    float detectionRadius = 55f;  // Adjust based on your game's needs
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
    float closestDistanceSqr = Mathf.Infinity;
    Transform closestTarget = null;

    foreach (Collider hitCollider in hitColliders)
    {
        if (hitCollider.CompareTag("Enemy"))  // Assuming enemies have the tag "Enemy"
        {
            float distanceToTarget = (hitCollider.transform.position - transform.position).sqrMagnitude;
            if (distanceToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceToTarget;
                closestTarget = hitCollider.transform;
            }
        }
    }

    // If a close target was found and it's roughly in the direction of the original slash, adjust the direction
    if (closestTarget != null)
    {
        Vector3 directionToTarget = (closestTarget.position - transform.position).normalized;
        if (Vector3.Dot(originalDirection, directionToTarget) > 0.4f)  // Adjust this threshold as needed
        {
            print("helped");
            return directionToTarget;
        }
    }

    return originalDirection;
}

}
