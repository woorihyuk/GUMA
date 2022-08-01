using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemGroup;
    public ItemSelectableObject itemPrefab;
    public List<ItemSelectableObject> spawnedButtons = new();

    // For Tween
    public CanvasGroup canvasGroup;
    public Text inventoryText;
    public Text goldText;

    private Sequence _sequence;
    private bool _canInput, _isOpen;

    private void Start()
    {
        _canInput = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && _canInput)
        {
            if (_isOpen) Close();
            else Open();
        }
    }

    public void Open()
    {
        _canInput = false;

        if (InventoryManager.Instance.hasChange)
        {
            var itemObjects = itemGroup.transform.GetComponentsInChildren<ItemSelectableObject>();
            foreach (var itemObj in itemObjects)
            {
                Destroy(itemObj.gameObject);
            }

            foreach (var item in InventoryManager.Instance.items)
            {
                var itemSelectableObj = Instantiate(itemPrefab, itemGroup);
                itemSelectableObj.SetData(item);
            }

            InventoryManager.Instance.hasChange = false;
        }

        _sequence?.Kill();
        canvasGroup.DOKill(true);
        
        inventoryText.DOFade(0, 0);
        goldText.text = $"{InventoryManager.Instance.gold} 원";

        canvasGroup.DOFade(1, 0.3f)
            .OnComplete(() =>
            {
                _isOpen = true;
                _canInput = true;
                canvasGroup.blocksRaycasts = true;
                
                _sequence = DOTween.Sequence()
                    .Append(inventoryText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(inventoryText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .SetDelay(0.2f).Play();
            });
    }

    public void Close()
    {
        _canInput = false;
        
        canvasGroup.DOFade(0, 0.3f)
            .OnComplete(() =>
            {
                _isOpen = false;
                _canInput = true;
                canvasGroup.blocksRaycasts = false;
            });
    }
}