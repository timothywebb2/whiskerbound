using UnityEngine;

public class EnemyManagerDesert : MonoBehaviour
{
    public GameObject tazEnemy;
    public GameObject meerkatEnemy;
    public GameObject kangarooEnemy;

    public void Start()
    {
        if(PlayerPrefs.GetInt("BeatTaz", 0) == 1)
            tazEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatMeerkat", 0) == 1)
            meerkatEnemy.SetActive(false);
        if(PlayerPrefs.GetInt("BeatKangaroo", 0) == 1)
            kangarooEnemy.SetActive(false);
    }
}
