using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public UI_DialoguePanel dialoguePanel;
    public UI_ShopPanel shopPanel;

   
    public MonoBehaviour movementScript;

    private NPCInteractable currentTarget;
    private bool isBusy = false;

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
        if (isBusy)
        {
            if (dialoguePanel != null && dialoguePanel.IsOpen() && Input.GetKeyDown(KeyCode.E))
            {
                dialoguePanel.CloseDialogue();
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
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
