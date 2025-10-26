using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DialoguePanel : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text npcNameText;
    public TMP_Text bodyText;

    Action onCloseCallback;
    bool isOpen = false;

    public void ShowDialogue(Sprite portrait, string npcName, string line, Action onCloseCallback)
    {
        if (portraitImage != null) portraitImage.sprite = portrait;
        if (npcNameText != null) npcNameText.text = npcName;
        if (bodyText != null) bodyText.text = line;

        this.onCloseCallback = onCloseCallback;

        gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseDialogue()
    {
        gameObject.SetActive(false);
        isOpen = false;

        onCloseCallback?.Invoke();
        onCloseCallback = null;
    }

    public bool IsOpen() => isOpen;
}
