using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;


public class KatanaOrbSpawner : MonoBehaviour
{
    public GameObject orbPrefab; // The orb prefab
    public Transform katanaHandle; // The base/handle of the katana
    public Transform swordTip; // The transform for the sword's tip
    public float spawnDistance = 3f; // Distance the sword tip has to travel to spawn an orb
    public float orbMoveSpeed = 3f; // Speed at which the orb moves outward

    private bool isFiring = false; // Whether Fire1 is being held down
    private Vector3 lastSpawnPoint; // The last point where an orb was spawned
    public AudioClip gunshotSound;
    private AudioSource audioSource;
    private bool isHeld = false;

    private XRGrabInteractable grabInteractable;
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
    void Update()
    {
        // Check if the katana is held and Fire1 is pressed
        if (isHeld && Input.GetButtonDown("Fire1"))
        {
            isFiring = true;
            lastSpawnPoint = swordTip.position;
        }
        // Check if Fire1 is released
        else if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }

        if (isFiring)
        {
            // Calculate the distance traveled by the sword tip since the last spawn
            float distance = Vector3.Distance(swordTip.position, lastSpawnPoint);

            if (distance >= spawnDistance)
            {
                audioSource.PlayOneShot(gunshotSound);
                SpawnOrb();
                lastSpawnPoint = swordTip.position;
            }
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
    void SpawnOrb()
    {
        // Instantiate the orb at the sword tip's position
        GameObject orb = Instantiate(orbPrefab, swordTip.position, Quaternion.identity);

        // Get the direction from katana handle to sword tip
        Vector3 moveDirection = (swordTip.position - katanaHandle.position).normalized;

        // Start the movement of the orb
        StartCoroutine(MoveOrbOutward(orb.transform, moveDirection));
        Destroy(orb, 5f);

    }

    private IEnumerator MoveOrbOutward(Transform orbTransform, Vector3 moveDirection)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            // Check if the orb still exists
            if (orbTransform == null)
                yield break;  // Exit the coroutine if the orb has been destroyed

            orbTransform.position += moveDirection * orbMoveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
