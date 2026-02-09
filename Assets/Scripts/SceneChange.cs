using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public enum NewScene
    {
        ForestVillage, Overworld, SquirrelFight, FerretFight, TigerFight, DesertVillage
    }
    public NewScene myScene;

    public void OnTriggerEnter(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
        {
            if(myScene == NewScene.ForestVillage)
            {
                PlayerPrefs.SetInt("FromOverworld", 1);
                SceneManager.LoadScene("forestVillage");
            }
            else if(myScene == NewScene.Overworld)
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
            else if(myScene == NewScene.TigerFight)
            {
                //PlayerPrefs.SetInt("FromTiger", 1);
                SceneManager.LoadScene("TigerFight");
            }
            else if(myScene == NewScene.DesertVillage)
            {
                SceneManager.LoadScene("desertVillage");
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
