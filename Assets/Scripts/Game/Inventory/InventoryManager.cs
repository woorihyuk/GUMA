using Game.Items;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Game.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        private InventoryUIController _inventoryUIController;

        public int gold = 100;

        public BoolReactiveProperty hasChange = new(true);
        public Sprite[] itemSprites;


        private void Start()
        {
            _inventoryUIController = GetComponent<InventoryUIController>();
        }

        private void SetItemToSlot([CanBeNull] ItemBase itemData, int position = -1)
        {
            if (position == -1)
            {
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < _inventoryUIController.slots.Length; i++)
                {
                    if (_inventoryUIController.slots[i].itemData != null) continue;
                    _inventoryUIController.slots[i].itemData = itemData;
                    _inventoryUIController.slots[i].SetImage(itemData == null ? null : itemSprites[itemData.itemCode]);
                    return;
                }
                Debug.LogWarning("인벤토리가 가득 찼습니다!");
            }
            else
            {
                _inventoryUIController.slots[position].itemData = itemData;
                _inventoryUIController.slots[position].SetImage(itemData == null ? null : itemSprites[itemData.itemCode]);
            }
        }

        private int FindItemPosition(int itemCode)
        {
            for (var i = 0; i < _inventoryUIController.slots.Length; i++)
            {
                if (_inventoryUIController.slots[i].itemData?.itemCode == itemCode) return i;
            }
            return -1;
        }

        private void AddNewItem(ItemBase itemBase)
        {
            itemBase.currentQuantity++;
            SetItemToSlot(itemBase);
        }

        public void UseItem(int index)
        {
            if (_inventoryUIController.slots[index].itemData == null) return;
            _inventoryUIController.slots[index].itemData.Use();
            _inventoryUIController.slots[index].itemData.currentQuantity--;
            
            if (_inventoryUIController.slots[index].itemData.currentQuantity == 0)
            {
                _inventoryUIController.slots[index].itemData = null;
                _inventoryUIController.slots[index].SetImage(null);
            }
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
                var targetItemInSlot = _inventoryUIController.slots[i].itemData;
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
            if (itemName == "apple")
            {
                return new Apple();
            }

            return null;
        }
    }
}