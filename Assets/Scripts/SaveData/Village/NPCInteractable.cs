using UnityEngine;
using TMPro;

public enum NPCInteractionType
{
    Dialogue,
    Shop
}

public class NPCInteractable : MonoBehaviour
{
    public NPCInteractionType interactionType = NPCInteractionType.Dialogue;

    public string promptText = "[E]";

    public GameObject promptCanvas;
    public TMP_Text promptTextUI;

    public TMP_Text nameTagText;

    public string npcDisplayName = "";
    public Sprite npcPortrait;
    [TextArea(2, 4)]
    public string dialogueLine = "";

    public string shopTitle = "";
    [TextArea(3, 8)]
    public string shopInventoryDescription =
        "";

    bool playerInRange = false;
    public bool IsPlayerInRange => playerInRange;

    void Start()
    {
        if (promptCanvas != null)
            promptCanvas.SetActive(false);

        if (nameTagText != null)
            nameTagText.text = npcDisplayName;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (promptCanvas != null)
        {
            promptCanvas.SetActive(true);
            if (promptTextUI != null)
                promptTextUI.text = promptText;
        }

        var pic = other.GetComponent<PlayerInteractionController>();
        if (pic != null)
            pic.SetCurrentTarget(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptCanvas != null)
            promptCanvas.SetActive(false);

        var pic = other.GetComponent<PlayerInteractionController>();
        if (pic != null && pic.GetCurrentTarget() == this)
            pic.ClearCurrentTarget(this);
    }
}
