using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Collections;

public class GrenadeLauncherShoot : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform grenadeLocation;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
        private bool isReloading = false;
public AudioClip gunshotSound;
    private AudioSource audioSource;
    public float shotPower = 50f;

    public int maxAmmo = 5; // Set the maximum number of grenades
    private int ammoCount;

    public TMP_Text ammoDisplay; // Drag and drop the TextMeshPro object displaying ammo count

    private float reloadTime = 2f; // 2 seconds delay between shots
    private float nextTimeToShoot = 0f;

    void Awake()
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

    void Start()
    {
        if (grenadeLocation == null)
            grenadeLocation = transform;

        ammoCount = maxAmmo; // Initially, the grenade launcher is full
        UpdateAmmoDisplay();
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
    {
        if (isReloading)
            return;

        if (isHeld && (ammoCount <= 0 || Input.GetKeyDown(KeyCode.R)))
        {
            StartCoroutine(Reload());
            return;
        }
        if (isHeld && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToShoot && ammoCount > 0)
        {
            nextTimeToShoot = Time.time + reloadTime;
            audioSource.PlayOneShot(gunshotSound);
            Instantiate(grenadePrefab, grenadeLocation.position, grenadeLocation.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 50, ForceMode.Impulse);
            ammoCount--; // Decrease the ammo count
            UpdateAmmoDisplay();
        }
    }
IEnumerator Reload()
    {
        isReloading = true;
        print("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        ammoCount = maxAmmo;
        isReloading = false;
        UpdateAmmoDisplay();
        print("Reloaded!");
    }
    void UpdateAmmoDisplay()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.text = ammoCount.ToString()+"/"+maxAmmo;
        }
    }
}
