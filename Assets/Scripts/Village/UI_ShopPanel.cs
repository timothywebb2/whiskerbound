using UnityEngine;
using TMPro;
using System;

public class UI_ShopPanel : MonoBehaviour
{
    public TMP_Text shopTitleText;
    public TMP_Text itemListText;

    Action onCloseCallback;
    bool isOpen = false;

    public void ShowShop(string shopTitle, string itemsDescription, Action onCloseCallback)
    {
        if (shopTitleText != null) shopTitleText.text = shopTitle;
        if (itemListText != null) itemListText.text = itemsDescription;

        this.onCloseCallback = onCloseCallback;

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

    public bool IsOpen() => isOpen;
}
