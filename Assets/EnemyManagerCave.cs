using UnityEngine;

public class EnemyManagerCave : MonoBehaviour
{
    public GameObject batEnemy;
    public GameObject lionEnemy;
    public GameObject bearEnemy;

    public void Start()
    {
        if(PlayerPrefs.GetInt("BeatBat", 0) == 1)
            batEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatLion", 0) == 1)
            lionEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatBear", 0) == 1)
            bearEnemy.SetActive(false);
    }
}
