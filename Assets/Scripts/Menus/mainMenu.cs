using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//CECIL.CREATES - rumbletaozi - Cecil 
public class mainMenu : MonoBehaviour
{
    public GameObject mainMenuObject; //initializing objects in code for inspector attachment
    public GameObject optionsMenu;
    public GameObject creditsMenu;
    public GameObject controlsMenu;

    public GameObject blackScreen;

    void Start(){ // only main menu is active on start not the other canvases.
    optionsMenu.SetActive(false);
    creditsMenu.SetActive(false);
    controlsMenu.SetActive(false);
    }

    public void playGame(){
        StartCoroutine(delayLoad());
    }

    public void showcase() {
        SceneManager.LoadScene("#SHOWCASE");
        PlayerPrefs.SetInt("SpawnPoint", 0);
    }

    IEnumerator delayLoad()
    {
        yield return new WaitForSeconds(1.8f);
        blackScreen.SetActive(true);
        
        PlayerPrefs.SetInt("SpawnPoint", 0);
        SceneManager.LoadScene(PlayerPrefs.GetString("LastVillage", "forestVillage"));
    }

    //prevents overlap
    public void options(){// show only options screen
         optionsMenu.SetActive(true);
          //mainMenuObject.SetActive(false);
    }

    public void credits(){//ditto
    creditsMenu.SetActive(true);
          //mainMenuObject.SetActive(false);

    }

public void controls(){//ditto but close options screen
     controlsMenu.SetActive(true);
     optionsMenu.SetActive(false);
        //mainMenuObject.SetActive(false);
}

    public void backButton(){ // no matter which back button, sends back to main screen.
  optionsMenu.SetActive(false);
    creditsMenu.SetActive(false);
    controlsMenu.SetActive(false);
    //mainMenuObject.SetActive(true);
    }

    public void quit(){
        Application.Quit();
    }
}
