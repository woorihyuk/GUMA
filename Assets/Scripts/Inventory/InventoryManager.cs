using System.Collections.Generic;
using Items;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemBase> items = new();
    private readonly Dictionary<string, ItemBase> _itemDataDictionary = new();

    public int gold = 100;

    public bool hasChange = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _itemDataDictionary.Clear();
        _itemDataDictionary.Add("테스트 아이템 1", new ItemBase { name = "테스트 아이템 1", currentQuantity = 0, maxQuantity = 100, description = "" });
        _itemDataDictionary.Add("테스트 아이템 2", new ItemBase { name = "테스트 아이템 2", currentQuantity = 0, maxQuantity = 100, description = "" });
        _itemDataDictionary.Add("테스트 아이템 3", new ItemBase { name = "테스트 아이템 3", currentQuantity = 0, maxQuantity = 100, description = "" });
    }

    public void AddItem(string itemName)
    {
        var i = items.FindIndex(x => x.name == itemName);
        if (i == -1)
        {
            var itemBase = FindItemBase(itemName);
            if (itemBase == null)
            {
                Debug.LogError($"아이템 베이스 데이터를 찾지 못했습니다.");
                return;
            }

            itemBase.currentQuantity++;
            items.Add(itemBase);
        }
        else
        {
            if (items[i].maxQuantity < items[i].currentQuantity + 1)
            {
                Debug.LogWarning($"아이템 최대 수량을 넘으려고 했습니다. AddItem 작업을 무시합니다.");
                return;
            }

            items[i].currentQuantity++;
        }
        
        hasChange = true;
    }

    public void RemoveItem(string itemName)
    {
        var i = items.FindIndex(x => x.name == itemName);

        if (i == -1)
        {
            Debug.LogError($"인벤토리에서 아이템을 찾지 못했습니다.");
            return;
        }

        if (items[i].currentQuantity - 1 <= 0)
        {
            items.RemoveAt(i);
        }
        else
        {
            items[i].currentQuantity--;
        }

        hasChange = true;
    }

    public ItemBase FindItemBase(string itemName)
    {
        return _itemDataDictionary.ContainsKey(itemName) ? _itemDataDictionary[itemName].DeepCopy() : null;
    }
}