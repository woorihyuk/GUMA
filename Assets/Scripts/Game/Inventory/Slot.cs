using Game;
using Game.Items;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
    public Image itemHolder, itemImage;
    [CanBeNull] public ItemBase itemData;

    public void SetImage([CanBeNull] Sprite sprite)
    {
        if (sprite != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            itemImage.sprite = null;
        }
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (SlotSaver.startedSlot == this) return;
        if (!SlotSaver.startedSlot) return;
        if (SlotSaver.startedSlot.itemData == null) return;
        var startedSlotItemData = SlotSaver.startedSlot.itemData;
        var startedSlotItemSprite = SlotSaver.startedSlot.itemImage.sprite;

        if (itemData != null) // 아이템 교환
        {
            SlotSaver.startedSlot.SetImage(itemImage.sprite);
            SlotSaver.startedSlot.itemData = itemData;
            SetImage(startedSlotItemSprite);
            itemData = startedSlotItemData;
        }
        else // 아이템 변경
        {
            SlotSaver.startedSlot.SetImage(null);
            SlotSaver.startedSlot.itemData = null;
            SetImage(startedSlotItemSprite);
            itemData = startedSlotItemData;
        }
    }
}
