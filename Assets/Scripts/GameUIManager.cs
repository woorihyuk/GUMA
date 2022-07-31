using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public Image keyHintE, gameOverImage;
    public HpBarController hpBarPrefab;
    public Transform hpBarGroup;
    public readonly Dictionary<string, HpBarController> hpBars = new();
    public Text text1, text2;

    public void SetHpBarPercent(string key, float hpValue)
    {
        hpBars[key].hpBarImage.fillAmount = hpValue;
    }

    public void PushHpBar(string key, string targetName)
    {
        var hpBarObj = Instantiate(hpBarPrefab, hpBarGroup);
        hpBars.Add(key, hpBarObj);
        hpBarObj.targetNameText.text = targetName;
    }

    public void PopHpBar(string key)
    {
        Destroy(hpBars[key].gameObject);
        hpBars.Remove(key);
    }

    public bool TryPopHpBar(string key)
    {
        if (!hpBars.ContainsKey(key)) return false;
        PopHpBar(key);
        return true;
    }
    
    public bool TryPushHpBar(string key, string targetName)
    {
        if (hpBars.ContainsKey(key)) return false;
        PushHpBar(key, targetName);
        return true;
    }
    
    public bool TryPushHpBar(string key, string targetName, float hpValue)
    {
        if (hpBars.ContainsKey(key)) return false;
        PushHpBar(key, targetName);
        SetHpBarPercent(key, hpValue);
        return true;
    }
}
