using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using TMPro;

public class HomingOrbPistol : MonoBehaviour
{
    public GameObject[] orbPrefabs;  // Array of orb prefabs that can be spawned
    public Transform shootPoint;  // The point from which the gun shoots, usually at the end of the barrel.
    public float spawnDistance = 10f;  // The distance from the gun where the orb will appear.

    public int maxAmmo = 6; // Maximum ammo the pistol can hold.
    public float fireDelay = 2f; // Delay between shots in seconds.
    public float reloadTime = 2.0f; // Time taken to reload in seconds.

    private int currentAmmo;
    private float nextFireTime;
        private bool isReloading = false;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
public AudioClip gunshotSound;
    private AudioSource audioSource;
        public TMP_Text ammoDisplay; // Drag and drop the TextMeshPro object displaying ammo count

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
        currentAmmo = maxAmmo; // Initialize with max ammo.
        UpdateAmmoDisplay();
    }
void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = currentAmmo.ToString()+"/"+maxAmmo;
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

    void Update()
    {if (isReloading)
            return;

        if (isHeld && (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R)))
        {
            StartCoroutine(Reload());
            return;
        }
        if (isHeld && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            SpawnOrb();
                        audioSource.PlayOneShot(gunshotSound);

            nextFireTime = Time.time + fireDelay;
            currentAmmo--;
            UpdateAmmoDisplay();

            // You can add logic here to play a sound or provide feedback on low ammo, etc.
        }
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        print("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoDisplay();
        print("Reloaded!");
    }

    void SpawnOrb()
{
    Vector3 spawnLocation = shootPoint.position + shootPoint.forward * spawnDistance;
    GameObject selectedOrb = orbPrefabs[Random.Range(0, orbPrefabs.Length)]; // Select a random orb prefab
    GameObject spawnedOrb = Instantiate(selectedOrb, spawnLocation, Quaternion.identity);
    
    // Make the orb face the main camera
    Vector3 directionToCamera = (spawnLocation - Camera.main.transform.position).normalized;
    spawnedOrb.transform.rotation = Quaternion.LookRotation(directionToCamera);

    // Destroy the spawned orb after 3 seconds
    Destroy(spawnedOrb, 6f);
}

}
