using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text quantityText;

    string itemId = "";
    int quantity = 0;

    public bool IsEmpty => string.IsNullOrEmpty(itemId);

    public bool HasItem(string id)
    {
        return itemId == id;
    }

    public void SetItem(string id, Sprite icon, string displayName, int amount)
    {
        itemId = id;
        quantity = amount;

        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }

        if (nameText != null) nameText.text = displayName;
        if (quantityText != null) quantityText.text = quantity.ToString();
    }

    public void AddAmount(int amount)
    {
        quantity += amount;
        if (quantityText != null)
            quantityText.text = quantity.ToString();
    }


    public void OnUseButton()
    {
        UseItem(GameObject.FindWithTag("Player"));
    }

    void UseItem(GameObject player)
    {
        if (IsEmpty) return;

        var item = ItemDatabase.Instance.GetItem(itemId);
        if (item == null)
        {
            Debug.Log("Item not found: " + itemId);
            return;
        }

        item.Use(player);
    }

    void ConsumeOne()
    {
        quantity--;

        if (quantity <= 0)
            ClearSlot();
        else
            quantityText.text = quantity.ToString();
    }

    void ClearSlot()
    {
        itemId = "";
        quantity = 0;

        iconImage.enabled = false;
        nameText.text = "";
        quantityText.text = "";
    }
}