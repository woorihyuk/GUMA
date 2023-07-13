using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster.Saeuni
{
    public class NewSaeuniCtrl : Monster
    {
        public Vector2[] movePoint;
        public Transform lightningPoint;
        public BoxCollider2D attackRange;
        public LayerMask attackContactLayerMask;

        private ContactFilter2D _attackContactFilter;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private int _positionIndex;
        private float _followingLightningColliderSizeX;
        private bool _isAttack;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            transform.position = movePoint[0];
            _positionIndex = 0;
            _attackContactFilter = new ContactFilter2D
            {
                layerMask = attackContactLayerMask,
                useLayerMask = true
            };
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying) return;
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
        }

        public override void OnMonsterGetDamaged(int dmg)
        {
            if (hp.Value <= 0) return;
            base.OnMonsterGetDamaged(dmg);
            _spriteRenderer.material.color = Color.white;
            Observable.TimerFrame(1, FrameCountType.EndOfFrame)
                .Do(_ => { }, () => { _spriteRenderer.material.color = Color.black; }).Subscribe().AddTo(gameObject);
            if (hp.Value > 0) Move();
        }

        protected override void OnDirectionSet(int direction)
        {
            transform.rotation = Quaternion.Euler(0, direction, 0);
        }

        protected override void OnHpDrown()
        {
            _animator.Play("Die");
            _animator.Update(0);
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "새우니", (float)hp.Value / maxHp.Value);
            if (!_isAttack)
            {
                if (hp.Value > 0)
                {
                    Attack();
                }
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
            switch (_positionIndex)
            {
                case 0:
                    switch (i)
                    {
                        case 0:
                            transform.position = movePoint[1];
                            _positionIndex = 1;
                            break;
                        case 1:
                            transform.position = movePoint[2];
                            _positionIndex = 2;
                            break;
                    }

                    break;
                case 1:
                    transform.position = movePoint[2];
                    _positionIndex = 2;
                    break;
                case 2:
                    switch (i)
                    {
                        case 0:
                            transform.position = movePoint[0];
                            _positionIndex = 0;
                            break;
                        case 1:
                            transform.position = movePoint[1];
                            _positionIndex = 1;
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

        private IEnumerator LightningEffectPlay(float startPos, float endPos)
        {
            _followingLightningColliderSizeX =
                FxPoolManager.Instance.saeuniThunderEffectBluePrefab.attackRangeCollider.size.x;

            int lightningCount;
            int leftRight;
            if (startPos - endPos > 0)
            {
                lightningCount = (int)((startPos - endPos) / _followingLightningColliderSizeX);
                leftRight = -1;
            }
            else
            {
                lightningCount = (int)((endPos - startPos) / _followingLightningColliderSizeX);
                leftRight = 1;
            }

            for (var i = 0; i < lightningCount; i++)
            {
                var lightningPos = new Vector2(startPos + _followingLightningColliderSizeX * i * leftRight, -1.3f);
                FxPoolManager.Instance.saeuniThunderEffectBluePool.Get(out var vThunderEffect);
                vThunderEffect.transform.position = lightningPos;
                vThunderEffect.attackContactFilter = _attackContactFilter;
                yield return YieldInstructionCache.WaitForSeconds(0.05f);
            }
        }

        private void AttackRange(Collider2D range, int dmg)
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

        public void OnAttackEvent1()
        {
            AttackRange(attackRange, 15);
            FxPoolManager.Instance.saeuniThunderEffectRedPool.Get(out var vThunderEffect);
            vThunderEffect.transform.position = lightningPoint.position;
            vThunderEffect.attackContactFilter = _attackContactFilter;
            FxPoolManager.Instance.saeuniAttackEffectPool.Get(out var vAttackEffect);
            vAttackEffect.transform.position = transform.position;
            vAttackEffect.transform.localScale =
                new Vector3(transform.position.x - lastTargetPlayer.player.transform.position.x > 0 ? -1 : 1, 1, 1);
        }

        public void OnAttackEvent2()
        {
            Move();
        }

        #endregion
    }
}