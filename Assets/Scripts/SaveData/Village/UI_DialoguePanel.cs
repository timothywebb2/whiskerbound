using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_DialoguePanel : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text npcNameText;
    public TMP_Text bodyText;
    public GameObject promptCanvas;

    int lineIndex;
    int lineMax;
    string[] lineCopy;

    bool isRecruitCopy;
    GameObject npcCopy;

    Action onCloseCallback;
    bool isOpen = false;

    public GameObject sorcererFollower;
    public GameObject clericFollower;
    public GameObject player;

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
                
                promptCanvas.SetActive(false);

                if (newPartySize == 2)
                {
                    sorcererFollower.transform.position = npcCopy.transform.position;
                    sorcererFollower.SetActive(true);
                    sorcererFollower.GetComponent<NPCFollow>().followCharacterPositions.Clear();
                    sorcererFollower.GetComponent<NPCFollow>().followCharacterPositions.Add(player.transform.position);
                }
                else if (newPartySize == 3)
                {
                    clericFollower.transform.position = npcCopy.transform.position;
                    clericFollower.SetActive(true);
                    clericFollower.GetComponent<NPCFollow>().followCharacterPositions.Clear();
                    clericFollower.GetComponent<NPCFollow>().followCharacterPositions.Add(player.transform.position);
                }
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
