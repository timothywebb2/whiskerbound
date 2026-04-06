using UnityEngine;
using UnityEngine.UI;

public class inventoryMenu : MonoBehaviour 
{
    
    public GameObject container;
    public bool isPause = false;
    //public Image world1Map;
    //public Image world2Map;


    public Image worldMapHolder;
    public Sprite worldMap1;
    public Sprite worldMap2;


    //public Image world3Map;
    //public Image world4Map;
    //public Image world5Map;
    //public int whichMapEnabled = 0;

    void Start()
    {
        //world1Map.enabled = false;
        //whichMapEnabled = 1;
        //world3Map.enabled = false;
        //world4Map.enabled = false;
        //world5Map.enabled = false;

        container.SetActive(false);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPause)
        {
            container.SetActive(true);
            Time.timeScale = 0;
            isPause = true;
            //world1Map.enabled = true;
            //whichMapEnabled = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Tab)) && isPause == true) 
        {
            container.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
        }
    }

    /*public void ExitButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
    }*/

    public void World1MapButton(int whichMapEnabled)
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("ProtoVillage");
        switch (whichMapEnabled)
        {
            case 1:
                worldMapHolder.GetComponent<Image>().sprite = worldMap1;
                break;
            case 2:
                worldMapHolder.GetComponent<Image>().sprite = worldMap2;
                //world2Map.enabled = false;
                //world1Map.enabled = true;
                //whichMapEnabled = 1;
                break;
            /*case 3:
                world3Map.enabled = false;
                world1Map.enabled = true;
                whichMapEnabled = 1;
            case 4:
                world4Map.enabled = false;
                world1Map.enabled = true;
                whichMapEnabled = 1;
            case 5:
                world5Map.enabled = false;
                world1Map.enabled = true;
                whichMapEnabled = 1;*/
            default:
                worldMapHolder.GetComponent<Image>().sprite = worldMap1;
                //world1Map.enabled = true;
                //world2Map.enabled = false;
                //world3Map.enabled = false;
                //world4Map.enabled = false;
                //world5Map.enabled = false;
                //whichMapEnabled = 1;
                break;
        }
    }
}
