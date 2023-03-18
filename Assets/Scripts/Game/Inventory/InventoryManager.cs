using System.Collections.Generic;
using Items;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private readonly Dictionary<string, ItemBase> _itemDataDictionary = new();
    private InventoryUI _inventoryUI;

    public int gold = 100;

    public BoolReactiveProperty hasChange = new(true);
    public Sprite[] itemSprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _itemDataDictionary.Clear();
        _itemDataDictionary.Add("apple", new ItemBase { itemCode = 0, name = "사과", currentQuantity = 0, maxQuantity = 1, description = "체력 회복" });
    }

    private void FindUIComponent()
    {
        if (!_inventoryUI) _inventoryUI = FindAnyObjectByType<InventoryUI>();
    }
    
    private void SetItemToSlot([CanBeNull] ItemBase itemData, int position = -1)
    {
        FindUIComponent();
        
        if (position == -1)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _inventoryUI.slots.Length; i++)
            {
                if (_inventoryUI.slots[i].itemData != null) continue;
                _inventoryUI.slots[i].itemData = itemData;
                _inventoryUI.slots[i].SetImage(itemData == null ? null : itemSprites[itemData.itemCode]);
                return;
            }
            Debug.LogWarning("인벤토리가 가득 찼습니다!");
        }
        else
        {
            _inventoryUI.slots[position].itemData = itemData;
            _inventoryUI.slots[position].SetImage(itemData == null ? null : itemSprites[itemData.itemCode]);
        }
    }

    private int FindItemPosition(int itemCode)
    {
        FindUIComponent();
        
        for (var i = 0; i < _inventoryUI.slots.Length; i++)
        {
            if (_inventoryUI.slots[i].itemData?.itemCode == itemCode) return i;
        }
        return -1;
    }

    private void AddNewItem(ItemBase itemBase)
    {
        itemBase.currentQuantity++;
        SetItemToSlot(itemBase);
    }

    public void AddItem(string itemName)
    {
        var itemBase = FindItemBase(itemName);
        if (itemBase == null)
        {
            Debug.LogError($"아이템 베이스 데이터를 찾지 못했습니다.");
            return;
        }

        var i = FindItemPosition(itemBase.itemCode);
        if (i == -1)
        {
            AddNewItem(itemBase);
            hasChange.Value = true;
        }
        else
        {
            var targetItemInSlot = _inventoryUI.slots[i].itemData;
            if (targetItemInSlot == null)
            {
                Debug.LogWarning($"Null인 슬롯[{i}]에 아이템을 추가하려고 했습니다.");
                return;
            }
            if (targetItemInSlot.maxQuantity < targetItemInSlot.currentQuantity + 1)
            {
                // Debug.LogWarning($"아이템 최대 수량을 넘으려고 했습니다. 새 아이템으로 추가합니다.");
                AddNewItem(itemBase);
                hasChange.Value = true;
                return;
            }
            
            targetItemInSlot.currentQuantity++;
        }
        
        hasChange.Value = true;
    }

    /*public void RemoveItem(string itemName)
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

        hasChange.Value = true;
    }*/

    private ItemBase FindItemBase(string itemName)
    {
        return _itemDataDictionary.ContainsKey(itemName) ? _itemDataDictionary[itemName].DeepCopy() : null;
    }
}