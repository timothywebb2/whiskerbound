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

            PlayerPrefs.SetInt("SpawnPoint", spawnPoint);
        }
    }
}
