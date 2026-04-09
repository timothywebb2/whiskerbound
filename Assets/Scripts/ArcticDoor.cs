using UnityEngine;

public class ArcticDoor : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip audioClip;
    void Start()
    {
        if(PlayerPrefs.GetInt("ArcticKeyUsed", 0) == 1)
            this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider whatIHit)
    {
        if(whatIHit.tag == "Player")
        {
            Debug.Log("touched door");
            if(PlayerPrefs.GetInt("BeatKangaroo", 0) == 1)
            {
                audioManager.PlaySFX(audioClip);
                PlayerPrefs.SetInt("ArcticKeyUsed", 1);
                this.gameObject.SetActive(false);
            }
        }
    }
}
