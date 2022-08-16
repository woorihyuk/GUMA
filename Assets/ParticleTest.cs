using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    public SpriteRenderer sr;
    public ParticleTestController controller;
    public List<Sprite> sprites;

    private void OnEnable()
    {
        DOTween.Sequence()
            .Insert(0, transform.DOScale(0, 1f))
            .InsertCallback(0, () => sr.sprite = sprites[7])
            .InsertCallback(0.125f, () => sr.sprite = sprites[6])
            .InsertCallback(0.250f, () => sr.sprite = sprites[5])
            .InsertCallback(0.375f, () => sr.sprite = sprites[4])
            .InsertCallback(0.5f, () => sr.sprite = sprites[3])
            .InsertCallback(0.625f, () => sr.sprite = sprites[2])
            .InsertCallback(0.75f, () => sr.sprite = sprites[1])
            .InsertCallback(0.875f, () => sr.sprite = sprites[0])
            .OnComplete(Release).Play();
    }

    private void Release()
    {
        controller.testPool.Release(this);
    }
}