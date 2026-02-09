using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    public UI_ShopPanel shopPanel;

    public string itemId;
    public string displayName;
    public Sprite icon;
    public int cost = 10;
    public int amount = 1;

    public void OnBuyClicked()
    {
        if (shopPanel != null)
        {
            shopPanel.TryBuyItem(itemId, icon, displayName, cost, amount);
        }
    }
}
