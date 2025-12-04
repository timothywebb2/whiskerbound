using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public enum NewScene
    {
        Village, Overworld, SquirrelFight, FerretFight, TigerFight
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
            else if(myScene == NewScene.SquirrelFight)
            {
                PlayerPrefs.SetInt("FromSquirrel", 1);
                SceneManager.LoadScene("SquirrelFight");
            }
            else if(myScene == NewScene.FerretFight)
            {
                PlayerPrefs.SetInt("FromFerret", 1);
                SceneManager.LoadScene("FerretFight");
            }
            else if(myScene == NewScene.TigerFight)
            {
                PlayerPrefs.SetInt("FromTiger", 1);
                SceneManager.LoadScene("TigerFight");
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
