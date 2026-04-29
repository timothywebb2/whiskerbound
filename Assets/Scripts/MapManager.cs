using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Maps")]
    public GameObject DesertMap;
    public GameObject CaveMap;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  
    public void forestMap() {
        DesertMap.SetActive(false);
        CaveMap.SetActive(false);
    }

    public void desertMap()
    {
        DesertMap.SetActive(true);
        CaveMap.SetActive(false);
    }

}
