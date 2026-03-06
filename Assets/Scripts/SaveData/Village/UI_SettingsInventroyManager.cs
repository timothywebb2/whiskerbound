using UnityEngine;
using UnityEngine.InputSystem;

public class UI_SettingsInventoryManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelSettings;
    public GameObject panelInventory;

    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction toggleSettingsAction;
    private InputAction toggleInventoryAction;

    private void Awake()
    {
        toggleSettingsAction = inputActions.FindAction("UI/ToggleSettings");
        toggleInventoryAction = inputActions.FindAction("UI/ToggleInventory");

        if (toggleSettingsAction == null || toggleInventoryAction == null)
        {
            Debug.LogError("Could not find UI actions in InputActionAsset!");
            return;
        }

        // Subscribe to performed events
        toggleSettingsAction.performed += ctx => ToggleSettingsPanel();
        toggleInventoryAction.performed += ctx => ToggleInventoryPanel();
    }

    private void OnEnable()
    {
        toggleSettingsAction?.Enable();
        toggleInventoryAction?.Enable();
    }

    private void OnDisable()
    {
        toggleSettingsAction?.Disable();
        toggleInventoryAction?.Disable();
    }

    // ===== panels =====
    public void ToggleSettingsPanel()
    {
        bool newState = !panelSettings.activeSelf;
        panelSettings.SetActive(newState);

        if (newState && panelInventory.activeSelf)
            panelInventory.SetActive(false);
    }

    public void ToggleInventoryPanel()
    {
        bool newState = !panelInventory.activeSelf;
        panelInventory.SetActive(newState);

        if (newState && panelSettings.activeSelf)
            panelSettings.SetActive(false);
    }
}
