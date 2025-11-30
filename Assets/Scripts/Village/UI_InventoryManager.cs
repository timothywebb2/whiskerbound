using UnityEngine;

public class UI_InventoryManager : MonoBehaviour
{
    public UI_InventorySlot[] consumableSlots;

    public void AddItem(string id, Sprite icon, string displayName, int amount)
    {
        foreach (var slot in consumableSlots)
        {
            if (slot != null && slot.HasItem(id))
            {
                slot.AddAmount(amount);
                return;
            }
        }

        foreach (var slot in consumableSlots)
        {
            if (slot != null && slot.IsEmpty)
            {
                slot.SetItem(id, icon, displayName, amount);
                return;
            }
        }

        Debug.Log("No free consumable slot for item: " + displayName);
    }
}
