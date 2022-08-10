using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : PrefabSingleton<GameUIManager>
{
    private readonly Dictionary<string, HpBarController> _hpBars = new();
    public Image keyHintE, gameOverImage;
    public HpBarController hpBarPrefab;
    public Transform hpBarGroup;
    public Text text1, text2;
    public CanvasGroup letterBox;
    
    public TMP_Text dialogText;
    public Image dialogBackgroundImage, endTriangle;
    public Image[] dialogCharacters;
    public Transform dialogBackgroundStartTf, dialogBackgroundEndTf;

    public Transform saveStart, saveEnd;
    public TMP_Text saveText;
    private Sequence _saveTextSequence;

    public void ShowSaveMsg()
    {
        _saveTextSequence?.Kill(true);
        saveText.transform.DOMoveY(saveStart.transform.position.y, 0).Play();
        _saveTextSequence = DOTween.Sequence()
            .PrependCallback(() => saveText.gameObject.SetActive(true))
            .Append(saveText.transform.DOMoveY(saveEnd.transform.position.y, 0.5f))
            .Insert(2.5f, saveText.transform.DOMoveY(saveStart.transform.position.y, 0.5f).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    saveText.gameObject.SetActive(false);
                })
                .OnKill(() =>
                {
                    saveText.gameObject.SetActive(false);
                }));
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
}
