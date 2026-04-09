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

    void OnCollisionEnter(Collision whatIHit)
    {
        if(whatIHit.collider.tag == "Player" && PlayerPrefs.GetInt("ArcticKey", 0) == 1)
        {
            audioManager.PlaySFX(audioClip);
            PlayerPrefs.SetInt("ArcticKeyUsed", 1);
            this.gameObject.SetActive(false);
        }
    }
}
