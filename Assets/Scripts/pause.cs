using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class pause : MonoBehaviour
{

    public GameObject pMenu; //pause
    public GameObject oMenu; //options
    public GameObject cMenu; //confirmation
    public bool isPause;

    void Start()
    {
          pMenu.SetActive(false);
          oMenu.SetActive(false);
          cMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
         if (Keyboard.current.escapeKey.isPressed){
            pauseGame();
        }
    }

   public void pauseGame()
    {
        //pauses game activity and turns menu on
        Time.timeScale = 0f;
        pMenu.SetActive(true);
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
    pMenu.SetActive(false);

}

public void back(){
pMenu.SetActive(true);
oMenu.SetActive(false);
cMenu.SetActive(false);


}

public void askPlayer(){
    cMenu.SetActive(true);
}

public void goMainMenu(){
SceneManager.LoadScene(0);

}

}
