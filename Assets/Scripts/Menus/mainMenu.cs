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
        
        PlayerPrefs.SetInt("SpawnPoint", 0); // put player at start of village

        // heal all party members
        //PlayerPrefs.SetInt("KnightHealth", 100);
        //PlayerPrefs.SetInt("SorcererHealth", 60);
        //PlayerPrefs.SetInt("ClericHealth", 80);
        
        SceneManager.LoadScene(PlayerPrefs.GetString("LastVillage", "forestVillage")); // load last village
    }

    public void options() // show only options screen; prevents overlap
    {
        optionsMenu.SetActive(true);  
    }

    public void credits() //ditto
    {
        creditsMenu.SetActive(true);
    }

    public void controls() //ditto but close options screen
    {
        controlsMenu.SetActive(true);
        optionsMenu.SetActive(false);  
    }

    public void backButton() // no matter which back button, sends back to main screen.
    {
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }
}