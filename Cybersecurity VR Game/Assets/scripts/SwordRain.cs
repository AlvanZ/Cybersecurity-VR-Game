using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SwordRain : MonoBehaviour
{
    public GameObject[] swordPrefabs; // Drag your sword prefabs here
    public float spawnRate = 0.5f; // Time interval between spawns
    public float forwardOffset = 2f; // How far in front of the player the swords start
    public float downwardAngle = 45f; // Angle of descent for the swords
    public GameObject swordSpawnVFXPrefab;
public GameObject spawnZoneReference;  // Drag the SpawnZoneReference GameObject here
public AudioClip gunshotSound;
    private AudioSource audioSource;
    public float swordSpeed = 40f; // Adjust this value as needed

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    private bool isRaining = false;
    public Vector3 vfxRotationOffset = new Vector3(90, 0, 0);

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
        StopRaining();
    }

    private void Update()
    {
        if (isHeld && Input.GetButtonDown("Fire1"))
        {
            StartRaining();
        }
        else if (isHeld && Input.GetButtonUp("Fire1"))
        {
            StopRaining();
        }
    }

    private void StartRaining()
    {
        if (!isRaining)
        {
            isRaining = true;
            InvokeRepeating("SpawnSword", 0, spawnRate);
        }
    }

    private void StopRaining()
    {
        isRaining = false;
        CancelInvoke("SpawnSword");
    }

    private void SpawnSword()
{
    GameObject randomSwordPrefab = swordPrefabs[Random.Range(0, swordPrefabs.Length)];

    // Define the zone dimensions
    float zoneWidth = 40f;  // Modify as needed
    float zoneHeight = 40f; // Modify as needed

    // Calculate the top-left-back corner of the zone from the center-back point
    Vector3 zoneStart = spawnZoneReference.transform.position;


    // Choose a random position within the zone
    Vector3 spawnPosition = zoneStart + new Vector3(Random.Range(-zoneWidth * 0.5f, zoneWidth * 0.5f), 
                                                Random.Range(-zoneHeight * 0.5f, zoneHeight * 0.5f), 
                                                0);  // No depth offset, as the reference object represents the center.


    // Calculate the direction the sword should be moving in
    Vector3 direction = Quaternion.AngleAxis(-downwardAngle, Camera.main.transform.right) * Camera.main.transform.forward;

    // Calculate the rotation to make the sword face the direction it's being launched towards
    Quaternion spawnRotation = Quaternion.LookRotation(direction);

    // Instantiate the sword and set its velocity
    GameObject swordInstance = Instantiate(randomSwordPrefab, spawnPosition, spawnRotation);
    Rigidbody rb = swordInstance.GetComponent<Rigidbody>();
    if (rb)
    {
        rb.velocity = direction * swordSpeed; // Assuming you have a variable for swordSpeed
    }

    // Instantiate and destroy the VFX prefab if it exists
    if (swordSpawnVFXPrefab)
    {
            audioSource.PlayOneShot(gunshotSound);

        Quaternion vfxRotation = Quaternion.LookRotation(direction, -Camera.main.transform.up) * Quaternion.Euler(vfxRotationOffset);
        GameObject vfxInstance = Instantiate(swordSpawnVFXPrefab, spawnPosition, vfxRotation);
        Destroy(vfxInstance, 0.5f);
    }

    // Destroy the sword instance after 1 second
    Destroy(swordInstance, 4f);
}




}
