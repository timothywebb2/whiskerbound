using UnityEngine;

public class inventoryMenu : MonoBehaviour 
{
    
    public GameObject container;


    void Update() 
    {
        bool isPause = false;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            container.SetActive(true);
            Time.timeScale = 0;
            isPause = true;
        }
        /*else if ((Input.GetKeyDown(KeyCode.Tab)) && isPause == true) 
        {
            container.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
        }*/
    }

    public void ExitButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void World1MapButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ProtoVillage");
    }
}
