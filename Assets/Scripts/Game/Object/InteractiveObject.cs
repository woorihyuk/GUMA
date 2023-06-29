﻿using DG.Tweening;
using UnityEngine;

public enum InteractiveObjectType
{
    None,
    Sign,
    Door,
    SavePoint,
    Tree,
    Item
}

public class InteractiveObject : MonoBehaviour
{
    public InteractiveObjectType objectType;
    public SpriteRenderer keyHintSprite;

    private Sequence _sequence;
    private float _keyHintStartPos;

    private void Start()
    {
        if (keyHintSprite)
        {
            _keyHintStartPos = keyHintSprite.transform.localPosition.y;
        }
    }

    public virtual void OnInteract()
    {
    }

    public void OnSelect()
    {
        if (keyHintSprite)
        {
            keyHintSprite.gameObject.SetActive(true);
            _sequence = DOTween.Sequence()
                .Prepend(keyHintSprite.transform.DOLocalMoveY(_keyHintStartPos - 0.1f, 1f))
                .Append(keyHintSprite.transform.DOLocalMoveY(_keyHintStartPos, 1f))
                .SetLoops(-1);
            _sequence.Restart();
        }
    }

    public void OnDeselect()
    {
        if (keyHintSprite)
        {
            keyHintSprite.gameObject.SetActive(false);
            keyHintSprite.transform.DOLocalMoveY(_keyHintStartPos, 0).SetUpdate(true).Play();
            _sequence?.Kill();
        }
    }
}