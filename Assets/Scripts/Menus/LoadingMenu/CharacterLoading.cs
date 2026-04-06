using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CharacterLoading : MonoBehaviour
{
    public Sprite[] knightFrames;
    public Sprite[] sorcererFrames;
    public Sprite[] clericFrames;
    public Image characterImage;
    public float frameDuration = 0.25f;

    private Coroutine animCoroutine;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCharacter(scene.name.ToLower());
    }

    void UpdateCharacter(string sceneName)
    {
        if (animCoroutine != null)
            StopCoroutine(animCoroutine);

        Sprite[] frames = null;

        if (sceneName.Contains("forest"))
            frames = knightFrames;
        else if (sceneName.Contains("desert"))
            frames = sorcererFrames;
        else if (sceneName.Contains("cave"))
            frames = clericFrames;
        else
            frames = knightFrames;

        if (frames != null && frames.Length > 0)
            animCoroutine = StartCoroutine(PlayFrames(frames));
    }

    IEnumerator PlayFrames(Sprite[] frames)
    {
        int index = 0;
        while (true)
        {
            characterImage.sprite = frames[index];
            index = (index + 1) % frames.Length;
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}