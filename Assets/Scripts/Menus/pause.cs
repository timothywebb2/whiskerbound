using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//CECIL.CREATES - Cecil

public class pause : MonoBehaviour
{

    public GameObject pMenu; //full pause object
    public GameObject pScreen; //initial pause screen
    public GameObject oMenu; //options
    public GameObject cMenu; //confirmation
    public GameObject cScreen; //controls
    public GameObject KeyScreen;
    public GameObject XboxScreen;


    public bool isPause = false;

    void Start()
    {
          //makes sure sub pause menus don't all activate once pause turns on
          oMenu.SetActive(false);
          cMenu.SetActive(false);
          cScreen.SetActive(false);
          Debug.Log("Menus are inactive");
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
                } else
                    {
               resumeGame();
                }    
            }
    }

    public void restartScene() { 
    var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        resumeGame();

    }


   public void pauseGame()
    {
        //pauses game activity and turns menu on
        Debug.Log("pauseGame is running");
        pMenu.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }

    public void resumeGame()
    {
//resumes game activity, turning menu off
        pMenu.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;

    }

public void options(){
    oMenu.SetActive(true);
    pScreen.SetActive(false);

}

public void back(){
pScreen.SetActive(true);
oMenu.SetActive(false);
cMenu.SetActive(false);
cScreen.SetActive(false);
}

public void controls(){
    cScreen.SetActive(true);
    oMenu.SetActive(false);
}

public void askPlayer(){
    cMenu.SetActive(true);
}

public void goMainMenu(){
    isPause = false;
    Time.timeScale = 1f; //stops going back to menu breaking the game
    SceneManager.LoadScene(0);

}

public void xbox(){
    XboxScreen.SetActive(true);
    KeyScreen.SetActive(false);
}

public void key(){
    KeyScreen.SetActive(true);
    XboxScreen.SetActive(false);
}


}
