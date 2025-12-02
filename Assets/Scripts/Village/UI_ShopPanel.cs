using UnityEngine;
using TMPro;
using System;

public class UI_ShopPanel : MonoBehaviour
{
    public TMP_Text shopTitleText;
    public TMP_Text coinText;
    public UI_InventoryManager inventoryManager;

    public int coins = 100;

    Action onCloseCallback;
    bool isOpen = false;

    public void ShowShop(string shopTitle, Action onCloseCallback)
    {
        if (shopTitleText != null) shopTitleText.text = shopTitle;

        this.onCloseCallback = onCloseCallback;

        UpdateCoinText();
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
        isOpen = false;

        onCloseCallback?.Invoke();
        onCloseCallback = null;
    }

    public void PurchaseItem(int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            UpdateCoinText();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void TryBuyItem(string itemId, Sprite icon, string displayName, int cost, int amount)
    {
        if (coins < cost)
        {
            Debug.Log("Not enough coins!");
            return;
        }

        coins -= cost;
        UpdateCoinText();

        if (inventoryManager != null)
        {
            inventoryManager.AddItem(itemId, icon, displayName, amount);
        }
        else
        {
            Debug.LogWarning("UI_ShopPanel: inventoryManager is not assigned.");
        }
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
