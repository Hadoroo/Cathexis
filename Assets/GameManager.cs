using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<string> sceneOrder;

    public Collider2D switchSceneTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            sceneOrder = new List<string> { "PoliceStation1", "Market", "ToolShop",
                                          "PoliceStation2", "Bar", "PoliceStation3",
                                          "Court", "PoliceStation4" };
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Start with fade in when scene loads
        StartCoroutine(SceneTransition.Instance.FadeIn());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadNextSceneWithFade());
        }
    }

    public IEnumerator LoadNextSceneWithFade()
    {
        // Fade to black
        yield return SceneTransition.Instance.FadeOut();

        string currentScene = SceneManager.GetActiveScene().name;
        int currentIndex = sceneOrder.IndexOf(currentScene);

        if (currentIndex >= 0 && currentIndex < sceneOrder.Count - 1)
        {
            // SceneManager.LoadScene will trigger OnSceneLoaded which handles fade-in
            SceneManager.LoadScene(sceneOrder[currentIndex + 1]);
        }
        else
        {
            Debug.Log("No next scene available");
            // Manually fade in if not changing scenes
            yield return SceneTransition.Instance.FadeIn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNextSceneWithFade());
        }
    }
}
