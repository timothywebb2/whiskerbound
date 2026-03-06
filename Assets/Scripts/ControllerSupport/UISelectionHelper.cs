using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UISelectionHelper
{
    public static void SelectFirstButton(Transform root)
    {
        Button btn = root.GetComponentInChildren<Button>();
        if (btn != null)
        {
            EventSystem.current.SetSelectedGameObject(btn.gameObject);
        }
    }
}