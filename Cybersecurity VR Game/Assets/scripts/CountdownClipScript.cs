using UnityEngine;
using UnityEngine.Video;

public class CountdownClipScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject slideshowObject; // Drag the object with the Slideshow script here.

    private void Start()
    {
        slideshowObject.SetActive(false); // Ensure the slideshow object is inactive at the start.
        videoPlayer.loopPointReached += OnCountdownFinished;
        videoPlayer.Play();
    }

    private void OnCountdownFinished(VideoPlayer vp)
    {
        slideshowObject.SetActive(true); // Activate the slideshow object.
        this.gameObject.SetActive(false); // Disable this object.
    }
}
