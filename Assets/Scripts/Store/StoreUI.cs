using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public StoreManager storeManager;
    public Transform itemGroup;
    public ItemButton itemPrefab;
    public List<ItemButton> spawnedButtons = new();

    // For Tween
    public CanvasGroup canvasGroup;
    public Text storeText;
    public Text goldText;

    // For Test
    public StoreBuyPopUpTest popUp;
    
    private Sequence _sequence;
    private bool _canInput = true, _isOpen;

    private void Start()
    {
        for (var i = 0; i < storeManager.sellingItems.Count; i++)
        {
            var item = storeManager.sellingItems[i];
            var itemBtn = Instantiate(itemPrefab, itemGroup);
            itemBtn.SetTextData(item);
            var itemIndex = i;
            itemBtn.button.onClick.AddListener(() => OnSelectItem(itemIndex));
            if (storeManager.sellingItems[i].currentQuantity == 0)
                itemBtn.button.interactable = false;
            spawnedButtons.Add(itemBtn);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && _canInput)
        {
            if (_isOpen) Close();
            else Open();
        }
    }

    public void Open()
    {
        _canInput = false;

        _sequence?.Kill();
        DOTween.Kill("goldText01");
        canvasGroup.DOKill(true);
        
        storeText.DOFade(0, 0);
        DOVirtual.Int(0, InventoryManager.Instance.gold, 1, value =>
        {
            goldText.text = $"{value} 원";
        }).SetId("goldText01");

        canvasGroup.DOFade(1, 0.3f)
            .OnComplete(() =>
            {
                _isOpen = true;
                _canInput = true;
                canvasGroup.blocksRaycasts = true;
                
                _sequence = DOTween.Sequence()
                    .Append(storeText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(0, 0.05f).SetDelay(0.05f))
                    .Append(storeText.DOFade(1, 0.05f).SetDelay(0.05f))
                    .SetDelay(0.5f).Play();
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

    private void OnSelectItem(int index)
    {
        if (!_canInput) return;
        popUp.SetData(storeManager.sellingItems[index]);
        popUp.buyButton.onClick.RemoveAllListeners();
        popUp.cancelButton.onClick.RemoveAllListeners();
        popUp.buyButton.onClick.AddListener(() =>
        {
            if (InventoryManager.Instance.gold < storeManager.sellingItems[index].cost) return;
            var oldGold = InventoryManager.Instance.gold;
            InventoryManager.Instance.gold -= storeManager.sellingItems[index].cost;
            DOTween.Kill("goldText01");
            DOVirtual.Int(oldGold, InventoryManager.Instance.gold, 1, value =>
            {
                goldText.text = $"{value} 원";
            }).SetId("goldText01");
            InventoryManager.Instance.AddItem(storeManager.sellingItems[index].name);
            popUp.canvasGroup.DOFade(0, 0.5f)
                .OnComplete(() => popUp.canvasGroup.gameObject.SetActive(false));
            if (--storeManager.sellingItems[index].currentQuantity == 0)
                spawnedButtons[index].button.interactable = false;
            spawnedButtons[index].SetTextData(storeManager.sellingItems[index]);
        });
        popUp.cancelButton.onClick.AddListener(() =>
        {
            popUp.canvasGroup.DOFade(0, 0.5f)
                .OnComplete(() => popUp.canvasGroup.gameObject.SetActive(false));
        });
        popUp.canvasGroup.DOFade(1, 0.5f);
        popUp.canvasGroup.gameObject.SetActive(true);
    }
}