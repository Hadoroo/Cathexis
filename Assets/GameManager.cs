using UnityEngine;
using UnityEngine.Video; // Required for VideoPlayer
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public List<string> sceneOrder;
    private SceneTransition sceneTransition;
    private const string SpawnPointTag = "GameManagerSpawnPoint";
    private VideoPlayer videoPlayer; // Reference to the VideoPlayer in "Cutscene"

    void Awake()
    {
        sceneOrder = new List<string> { "MainMenu", "PoliceStation1", "Market", "ToolShop",
                                      "PoliceStation2", "Bar", "PoliceStation3",
                                      "Court", "PoliceStation4", "Cutscene" };
        
        sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition == null)
        {
            Debug.LogError("No SceneTransition found in the scene!");
        }

        // Find the spawn point and move to it
        GameObject spawnPoint = GameObject.FindWithTag(SpawnPointTag);
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning($"No GameObject with tag '{SpawnPointTag}' found in the scene.");
        }

        // If this is the Cutscene, get the VideoPlayer and set up its completion event
        if (SceneManager.GetActiveScene().name == "Cutscene")
        {
            videoPlayer = FindObjectOfType<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.loopPointReached += OnVideoEnd; // Trigger when video ends
            }
            else
            {
                Debug.LogError("No VideoPlayer found in the Cutscene!");
            }
        }
    }

    void Start()
    {
        if (sceneTransition != null)
        {
            StartCoroutine(sceneTransition.FadeIn());
        }
    }

    // Called when the video finishes playing
    private void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        if (sceneTransition != null)
        {
            yield return sceneTransition.FadeOut(); // Fade to black
        }
        SceneManager.LoadScene("MainMenu"); // Load Main Menu
    }

    public IEnumerator LoadNextSceneWithFade()
    {
        if (sceneTransition == null) yield break;
        
        yield return sceneTransition.FadeOut();

        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = sceneOrder.IndexOf(currentScene);

        if (currentIndex >= 0 && currentIndex < sceneOrder.Count - 1)
        {
            SceneManager.LoadScene(sceneOrder[currentIndex + 1]);
        }
        else
        {
            Debug.Log("No next scene available");
            yield return sceneTransition.FadeIn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextSceneWithFade());
        }
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