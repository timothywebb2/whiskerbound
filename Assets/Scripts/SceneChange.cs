using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public enum NewScene
    {
        Village, Overworld
    }
    public NewScene myScene;

    public void OnTriggerEnter(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
        {
            if(myScene == NewScene.Village)
            {
                PlayerPrefs.SetInt("FromOverworld", 1);
                SceneManager.LoadScene("ProtoVillage");
            }
            else if(myScene == NewScene.Overworld)
            {
                // note: put some way to determine overworld position
                SceneManager.LoadScene("Overworld");
            }
        }
    }

    public bool isSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return true;
        }
        return false;
    }
}
