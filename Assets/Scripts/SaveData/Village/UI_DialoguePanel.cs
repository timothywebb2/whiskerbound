using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;
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

    public Sprite[] sorcererSprites;
    public Sprite[] clericSprites;
    private int Xdir;
    private int Ydir;

    public void ShowDialogue(Sprite portrait, string npcName, string[] line, bool isRecruit, GameObject npc, Action onCloseCallback)
    {
        if (portraitImage != null) portraitImage.sprite = portrait;
        if (npcNameText != null) npcNameText.text = npcName;
        
        isRecruitCopy = isRecruit;
        npcCopy = npc;

        // make npc turn to player
        if(isRecruit)
        {
            // get player animation state
            string playerClip = player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name;
Debug.Log("playerClip is " + playerClip);

            int spriteIndex = 0;
            if(playerClip == "WalkingFront" || playerClip == "FrontIdle") //player is facing towards camera, npc should be facing away from camera
                spriteIndex = 0;
            else if(playerClip == "WalkingBack" || playerClip == "BackIdle") //player is facing away from camera, npc should be facing towards camera
                spriteIndex = 1;
            else if(playerClip == "WalkingLeft" || playerClip == "LeftIdle") //player is facing left, npc should be facing right
                spriteIndex = 2;
            else if(playerClip == "WalkingRight" || playerClip == "RightIdle") //player is facing right, npc should be facing left
                spriteIndex = 3;

            if(PlayerPrefs.GetInt("PartySize", 1) == 1) // talking to sorcerer
                npc.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = sorcererSprites[spriteIndex];
            else if(PlayerPrefs.GetInt("PartySize", 1) == 2) // talking to cleric
                npc.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = clericSprites[spriteIndex];
        }

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
