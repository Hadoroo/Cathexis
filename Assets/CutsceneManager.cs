using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextScene = "MainMenu"; // Scene to load after video ends
    public bool useFadeTransition = true; // Enable/disable fade effect
    private SceneTransition sceneTransition;

    void Start()
    {
        // Get the SceneTransition if it exists
        if (useFadeTransition)
        {
            sceneTransition = FindObjectOfType<SceneTransition>();
        }

        // Ensure the videoPlayer is assigned
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("No VideoPlayer assigned or found on this GameObject!");
                return;
            }
        }

        // Subscribe to the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // Called when the video finishes playing
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video ended! Loading next scene...");
        
        if (useFadeTransition && sceneTransition != null)
        {
            StartCoroutine(TransitionToNextScene());
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private IEnumerator TransitionToNextScene()
    {
        // Fade out
        yield return sceneTransition.FadeOut();
        
        // Load the next scene
        SceneManager.LoadScene(nextScene);
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}