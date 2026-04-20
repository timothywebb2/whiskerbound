using UnityEngine;
using TMPro;
using System;

public class UI_ShopPanel : MonoBehaviour
{
    [Header("Shop Stock")]
    public ItemData[] possibleItems;
    public UI_ShopSlots[] shopSlots;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip purchaseClip;

    [Header("Shop Objects")]
    public TMP_Text shopTitleText;
    public TMP_Text itemListText;
    public TMP_Text coinText;
    public UI_InventoryManager inventoryManager;
    public TMP_Text descriptionText;

    public int coins = 100;

    Action onCloseCallback;
    bool isOpen = false;

    // -------------------- Show shop --------------------
    public void ShowShop(string shopTitle, string itemsDescription, Action onCloseCallback)
    {
        if (shopTitleText != null) shopTitleText.text = shopTitle;
        if (itemListText != null) itemListText.text = itemsDescription;

        this.onCloseCallback = onCloseCallback;

        GenerateShopStock();
        UpdateCoinText();

        gameObject.SetActive(true);
        isOpen = true;
        
    }

    public void ShowShop(string shopTitle, Action onCloseCallback)
    {
        if (shopTitleText != null)
            shopTitleText.text = shopTitle;

        this.onCloseCallback = onCloseCallback;

        GenerateShopStock();
        UpdateCoinText();

        gameObject.SetActive(true);
        isOpen = true;
        Debug.Log("Shop Menu is Open");
    }

    // -------------------- Shop functionality --------------------
    void GenerateShopStock()
    {
        foreach (var slot in shopSlots)
        {
            ItemData item = possibleItems[UnityEngine.Random.Range(0, possibleItems.Length)];

            int amount = UnityEngine.Random.Range(1, 4);

            int cost = item.baseCost * amount;

            slot.Setup(item, cost, amount, this);
        }
    }

    public void ShowDescription(string text)
    {
        if (descriptionText != null)
            descriptionText.text = text;
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        isOpen = false;

        onCloseCallback?.Invoke();
        onCloseCallback = null;
    }

    public bool PurchaseItem(UI_ShopSlots slot, int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            UpdateCoinText();

            int amountToAdd = Mathf.Max(1, slot.GetAmount());

            inventoryManager.AddItem(
                slot.GetItem().itemId,
                slot.GetItem().icon,
                slot.GetItem().displayName,
                amountToAdd
            );

            slot.SellItem();
            PlayPurchaseSound();

            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    public bool TryBuyItem(string itemId, Sprite icon, string displayName, int cost, int amount)
    {
        if (coins < cost)
        {
            Debug.Log("Not enough coins!");
            return false;
        }

        coins -= cost;
        UpdateCoinText();

        inventoryManager.AddItem(itemId, icon, displayName, amount);

        PlayPurchaseSound();

        return true;
    }

    void PlayPurchaseSound()
    {
        if (uiAudioSource != null && purchaseClip != null)
            uiAudioSource.PlayOneShot(purchaseClip);
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
        UISelectionHelper.SelectFirstButton(transform);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
            coinText.text = "" + coins;
    }

    public bool IsOpen() => isOpen;
}