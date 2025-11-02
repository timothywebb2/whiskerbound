using UnityEngine;
using UnityEngine.SceneManagement;

//CECIL SCRIPT
public class mainMenu : MonoBehaviour
{
    public GameObject mainMenuObject; //initializing objects in code for inspector attachment
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;

    void Start(){ // only main menu is active on start not the other canvases.
    optionsMenu.SetActive(false);
    creditsMenu.SetActive(false);
    controlsMenu.SetActive(false);
    }

    public void playGame(){
        SceneManager.LoadScene(1);
    }
//prevents overlap
    public void options(){// show only options screen
         optionsMenu.SetActive(true);
          mainMenuObject.SetActive(false);
    }

    public void credits(){//ditto
    creditsMenu.SetActive(true);
          mainMenuObject.SetActive(false);

    }

public void controls(){//ditto but close options screen
     controlsMenu.SetActive(true);
     optionsMenu.SetActive(false);
        mainMenuObject.SetActive(false);
}

    public void backButton(){ // no matter which back button, sends back to main screen.
  optionsMenu.SetActive(false);
    creditsMenu.SetActive(false);
    controlsMenu.SetActive(false);
    mainMenuObject.SetActive(true);

    }

    public void quit(){
        Application.Quit();
    }
}
