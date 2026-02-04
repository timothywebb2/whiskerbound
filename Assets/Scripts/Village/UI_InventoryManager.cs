using UnityEngine;

public class UI_InventoryManager : MonoBehaviour
{
    public UI_InventorySlot[] consumableSlots;

    public void AddItem(string id, Sprite icon, string displayName, int amount)
    {
        for (int i = 0; i < consumableSlots.Length; i++)
        {
            var slot = consumableSlots[i];
            if (slot != null && !slot.IsEmpty && slot.HasItem(id))
            {
                slot.AddAmount(amount);
                return;
            }
        }

        for (int i = 0; i < consumableSlots.Length; i++)
        {
            var slot = consumableSlots[i];
            if (slot != null && slot.IsEmpty)
            {
                slot.SetItem(id, icon, displayName, amount);
                return;
            }
        }

        Debug.Log("Consumables inventory full!");
    }
}
