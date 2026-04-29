using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount;
    public UI_InventoryManager inventoryManager;

    public AudioManager audioManager;
    public AudioClip audioClip;

    public void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = item.icon;

        if(PlayerPrefs.GetInt("GotAdrenaline", 0) == 1 && item.itemId == "adrenaline")
            gameObject.SetActive(false);
        if(PlayerPrefs.GetInt("GotPotion", 0 ) == 1 && item.itemId == "health_potion")
            gameObject.SetActive(false);

    }
    void OnTriggerEnter(Collider whatIHit)
    {
        if(whatIHit.tag == "Player")
        {
            audioManager.PlaySFX(audioClip);
            inventoryManager.AddItem(item.itemId, item.icon, item.displayName, amount);

            if(item.itemId == "health_potion")
                PlayerPrefs.SetInt("GotPotion", 1);
            if(item.itemId == "adrenaline")
                PlayerPrefs.SetInt("GotAdrenaline", 1);

            Destroy(this.gameObject);
        }
    }
}
