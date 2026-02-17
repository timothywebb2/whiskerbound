using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    public UI_DialoguePanel dialoguePanel;
    public UI_ShopPanel shopPanel;

   
    public MonoBehaviour movementScript;

    private NPCInteractable currentTarget;
    private bool isBusy = false;

    public InputActionReference interactActionRef;
    private InputAction interactAction;


    private void OnEnable()
    {
        if (interactActionRef != null)
        {
            interactAction = interactActionRef.action;
            interactAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.Disable();
    }

    public void SetCurrentTarget(NPCInteractable npc)
    {
        if (isBusy) return;
        currentTarget = npc;
    }

    public void ClearCurrentTarget(NPCInteractable npc)
    {
        if (currentTarget == npc)
            currentTarget = null;
    }

    public NPCInteractable GetCurrentTarget()
    {
        return currentTarget;
    }


    private void Update()
    {
        bool interactPressed = interactAction.WasPressedThisFrame();

        if (isBusy)
        {
            if (dialoguePanel != null &&
                dialoguePanel.IsOpen() &&
                interactPressed)
            {
                dialoguePanel.CloseDialogue();
            }
            return;
        }

        if (interactPressed && currentTarget != null)
        {
            switch (currentTarget.interactionType)
            {
                case NPCInteractionType.Dialogue:
                    OpenDialogueFromNPC(currentTarget);
                    break;

                case NPCInteractionType.Shop:
                    OpenShopFromNPC(currentTarget);
                    break;
            }
        }
    }


    void OpenDialogueFromNPC(NPCInteractable npc)
    {
        if (dialoguePanel == null) return;

        isBusy = true;
        if (movementScript != null) movementScript.enabled = false;

        dialoguePanel.ShowDialogue(
            npc.npcPortrait,
            npc.npcDisplayName,
            npc.dialogueLine,
            OnDialogueClosed
        );
    }

    void OpenShopFromNPC(NPCInteractable npc)
    {
        if (shopPanel == null) return;

        isBusy = true;
        if (movementScript != null) movementScript.enabled = false;

        shopPanel.ShowShop(
            npc.shopTitle,
            OnShopClosed
        );
    }

    void OnDialogueClosed()
    {
        isBusy = false;
        if (movementScript != null) movementScript.enabled = true;
    }

    void OnShopClosed()
    {
        isBusy = false;
        if (movementScript != null) movementScript.enabled = true;
    }
}
