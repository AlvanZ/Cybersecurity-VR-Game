using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using TMPro;


public class SimpleRifle : MonoBehaviour
{
    public float bulletSpeed = 100f;  // Example speed
    public float bulletDamage = 10f; // Example damage
    public GameObject bulletPrefab;  // Drag the bullet prefab here
    public Transform gunTip;
    public float fireRate = 10f;
    public ParticleSystem muzzleFlashEffect;
    private float nextTimeToFire = 0f;
    private bool isHeld = false;  // Flag to check if the gun is being held
    private XRGrabInteractable grabInteractable;
    public AudioClip gunshotSound;
    private AudioSource audioSource;
    //ammo
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;
    public TextMeshProUGUI ammoText;


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
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
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
        if (isReloading)
            return;

        if (isHeld && (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R)))
        {
            StartCoroutine(Reload());
            return;
        }

        if (isHeld && Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
                currentAmmo--;
                UpdateAmmoUI();
            }
        }
    }
     void UpdateAmmoUI()
    {
        ammoText.text =  currentAmmo + "/" + maxAmmo;
    }
    IEnumerator Reload()
    {
        isReloading = true;
        print("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoUI();
        print("Reloaded!");
    }
    void Shoot()
{
    // Create the bullet, set its properties, and set its direction
    GameObject bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
    
    BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
    bulletBehavior.SetBulletProperties(bulletSpeed, bulletDamage);
    
    bullet.GetComponent<Rigidbody>().velocity = gunTip.forward * bulletBehavior.speed;

    // gunAnimator.SetTrigger("Shot");
    muzzleFlashEffect.Play();
    audioSource.PlayOneShot(gunshotSound);
}



}
