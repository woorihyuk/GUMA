using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Effect
{
    public class DamageText : MonoBehaviour
    {
        public TMP_Text text;

        public void Play()
        {
            Transform transform1;
            (transform1 = transform).DOKill();
            text.alpha = 1;
            
            transform.DOMoveY(transform1.position.y + 1, 0.5f);
            text.DOFade(0, 0.25f).SetDelay(0.25f)
                .OnComplete(() => FxPoolManager.Instance.damageTextPool.Release(this));
        }
        
        public void SetText(int value)
        {
            text.text = value.ToString();
        }
    }
}