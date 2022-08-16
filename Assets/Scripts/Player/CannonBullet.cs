using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public int dmg;
    
    public BoxCollider2D attackCollider;
    [HideInInspector] public ContactFilter2D attackCheckFilter;
    public SpriteRenderer sr;
    public Sprite[] animationSprites;
    public float[] xMoveAmounts;

    private Sequence _sequence;
    
    public void PlayTween()
    {
        var originPosX = transform.position.x;
        sr.sprite = animationSprites[0];
        _sequence = DOTween.Sequence()
            .Insert(0.1f, transform.DOMoveX(originPosX + xMoveAmounts[1] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.1f, () => sr.sprite = animationSprites[1])
            .Insert(0.2f, transform.DOMoveX(originPosX + xMoveAmounts[2] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.2f, () => sr.sprite = animationSprites[2])
            .Insert(0.3f, transform.DOMoveX(originPosX + xMoveAmounts[3] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.3f, () => sr.sprite = animationSprites[3])
            .Insert(0.4f, transform.DOMoveX(originPosX + xMoveAmounts[4] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.4f, () => sr.sprite = animationSprites[4])
            .Insert(0.5f, transform.DOMoveX(originPosX + xMoveAmounts[5] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.5f, () => sr.sprite = animationSprites[5])
            .Insert(0.6f, transform.DOMoveX(originPosX + xMoveAmounts[6] * (!sr.flipX ? 1 : -1), 0.1f)
                .SetEase(Ease.Linear))
            .InsertCallback(0.6f, () => sr.sprite = animationSprites[6])
            .OnUpdate(AttackEffect).OnComplete(OnAnimationEnd).Play();
    }

    private void AttackEffect()
    {
        var enemies = new List<Collider2D>();
        
        var counts = attackCollider.OverlapCollider(attackCheckFilter, enemies);
        if (counts == 0) return;
        var entity = enemies[0].GetComponent<MonsterMove>();
        entity.OnMonsterGetDamaged(dmg);
        _sequence.Kill();
        OnAnimationEnd();
    }
    
    private void OnAnimationEnd()
    {
        FxPoolManager.Instance.bulletPool.Release(this);
    }
}
