using UnityEngine;
using UnityEngine.InputSystem;

public class UI_SettingsInventoryManager : MonoBehaviour
{
    //opacity - cecil
    public GameObject opacity;

    [Header("Panels")]
    public GameObject panelSettings;
    public GameObject panelInventory;
    public GameObject topRightGroup;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip inventoryOpenClip;

    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction toggleSettingsAction;
    private InputAction toggleInventoryAction;

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleInventoryPanel();
        }
    }

    private void Awake()
    {
        toggleSettingsAction = inputActions.FindAction("UI/ToggleSettings");
        toggleInventoryAction = inputActions.FindAction("UI/ToggleInventory");

        if (toggleSettingsAction == null || toggleInventoryAction == null)
        {
            Debug.LogError("Could not find UI actions in InputActionAsset!");
            return;
        }

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

    public void ToggleSettingsPanel()
    {
        if (panelSettings == null) return;

        bool newState = !panelSettings.activeSelf;
        panelSettings.SetActive(newState);

        if (newState && panelInventory.activeSelf)
            CloseInventoryPanel();

        if (opacity != null)
            opacity.SetActive(newState);
    }

    public void OpenInventoryPanel()
    {
        panelInventory.SetActive(true);

        if (panelSettings.activeSelf)
            panelSettings.SetActive(false);

        if (opacity != null)
            opacity.SetActive(true);

        if (topRightGroup != null)
            topRightGroup.SetActive(false);

        if (uiAudioSource != null && inventoryOpenClip != null)
            uiAudioSource.PlayOneShot(inventoryOpenClip);
    }

    public void CloseInventoryPanel()
    {
        panelInventory.SetActive(false);

        if (opacity != null)
            opacity.SetActive(false);

        if (topRightGroup != null)
            topRightGroup.SetActive(true);
    }

    public void ToggleInventoryPanel()
    {
        if (panelInventory.activeSelf)
            CloseInventoryPanel();
        else
            OpenInventoryPanel();
    }
}