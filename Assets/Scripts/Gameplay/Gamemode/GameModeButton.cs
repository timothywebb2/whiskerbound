using UnityEngine;
using UnityEngine.UI;

public class GameModeButton : MonoBehaviour
{
    public enum ModeType
    {
        InfiniteCoins,
        GodMode
    }

    public ModeType modeType;
    public Image buttonImage;

    void Start()
    {
        UpdateVisual();
    }

    public void OnButtonPressed()
    {
        if (modeType == ModeType.InfiniteCoins)
            GameModeManager.Instance.ToggleInfiniteCoins();
        else if (modeType == ModeType.GodMode)
            GameModeManager.Instance.ToggleGodMode();

        UpdateVisual();
    }

    void UpdateVisual()
    {
        bool active = false;

        if (modeType == ModeType.InfiniteCoins)
            active = GameModeManager.Instance.IsInfiniteCoins();
        else if (modeType == ModeType.GodMode)
            active = GameModeManager.Instance.IsGodMode();

        buttonImage.color = active ? new Color(1f, 0.5f, 0f) : Color.white;
    }
}
