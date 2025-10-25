using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class pause : MonoBehaviour
{

    public GameObject pMenu;
    public GameObject oMenu;
    public bool isPause;

    void Start()
    {
          pMenu.SetActive(false);
          oMenu.SetActive(false);
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
}

public void back(){
oMenu.SetActive(false);

}
}
