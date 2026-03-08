using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;

    public GameObject loadingPanel;
    public float overlayTime = 2.5f;

    bool firstSceneLoaded = false;

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
        if (!firstSceneLoaded)
        {
            firstSceneLoaded = true;
            return;
        }

        StartCoroutine(ShowOverlay());
    }

    IEnumerator ShowOverlay()
    {
        loadingPanel.SetActive(true);

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(overlayTime);

        Time.timeScale = 1f;

        loadingPanel.SetActive(false);
    }
}
