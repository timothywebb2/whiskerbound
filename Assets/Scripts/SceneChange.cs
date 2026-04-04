using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public enum NewScene
    {
        ForestVillage, ForestOverworld, SquirrelFight, FerretFight, TigerFight, DesertVillage, DesertOverworld, BATTLE
    }
    public NewScene myScene;

    public void OnTriggerEnter(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
        {
            //SceneManager.LoadScene(myScene.toString);

            if(myScene == NewScene.ForestVillage)
            {
                PlayerPrefs.SetInt("FromOverworld", 1);
                SceneManager.LoadScene("forestVillage");
            }
            else if(myScene == NewScene.ForestOverworld)
            {
                // note: put some way to determine overworld position
                SceneManager.LoadScene("forestOverworld");
            }
            else if(myScene == NewScene.SquirrelFight)
            {
                //PlayerPrefs.SetInt("FromSquirrel", 1);
                SceneManager.LoadScene("SquirrelFight");
            }
            else if(myScene == NewScene.FerretFight)
            {
                //PlayerPrefs.SetInt("FromFerret", 1);
                SceneManager.LoadScene("FerretFight");
            }
            else if(myScene == NewScene.BATTLE)
            {
                //PlayerPrefs.SetInt("FromTiger", 1);
                SceneManager.LoadScene("BATTLE");
            }
            else if(myScene == NewScene.DesertVillage)
            {
                SceneManager.LoadScene("desertVillage");
            }
            else if(myScene == NewScene.DesertOverworld)
            {
                SceneManager.LoadScene("desertOverworld");
            }
        }
    }

    /*public bool isSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == sceneName)
                return true;
        }
        return false;
    }*/
}
