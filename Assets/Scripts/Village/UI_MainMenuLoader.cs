using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenuLoader : MonoBehaviour
{
    // Call this from Button_ReturnToMainMenu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MAIN MENU");
    }
}
