using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster.Seauni
{
    public class NewSeauniCtrl : Monster
    {
        public Vector2[] movePoint;
        public Transform lightningPoint;
        public GameObject lightning;
        public GameObject Followinglightning;
        public GameObject attakEffect;
        public BoxCollider2D attackRange;
        public LayerMask attackContactLayerMask;

        private ContactFilter2D _attackContactFilter;
        private BoxCollider2D FollowinglightningCollider;
        private BoxCollider2D FollowinglightningRange;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private int position;
        private float FollowinglightningColliderSizeX;
        private bool _isAttack;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            FollowinglightningCollider = Followinglightning.GetComponent<BoxCollider2D>();
            FollowinglightningColliderSizeX = FollowinglightningCollider.size.x;
            transform.position = movePoint[0];
            position = 0;
            _attackContactFilter = new ContactFilter2D
            {
                layerMask = attackContactLayerMask,
                useLayerMask = true
            };
        }

        public override void OnMonsterGetDamaged(int dmg)
        {
            if (hp.Value <= 0) return;
            base.OnMonsterGetDamaged(dmg);
            _spriteRenderer.material.color = Color.white;
            Observable.TimerFrame(1, FrameCountType.EndOfFrame)
                .Do(_ => { }, () => { _spriteRenderer.material.color = Color.black; }).Subscribe().AddTo(gameObject);
            if (hp.Value>0) Move();
        }

        protected override void OnDirectionSet(int direction)
        {
            transform.rotation = Quaternion.Euler(0, direction, 0);
        }

        protected override void OnHpDrown()
        {
            print("die");
            _animator.Play("Die");
            _animator.Update(0);
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "새우니", (float)hp.Value / maxHp.Value);
            if (!_isAttack)
            {
                Attack();
            }
        }

        protected override void OnPlayerLost()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
        }

        private void Move()
        {
            int i = Random.Range(0, 2);
            Vector2 startPos = transform.position;
            switch (position)
            {
                case 0:
                    switch (i)
                    {
                        case 0:
                            transform.position = movePoint[1];
                            position = 1;
                            break;
                        case 1:
                            transform.position = movePoint[2];
                            position = 2;
                            break;
                    }
                    break;
                case 1:
                    transform.position = movePoint[2];
                    position = 2;
                    break;
                case 2:
                    transform.position = movePoint[3];
                    position = 3;
                    break;

                case 3:
                    switch (i)
                    {
                        case 0:
                            transform.position = movePoint[0];
                            position = 0;
                            break;
                        case 1:
                            transform.position = movePoint[1];
                            position = 1;
                            break;
                    }
                    break;


            }
            StartCoroutine(LightningEffectPlay(startPos.x, transform.position.x));

        }

        private void Attack()
        {
            switch (transform.position.x - lastTargetPlayer.player.transform.position.x)
            {
                case > 0:
                    OnDirectionSet(-180);
                    break;
                case < 0:
                    OnDirectionSet(0);
                    break;
            }
            _animator.Play("Attack_Motion");
        }

        public IEnumerator LightningEffectPlay(float startPos, float endPos)
        {
            int lightningCount;
            int leftRight;
            if (startPos - endPos > 0)
            {
                lightningCount = (int)((startPos - endPos) / FollowinglightningColliderSizeX);
                leftRight = -1;
            }
            else
            {
                lightningCount = (int)((endPos - startPos) / FollowinglightningColliderSizeX);
                leftRight = 1;
            }
            for (int i = 0; i < lightningCount; i++)
            {
                var lightningPos = new Vector2(startPos + FollowinglightningColliderSizeX * i*leftRight, -1.3f);
                GameObject lightning = Instantiate(Followinglightning, lightningPos, Quaternion.identity);
                FollowinglightningRange = lightning.GetComponent<BoxCollider2D>();
                AttackRange(FollowinglightningRange, 15);
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }
        }

        private void AttackRange(BoxCollider2D range, int dmg)
        {
            var players = new List<Collider2D>();
            var counts = range.OverlapCollider(_attackContactFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(dmg);
            }
        }

       
        #region 애니메이션 이벤트

        public void OnDieEnd()
        {
            Destroy(gameObject);
        }

        public void AttackEvent1()
        {
            Instantiate(lightning, lightningPoint.position, Quaternion.identity);
            Instantiate(attakEffect, transform.position, transform.rotation);
            AttackRange(attackRange,15);
        }

        public void AttackEvent2()
        {
            _animator.Play("New State");
            Move();
        }
        #endregion
    }
}

