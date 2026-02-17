using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControllerSkillNavigation : MonoBehaviour
{
    public GameObject KnightSkills;
    public Button[] skillButtons;   //buttons in order

    public InputActionReference openMenuAction; //start button
    public InputActionReference navigateUpAction; //up
    public InputActionReference navigateDownAction; //down
    public InputActionReference confirmAction;  //a button
    public InputActionReference backAction; //b button

    public KnightMoveset knightMoveset; //game object with knightmoveset script for printing reference

    public int index = 0;


    void OnEnable()
    {
        openMenuAction.action.performed += OpenMenu;
        confirmAction.action.performed += ConfirmSelection;
        backAction.action.performed += CloseMenu;

        navigateUpAction.action.performed += _ => MoveUp();
        navigateDownAction.action.performed += _ => MoveDown();
    }

    void OnDisable()
    {
        openMenuAction.action.performed -= OpenMenu;
        confirmAction.action.performed -= ConfirmSelection;
        backAction.action.performed -= CloseMenu;

        navigateUpAction.action.performed += _ => MoveUp();
        navigateDownAction.action.performed += _ => MoveDown();
    }

    public void OpenMenu(InputAction.CallbackContext ctx)
    {
        if (KnightSkills.activeSelf || (knightMoveset != null && knightMoveset.printing))
            return;

        KnightSkills.SetActive(true);
        index = 0;
        SelectButton(index);
    }

    public void CloseMenu(InputAction.CallbackContext ctx)
    {
        if (!KnightSkills.activeSelf)
            return;

        KnightSkills.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MoveUp()
    {
        index--;
        if (index < 0) index = skillButtons.Length - 1;
        SelectButton(index);
    }

    public void MoveDown()
    {
        index++;
        if (index >= skillButtons.Length) index = 0;
        SelectButton(index);
    }


    public void ConfirmSelection(InputAction.CallbackContext ctx)
    {
        if (!KnightSkills.activeSelf)
            return;

        skillButtons[index].onClick.Invoke();
        KnightSkills.SetActive(false);
    }

    public void SelectButton(int i)
    {
        if (skillButtons == null || skillButtons.Length == 0)
            return;

        for (int j = 0; j < skillButtons.Length; j++)
        {
            Button btn = skillButtons[j];
            if (btn == null) continue;

            Image img = btn.GetComponent<Image>();

            if (j == i)
            {
                //highlight selected button
                if (img != null) img.color = Color.yellow; //bright highlight
                btn.transform.localScale = Vector3.one * 1.2f; //slightly larger
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
            }
            else
            {
                // Reset other buttons
                if (img != null) img.color = Color.white;
                btn.transform.localScale = Vector3.one; //normal size
            }
        }
    }

}