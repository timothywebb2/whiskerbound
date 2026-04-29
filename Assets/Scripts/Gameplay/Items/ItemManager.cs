using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class ItemData
    {
        public int currentCharges = 0;
        public int maxCharges = 5;
    }

    public Dictionary<string, ItemData> items = new Dictionary<string, ItemData>();

    public void InitializeItem(string itemId)
    {
        if (!items.ContainsKey(itemId))
        {
            items[itemId] = new ItemData();
        }
    }

    public int GetCharges(string itemId)
    {
        InitializeItem(itemId);
        return items[itemId].currentCharges;
    }

    public bool UseItem(string itemId)
    {
        InitializeItem(itemId);

        if (items[itemId].currentCharges > 0)
        {
            items[itemId].currentCharges--;
            PlayerPrefs.SetInt(itemId, items[itemId].currentCharges);
            return true;
        }

        return false;
    }

    public void AddItemCharge(string itemId, int amount)
    {
        InitializeItem(itemId);

        items[itemId].currentCharges += amount;

        if (items[itemId].currentCharges > items[itemId].maxCharges)
            items[itemId].currentCharges = items[itemId].maxCharges;
    }
}
