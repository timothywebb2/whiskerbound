using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    public List<ItemData> items;

    Dictionary<string, ItemData> itemLookup = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var item in items)
        {
            if (item == null)
                continue;

            itemLookup[item.itemId] = item;
        }
    }

    public ItemData GetItem(string id)
    {
        itemLookup.TryGetValue(id, out var item);
        return item;
    }
}