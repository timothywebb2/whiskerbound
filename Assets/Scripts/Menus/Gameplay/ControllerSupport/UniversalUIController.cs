using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UniversalUIController : MonoBehaviour
{
    static UniversalUIController instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyGlobalButtonColors();
    }

    void ApplyGlobalButtonColors()
    {
        Color highlight = new Color(236f / 255f, 164f / 255f, 60f / 255f);
        Color pressed = new Color(116f / 255f, 70f / 255f, 6f / 255f);

        var buttons = FindObjectsOfType<Button>(true);

        foreach (var button in buttons)
        {
            var colors = button.colors;
            colors.highlightedColor = highlight;
            colors.selectedColor = highlight;
            colors.pressedColor = pressed;
            button.colors = colors;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}