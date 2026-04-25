using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    public GameObject loadingPanel;
    public float overlayTime = 2.5f;

    //bool firstSceneLoaded = false;

    void Start()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        string sceneName = scene.name.ToLower();


        if (sceneName == "main menu")
        {
            loadingPanel.SetActive(false);
            return;
        }

        StartCoroutine(ShowOverlay());
    }

    IEnumerator ShowOverlay()
    {
        if (loadingPanel == null)
            yield break;

        loadingPanel.SetActive(true);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(overlayTime);

        Time.timeScale = 1f;

        loadingPanel.SetActive(false);
    }
}