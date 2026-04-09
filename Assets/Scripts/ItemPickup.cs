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
    }
    void OnTriggerEnter(Collider whatIHit)
    {
        if(whatIHit.tag == "Player")
        {
            audioManager.PlaySFX(audioClip);
            inventoryManager.AddItem(item.itemId, item.icon, item.displayName, amount);
            Destroy(this.gameObject);
        }
    }
}
