using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class AutoSelectFirstButton : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(SelectFirstButton());
    }

    private IEnumerator SelectFirstButton()
    {
        while (EventSystem.current == null)
            yield return null;

        EventSystem.current.SetSelectedGameObject(null);

        Button buttonToSelect = null;

        while (buttonToSelect == null)
        {
            buttonToSelect = GetComponentsInChildren<Button>(true)
                .FirstOrDefault(b => b.gameObject.activeInHierarchy && b.interactable);

            yield return null;
        }

        yield return null;

        EventSystem.current.SetSelectedGameObject(buttonToSelect.gameObject);
    }
}
