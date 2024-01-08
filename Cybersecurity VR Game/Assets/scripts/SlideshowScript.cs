using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideshowScript : MonoBehaviour
{
    public Material[] slideshowMaterials; // Drag your slideshow materials here.
    public AudioClip[] voiceLines;        // Drag your voice line audio clips here.
    public Renderer screenRenderer;      // Drag the renderer of the curved TV or plane.
    public AudioSource audioSource;      // An AudioSource to play the voice lines.

    private int currentSlide = 0;

    private void Start()
    {
        StartSlideshow();
    }

    private void StartSlideshow()
    {
        if (currentSlide < slideshowMaterials.Length)
        {
            screenRenderer.material = slideshowMaterials[currentSlide]; // Set the material.
            audioSource.clip = voiceLines[currentSlide];
            audioSource.Play();
            currentSlide++;
            Invoke("StartSlideshow", audioSource.clip.length); // Continue the slideshow when the voice line finishes.
        }
        else
        {
            // If you want to transition to a new scene after the slideshow
            // Uncomment the line below and replace "YourSceneName" with your scene's name
            SceneManager.LoadScene("Map");
        }
    }
}
