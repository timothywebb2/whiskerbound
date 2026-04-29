using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DialoguePanel : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text npcNameText;
    public TMP_Text bodyText;

    int lineIndex;
    int lineMax;
    string[] lineCopy;

    bool isRecruitCopy;
    GameObject npcCopy;

    Action onCloseCallback;
    bool isOpen = false;

    public GameObject sorcererFollower;
    public GameObject clericFollower;

    public void ShowDialogue(Sprite portrait, string npcName, string[] line, bool isRecruit, GameObject npc, Action onCloseCallback)
    {
        if (portraitImage != null) portraitImage.sprite = portrait;
        if (npcNameText != null) npcNameText.text = npcName;
        
        isRecruitCopy = isRecruit;
        npcCopy = npc;

        if (bodyText != null) bodyText.text = line[0];
        lineCopy = line;
        lineIndex++;
        lineMax = line.Length - 1;

        this.onCloseCallback = onCloseCallback;

        gameObject.SetActive(true);
        isOpen = true;
        Debug.Log("Character Dialogue is open");
    }

    public void CloseDialogue()
    {
        if(lineIndex > lineMax)
        {
            lineIndex = 0;

            if(isRecruitCopy)
            {
                npcCopy.gameObject.SetActive(false);
                int newPartySize = PlayerPrefs.GetInt("PartySize", 1) + 1;
                PlayerPrefs.SetInt("PartySize", newPartySize);

                if (newPartySize == 2)
                    sorcererFollower.SetActive(true);
                else if (newPartySize == 3)
                    clericFollower.SetActive(true);
            }

            gameObject.SetActive(false);
            isOpen = false;

            onCloseCallback?.Invoke();
            onCloseCallback = null;
        }

        else
        {
            if (bodyText != null) bodyText.text = lineCopy[lineIndex];
            lineIndex++;
        }
    }

    public bool IsOpen() => isOpen;
}
