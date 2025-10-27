using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    public GameObject knightSkills;
    public KeyCode triggerKey = KeyCode.K;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (knightSkills != null)
            {
                knightSkills.SetActive(!knightSkills.activeSelf);
            }
            else
            {
                Debug.LogWarning("knightSkills");
            }
        }
    }
}

