using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameUIManager : PrefabSingleton<GameUIManager>
    {
        private readonly Dictionary<string, HpBarController> _hpBars = new();
        public Image gameOverImage;
        public HpBarController hpBarPrefab;
        public Transform hpBarGroup;
        public CanvasGroup letterBox;

        public CanvasGroup endCg;

        public TMP_Text dialogText;
        public Image dialogBackgroundImage, endTriangle;
        public Image[] dialogCharacters;
        public Transform dialogBackgroundStartTf, dialogBackgroundEndTf;

        public Animator hpAnimator, hpBackgroundAnimator;
        public Image hpBackgroundImage, hpImage;
        public Image[] inventorySlots;
        public GameObject apple;

        // 아이템 임시 등록
        public enum ItemType
        {
            None = -1,
            Apple = 0
        }

        public Image[] itemsImagesDb;
        public ItemType[] itemsData;
        public Image[] itemsImage;
        public int inventoryLimit = 3;

        public Transform saveStart, saveEnd;
        public TMP_Text saveText;
        private Sequence _saveTextSequence;

        public CanvasGroup pauseGroup;

        private void Start()
        {
            itemsData = new ItemType[inventoryLimit];
            itemsImage = new Image[inventoryLimit];
            for (var i = 0; i < itemsData.Length; i++)
            {
                itemsData[i] = ItemType.None;
            }
        }

        private void OnDestroy()
        {
            if (hpAnimator.playableGraph.IsValid())
                hpAnimator.playableGraph.Destroy();
            if (hpBackgroundAnimator.playableGraph.IsValid())
                hpBackgroundAnimator.playableGraph.Destroy();
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("Title");
            Time.timeScale = 1;
            pauseGroup.gameObject.SetActive(false);
        }

        public void ShowPauseScreen()
        {
            SetActivePlayerHud(false);
            Time.timeScale = 0;
            pauseGroup.gameObject.SetActive(true);
        }

        public void ClosePauseScreen()
        {
            SetActivePlayerHud(true);
            Time.timeScale = 1;
            pauseGroup.gameObject.SetActive(false);
        }

        public void ShowEndCg()
        {
            endCg.gameObject.SetActive(true);
            endCg.DOFade(1, 0.5f);
        }

        public void SetActivePlayerHud(bool value)
        {
            hpBackgroundImage.gameObject.SetActive(value);
            hpImage.gameObject.SetActive(value);
            foreach (var image in inventorySlots)
            {
                image.gameObject.SetActive(value);
            }
        }

        public void ShowSaveMsg()
        {
            _saveTextSequence?.Kill(true);
            saveText.transform.DOMoveY(saveStart.transform.position.y, 0).Play();
            _saveTextSequence = DOTween.Sequence()
                .PrependCallback(() => saveText.gameObject.SetActive(true))
                .Append(saveText.transform.DOMoveY(saveEnd.transform.position.y, 0.5f))
                .Insert(2.5f, saveText.transform.DOMoveY(saveStart.transform.position.y, 0.5f).SetEase(Ease.InBack)
                    .OnComplete(() => { saveText.gameObject.SetActive(false); })
                    .OnKill(() => { saveText.gameObject.SetActive(false); }));
            _saveTextSequence.Restart();
        }

        public void SetHpBarPercent(string key, float hpValue)
        {
            _hpBars[key].hpBarImage.fillAmount = hpValue;
        }

        public void PushHpBar(string key, string targetName)
        {
            var hpBarObj = Instantiate(hpBarPrefab, hpBarGroup);
            _hpBars.Add(key, hpBarObj);
            hpBarObj.targetNameText.text = targetName;
        }

        public void PopHpBar(string key)
        {
            Destroy(_hpBars[key].gameObject);
            _hpBars.Remove(key);
        }

        public bool TryPopHpBar(string key)
        {
            if (!_hpBars.ContainsKey(key)) return false;
            PopHpBar(key);
            return true;
        }

        public bool TryPushHpBar(string key, string targetName)
        {
            if (_hpBars.ContainsKey(key)) return false;
            PushHpBar(key, targetName);
            return true;
        }

        public bool TryPushHpBar(string key, string targetName, float hpValue)
        {
            if (_hpBars.ContainsKey(key)) return false;
            PushHpBar(key, targetName);
            SetHpBarPercent(key, hpValue);
            return true;
        }

        private Image GetItemImagePrefab(ItemType type)
        {
            return itemsImagesDb[(int)type];
        }

        public void AddItemToInventoryHotSlot(ItemType type)
        {
            var result = false;
            
            for (var i = 0; i < itemsData.Length; i++)
            {
                if (itemsData[i] != ItemType.None) continue;
                itemsData[i] = type;
                result = true;
                break;
            }

            if (result)
            {
                RefreshInventoryHotSlot();
            }
            else
            {
                Debug.LogWarning("[AddItemToInventory] Inventory Full!");
            }
        }

        public bool UseItemFromInventoryHotSlot(int index, out ItemType usedItem)
        {
            if (itemsData[index] == ItemType.None)
            {
                usedItem = ItemType.None;
                return false;
            }

            usedItem = itemsData[index];
            itemsData[index] = ItemType.None;
            RefreshInventoryHotSlot();
            return true;
        }

        private void RefreshInventoryHotSlot()
        {
            for (var i = 0; i < itemsData.Length; i++)
            {
                // 인벤토리[i]에 아이템이 없으면
                if (itemsData[i] == ItemType.None)
                {
                    // 현재 UI 슬롯에 생성된 이미지가 있으면
                    if (itemsImage[i])
                    {
                        // 제거
                        Destroy(itemsImage[i].gameObject);
                        itemsImage[i] = null;
                    }
                    continue;
                }
                
                var itemPrefab = GetItemImagePrefab(itemsData[i]);
                
                if (!itemsImage[i]) // 현재 UI 슬롯에 생성된 이미지가 없으면
                {
                    // 이미지 생성
                    itemsImage[i] = Instantiate(itemPrefab, inventorySlots[i].transform);
                }
                else if (itemsImage[i].sprite != itemPrefab.sprite) // 생성된 이미지가 존재하고, 스프라이트가 다르면 -> 다른 프리펩이면
                {
                    // 파괴 후 생성
                    Destroy(itemsImage[i].gameObject);
                    itemsImage[i] = null;
                    itemsImage[i] = Instantiate(itemPrefab, inventorySlots[i].transform);
                }
            }
        }

        public void ShowApple(bool i)
        {
            apple.SetActive(i);
        }
    }
}