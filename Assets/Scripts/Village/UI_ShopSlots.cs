using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopSlots : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button button;

    ItemData item;

    private ItemData currentItem;
    private int currentCost;
    private int currentAmount;
    private UI_ShopPanel shopPanel;

    bool sold = false;

    public void Setup(ItemData item, int cost, int amount, UI_ShopPanel shop)
    {
        currentItem = item;
        currentCost = cost;
        currentAmount = amount;
        shopPanel = shop;

        if (iconImage != null) iconImage.sprite = item.icon;
        if (nameText != null) nameText.text = $"{item.displayName} x{amount}";
        if (priceText != null) priceText.text = cost.ToString();

        if (button != null)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnBuy);
        }
        else
        {
            Debug.LogError("UI_ShopSlots: Button not assigned on " + gameObject.name);
        }
    }

    public void SellItem()
    {
        sold = true;

        if (button != null) button.interactable = false;
        if (iconImage != null) iconImage.color = new Color(1, 1, 1, 0.25f);
        if (nameText != null) nameText.text = "SOLD OUT";
        if (priceText != null) priceText.text = "";
    }

    public ItemData GetItem() => currentItem;
    public int GetAmount() => currentAmount;

    public void OnBuy()
    {
        if (sold) return;

        bool success = shopPanel.PurchaseItem(this, currentCost);

        if (success)
            SellItem();
    }
}