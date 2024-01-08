using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Nokobot.Assets.Crossbow
{
    public class CrossbowShoot : MonoBehaviour
    {
        public GameObject arrowPrefab;
        public Transform arrowLocation;
        public float shotPower = 100f;
        public float fireRate = 0.5f; // Number of shots per second. Adjust this value as needed.

        private XRGrabInteractable grabInteractable;
        private bool isHeld = false;
        private float nextTimeToFire = 0f;
public AudioClip gunshotSound;
    private AudioSource audioSource;
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
            if (arrowLocation == null)
                arrowLocation = transform;
        }

        void Update()
        {
            if (isHeld && Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                ShootArrow();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }

        void ShootArrow()
        {
            audioSource.PlayOneShot(gunshotSound);

            Instantiate(arrowPrefab, arrowLocation.position, arrowLocation.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * shotPower);
        }

        private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

        
    }
}
