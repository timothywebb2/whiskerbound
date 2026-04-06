using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrestManager : MonoBehaviour
{
    public Image crestImage;
    public Sprite forestCrest;
    public Sprite desertCrest;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateCrest();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        UpdateCrest();
    }

    void UpdateCrest()
    {
        string sceneName = SceneManager.GetActiveScene().name.ToLower();

        if (sceneName.Contains("desert"))
        {
            crestImage.sprite = desertCrest;
        }
        else
        {
            crestImage.sprite = forestCrest;
        }

        crestImage.enabled = true;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
