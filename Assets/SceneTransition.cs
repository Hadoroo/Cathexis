using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this line
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }
    
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    private Canvas fadeCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeCanvas();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Automatically fade in when new scene loads
        StartCoroutine(FadeIn());
    }

    void InitializeCanvas()
    {
        fadeCanvas = GetComponentInChildren<Canvas>();
        if (fadeCanvas == null)
        {
            GameObject canvasObj = new GameObject("FadeCanvas");
            fadeCanvas = canvasObj.AddComponent<Canvas>();
            fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.transform.SetParent(transform);
            
            // Create image
            GameObject imageObj = new GameObject("FadeImage");
            fadeImage = imageObj.AddComponent<Image>();
            imageObj.transform.SetParent(canvasObj.transform);
            
            // Setup image
            fadeImage.color = Color.clear;
            RectTransform rt = fadeImage.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    public IEnumerator FadeOut()
    {
        fadeImage.raycastTarget = true;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.clear, Color.black, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = Color.black;
    }

    public IEnumerator FadeIn()
    {
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = false;
        
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(Color.black, Color.clear, timer / fadeDuration);
            yield return null;
        }
        fadeImage.color = Color.clear;
    }
}