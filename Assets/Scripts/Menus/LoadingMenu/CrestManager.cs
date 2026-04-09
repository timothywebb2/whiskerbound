using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrestManager : MonoBehaviour
{
    public Image crestImage;
    public Sprite forestCrest;
    public Sprite desertCrest;
    public Sprite caveCrest;
    public Sprite arcticCrest;
   

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
        if (sceneName.Contains("arctic"))
        {
            crestImage.sprite = arcticCrest;
        }
        if (sceneName.Contains("cave"))
        {
            crestImage.sprite = caveCrest;
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
