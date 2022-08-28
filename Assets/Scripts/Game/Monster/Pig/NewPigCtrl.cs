using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster.Pig
{
    public class NewPigCtrl : Monster
    {
        private SpriteRenderer _spriteRenderer;
        public Transform attackRange;

        private void Start()
        {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public override void OnMonsterGetDamaged(int dmg)
        {
            if (hp.Value <= 0) return;
            base.OnMonsterGetDamaged(dmg);
            _spriteRenderer.material.color = Color.white;
            Observable.TimerFrame(1, FrameCountType.EndOfFrame)
                .Do(_ => { }, () => { _spriteRenderer.material.color = Color.black; }).Subscribe().AddTo(gameObject);
        }

        protected override void OnDirectionSet(int direction)
        {
            _spriteRenderer.flipX = direction == 1;
            attackRange.localScale = new Vector3(-direction, attackRange.localScale.y);
        }

        protected override void OnHpDrown()
        {
            throw new NotImplementedException();
        }

        protected override void OnPlayerFound()
        {
            throw new NotImplementedException();
        }

        protected override void OnPlayerLost()
        {
            throw new NotImplementedException();
        }
    }
}