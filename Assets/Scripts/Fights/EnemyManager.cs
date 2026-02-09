using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject squirrelEnemy;
    public GameObject ferretEnemy;
    public GameObject tigerEnemy;

    public void Start()
    {
        if(PlayerPrefs.GetInt("BeatSquirrel", 0) == 1)
            squirrelEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatFerret", 0) == 1)
            ferretEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatTiger", 0) == 1)
            tigerEnemy.SetActive(false);
    }
}
