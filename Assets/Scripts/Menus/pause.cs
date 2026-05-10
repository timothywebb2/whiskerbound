using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//CECIL.CREATES - Cecil

public class pause : MonoBehaviour
{
    [Header("Pause")]
    public GameObject pauseObject; 
    public GameObject pauseMenu;
    public bool isPause = false;

    [Header("Other Menus")]
    public GameObject optionsMenu;
    public GameObject controlsScreen;
    public GameObject confirmationMenu;
    public GameObject confirmationRestart;

    [Header("Audio")]
    public AudioSource villageMusic;
    public AudioSource uiAudioSource;
    public AudioClip pauseOpenClip;


    void Start()
    {
        //makes sure sub pause menus don't all activate once pause turns on
        optionsMenu.SetActive(false);
        confirmationMenu.SetActive(false);
        controlsScreen.SetActive(false);
        //Debug.Log("Menus are inactive");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("I pressed Escape");
            if (isPause == false)
            {
                pauseGame();
            }
            else
            {
                resumeGame();
            }
        }
    }

    public void restartScene()
    {
        //var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(0);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        resumeGame();

    }

    public void pauseGame()
    {
        //pauses game activity and turns menu on
        Debug.Log("pauseGame is running");
        pauseObject.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;

        if (villageMusic != null)
            villageMusic.Pause();

        if (uiAudioSource != null && pauseOpenClip != null)
            uiAudioSource.PlayOneShot(pauseOpenClip);
    }

    public void resumeGame()
    {
        //resumes game activity, turning menu off
        pauseObject.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;

        if (villageMusic != null)
            villageMusic.UnPause();
    }

    public void options()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void back()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        confirmationMenu.SetActive(false);
        controlsScreen.SetActive(false);
    }

    public void controls()
    {
        controlsScreen.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void askPlayer()
    {
        confirmationMenu.SetActive(true);
    }

    public void goMainMenu()
    {
        isPause = false;
        Time.timeScale = 1f; //stops going back to menu breaking the game

        if (villageMusic != null)
            villageMusic.UnPause();

        SceneManager.LoadScene(0);
    }
}