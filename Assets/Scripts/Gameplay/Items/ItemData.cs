using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemId;
    public string displayName;
    public Sprite icon;
    public int baseCost;

    [TextArea(2, 5)]
    public string description;

    public abstract void Use(GameObject player);
}