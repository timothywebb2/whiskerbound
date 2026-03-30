using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_InventoryManager : MonoBehaviour
{
    [Header("Consumable Slots")]
    public UI_InventorySlot adrenalineSlot;
    public UI_InventorySlot healingSlot;
    public UI_InventorySlot essenceSlot;
    public UI_InventorySlot extraSlot1;
    public UI_InventorySlot extraSlot2;

    [Header("Selected Item UI")]
    public Image selectedItemIcon;
    public TMP_Text selectedItemNameText;
    public TMP_Text selectedItemDescriptionText;
    public TMP_Dropdown targetDropdown;
    public Button useButton;

    UI_InventorySlot currentSelectedSlot;

    void Start()
    {
        if (adrenalineSlot != null) adrenalineSlot.SetManager(this);
        if (healingSlot != null) healingSlot.SetManager(this);
        if (essenceSlot != null) essenceSlot.SetManager(this);
        if (extraSlot1 != null) extraSlot1.SetManager(this);
        if (extraSlot2 != null) extraSlot2.SetManager(this);

        SetupDropdown();
        ClearSelectedUI();

        if (useButton != null)
            useButton.onClick.AddListener(OnUseClicked);
    }

    void SetupDropdown()
    {
        if (targetDropdown == null) return;

        targetDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "Knight",
            "Sorcerer",
            "Cleric"
        };

        targetDropdown.AddOptions(options);
        targetDropdown.value = 0;
        targetDropdown.RefreshShownValue();
    }

    public void AddItem(string id, Sprite icon, string displayName, int amount)
    {
        amount = Mathf.Max(1, amount);

        UI_InventorySlot targetSlot = GetPreferredSlot(id);

        if (targetSlot != null)
        {
            if (targetSlot.IsEmpty)
            {
                Debug.Log("Got " + amount + " of " + id);
                targetSlot.SetItem(id, icon, displayName, amount);
                return;
            }

            if (targetSlot.HasItem(id))
            {
                Debug.Log("Got " + amount + " of " + id);
                targetSlot.AddAmount(amount);
                return;
            }
        }

        UI_InventorySlot[] extraSlots = { extraSlot1, extraSlot2 };

        for (int i = 0; i < extraSlots.Length; i++)
        {
            var slot = extraSlots[i];
            if (slot != null && !slot.IsEmpty && slot.HasItem(id))
            {
                Debug.Log("Got " + amount + " of " + id);
                slot.AddAmount(amount);
                return;
            }
        }

        for (int i = 0; i < extraSlots.Length; i++)
        {
            var slot = extraSlots[i];
            if (slot != null && slot.IsEmpty)
            {
                Debug.Log("Got " + amount + " of " + id);
                slot.SetItem(id, icon, displayName, amount);
                return;
            }
        }

        Debug.Log("Consumables inventory full!");
    }
    UI_InventorySlot GetPreferredSlot(string id)
    {
        string lowerId = id.ToLower();

        if (lowerId.Contains("adrenaline"))
            return adrenalineSlot;

        if (lowerId.Contains("healing"))
            return healingSlot;

        if (lowerId.Contains("essence"))
            return essenceSlot;

        return null;
    }

    public void SelectSlot(UI_InventorySlot slot)
    {
        currentSelectedSlot = slot;

        UI_InventorySlot[] allSlots =
        {
            adrenalineSlot,
            healingSlot,
            essenceSlot,
            extraSlot1,
            extraSlot2
        };

        for (int i = 0; i < allSlots.Length; i++)
        {
            if (allSlots[i] != null)
                allSlots[i].SetSelected(allSlots[i] == slot);
        }

        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = slot.Icon;
            selectedItemIcon.enabled = slot.Icon != null;
        }

        if (selectedItemNameText != null)
            selectedItemNameText.text = slot.DisplayName;

        if (selectedItemDescriptionText != null)
            selectedItemDescriptionText.text = GetDescriptionFromId(slot.ItemId);
    }

    string GetDescriptionFromId(string id)
    {
        string lowerId = id.ToLower();

        if (lowerId.Contains("adrenaline"))
            return "Boosts combat readiness and improves offensive momentum.";

        if (lowerId.Contains("healing"))
            return "Restores health to the selected party member.";

        if (lowerId.Contains("essence"))
            return "A concentrated arcane material infused with magical energy.";

        return "A consumable item.";
    }

    void ClearSelectedUI()
    {
        if (selectedItemIcon != null)
        {
            selectedItemIcon.sprite = null;
            selectedItemIcon.enabled = false;
        }

        if (selectedItemNameText != null) selectedItemNameText.text = "";
        if (selectedItemDescriptionText != null) selectedItemDescriptionText.text = "";
    }

    void OnUseClicked()
    {
        if (currentSelectedSlot == null || currentSelectedSlot.IsEmpty)
            return;

        string targetName = "Knight";

        if (targetDropdown != null)
            targetName = targetDropdown.options[targetDropdown.value].text;

        Debug.Log("Use " + currentSelectedSlot.DisplayName + " on " + targetName);
    }
}