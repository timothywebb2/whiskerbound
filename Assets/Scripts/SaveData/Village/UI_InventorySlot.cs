using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text quantityText;
    public Button button;
    public GameObject selectedMarker;

    string itemId = "";
    string itemDisplayName = "";
    Sprite itemIcon;
    int quantity = 0;

    UI_InventoryManager manager;

    public bool IsEmpty => string.IsNullOrEmpty(itemId);
    public bool HasItem(string id) => itemId == id;

    public string ItemId => itemId;
    public string DisplayName => itemDisplayName;
    public Sprite Icon => itemIcon;
    public int Quantity => quantity;

    void Awake()
    {
        if (button != null)
            button.onClick.AddListener(OnClick);

        //Clear();
    }

    public void SetManager(UI_InventoryManager inventoryManager)
    {
        manager = inventoryManager;
    }

    public void Clear()
    {
        itemId = "";
        itemDisplayName = "";
        itemIcon = null;
        quantity = 0;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (nameText != null) nameText.text = "";
        if (quantityText != null) quantityText.text = "";

        if (selectedMarker != null)
            selectedMarker.SetActive(false);
    }

    public void SetItem(string id, Sprite icon, string displayName, int amount)
    {
        itemId = id;
        itemDisplayName = displayName;
        itemIcon = icon;
        quantity = amount;

        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }

        if (nameText != null) nameText.text = displayName;
        if (quantityText != null) quantityText.text = "x" + quantity;
    }

    public void AddAmount(int amount)
    {
        quantity += amount;
        if (quantityText != null) quantityText.text = "x" + quantity;
    }

    public void SetSelected(bool value)
    {
        if (selectedMarker != null)
            selectedMarker.SetActive(value);
    }

    void OnClick()
    {
        if (manager != null && !IsEmpty)
            manager.SelectSlot(this);
    }
}