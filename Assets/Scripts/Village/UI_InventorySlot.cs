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
        if (quantityText != null) quantityText.text = "" + quantity;
    }

    public void AddAmount(int amount)
    {
        quantity += amount;
        if (quantityText != null) quantityText.text = "" + quantity;
    }
}
