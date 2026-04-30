using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string myScene;
    public int spawnPoint;

    public void OnTriggerEnter(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
        {
            SceneManager.LoadScene(myScene);
            if(myScene != "BATTLE")
                PlayerPrefs.SetString("LastScene", myScene);
            PlayerPrefs.SetInt("SpawnPoint", spawnPoint);

            // if loading into a village, save scene to be loaded when game is started
            if(myScene == "forestVillage" || myScene == "desertVillage" || myScene == "caveVillage" || myScene == "arcticVillage")
                PlayerPrefs.SetString("LastVillage", myScene);
        }

        // if loading from main menu, send to village
        // with spawnpoint 1
    }
}
