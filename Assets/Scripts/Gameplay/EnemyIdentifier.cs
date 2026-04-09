using UnityEngine;

public class EnemyIdentifier : MonoBehaviour
{
    public int enemyInt;

    public void OnTriggerEnter(Collider WhatIHit)
    {
        if(WhatIHit.tag == "Player")
            PlayerPrefs.SetInt("Enemy", enemyInt);
    }
}
