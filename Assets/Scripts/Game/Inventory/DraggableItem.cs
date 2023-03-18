using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Slot _slot;
    private GameObject _followingObject;
    
    private void Start()
    {
        _slot = GetComponentInParent<Slot>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_slot.itemData == null) return;
        _followingObject = Instantiate(_slot.itemImage.gameObject, OverlayCanvas.Instance.canvas.transform);
        ((RectTransform)_followingObject.transform).anchorMax = new Vector2(0.5f, 0.5f);
        ((RectTransform)_followingObject.transform).anchorMin = new Vector2(0.5f, 0.5f);
        ((RectTransform)_followingObject.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 52f);
        ((RectTransform)_followingObject.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 52f);
        SlotSaver.startedSlot = _slot;

        var itemColor = _slot.itemImage.color;
        _slot.itemImage.color = new Color(itemColor.r, itemColor.g, itemColor.b, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_followingObject) return;
        _followingObject.transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_followingObject) Destroy(_followingObject);
        SlotSaver.startedSlot = null;
        var itemColor = _slot.itemImage.color;
        _slot.itemImage.color = new Color(itemColor.r, itemColor.g, itemColor.b, 1);
    }
}
