using UnityEngine;

public class UI_SettingsInventoryManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelSettings;    // assign Panel_Settings
    public GameObject panelInventory;   // assign Panel_Inventory

    // ===== Settings panel =====
    public void OpenSettingsPanel()
    {
        if (panelSettings != null) panelSettings.SetActive(true);
        if (panelInventory != null) panelInventory.SetActive(false);
    }

    public void CloseSettingsPanel()
    {
        if (panelSettings != null) panelSettings.SetActive(false);
    }

    // ===== Inventory panel =====
    public void OpenInventoryPanel()
    {
        if (panelInventory != null) panelInventory.SetActive(true);
        if (panelSettings != null) panelSettings.SetActive(false);
    }

    public void CloseInventoryPanel()
    {
        if (panelInventory != null) panelInventory.SetActive(false);
    }
}
