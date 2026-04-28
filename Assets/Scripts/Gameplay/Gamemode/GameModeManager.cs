using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    public bool infiniteCoins = false;
    public bool godMode = false;

    public bool IsInfiniteCoins() => infiniteCoins;
    public bool IsGodMode() => godMode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameModeManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleInfiniteCoins()
    {
        infiniteCoins = !infiniteCoins;
    }

    public void ToggleGodMode()
    {
        godMode = !godMode;
    }
}
