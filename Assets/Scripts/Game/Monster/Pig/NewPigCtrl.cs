using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster.Pig
{
    public class NewPigCtrl : Monster
    {
        public LayerMask attackContactLayerMask;
        private IDisposable _moveStateSubscription;
        public Transform attackRangePos;
        public BoxCollider2D attackRange;
        public float attackRangeCount;

        private ContactFilter2D _attackContactFilter;
        private Coroutine _aiMoveCoroutine;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private int _direction;
        private float _distance;
        private bool _isWait;
        private bool _isAttack;

        private static readonly int IsWalk = Animator.StringToHash("Walk");


        protected override void Start()
        {
            _animator = GetComponent<Animator>();
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 2, 1f, 2f));
            _moveStateSubscription = isMonsterMoving.DistinctUntilChanged()
                .Subscribe(v => { _animator.SetBool(IsWalk, v); }).AddTo(gameObject);
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
        }

        protected override void OnDirectionSet(int direction)
        {
            _spriteRenderer.flipX = direction == 1;
            attackRangePos.localScale = new Vector3(-direction, attackRangePos.localScale.y);
        }

        private void AttackRange(int dmg)
        {
            var players = new List<Collider2D>();
            var counts = attackRange.OverlapCollider(_attackContactFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(dmg);
            }
        }

        private IEnumerator Wait(float i)
        {
            _animator.SetBool("Walk", false);
            _isWait = true;
            yield return YieldInstructionCache.WaitForSeconds(i);
            _isWait = false;
            if (lastTargetPlayer!=null)
            {
                Attack();

            }
        }

        private IEnumerator MoveToPlayer(int direction)
        {
            _animator.SetBool("Walk", true);
            _animator.Play("Pig_Walking");
            while (_distance>attackRangeCount)
            {
                if (lastTargetPlayer!=null)
                {
                    _distance = Vector2.Distance(lastTargetPlayer.player.transform.position, transform.position);
                }
                transform.position += new Vector3(speed * direction, 0) * Time.deltaTime;
                yield return null;
            }
            _animator.SetBool("Attack", true);
            _isAttack = true;
            yield return null;  
        }

        protected override void OnHpDrown()
        {
            _animator.Play("Pig_Die");
            _animator.Update(0);
        }

        private void OnDestroy()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
            Destroy(gameObject);
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "금돼지", (float)hp.Value / maxHp.Value);
            RefreshHp();
            StopCoroutine(_aiMoveCoroutine);
            Attack();
        }

        private void Attack()
        {
            if (lastTargetPlayer != null)
            {
                _distance = Vector2.Distance(lastTargetPlayer.player.transform.position, transform.position);
                _direction = lastTargetPlayer.player.transform.position.x - transform.position.x > 0 ? 1 : -1;
            }

            if (!_isAttack && !_isWait)
            {
                if (_distance > attackRangeCount)
                {
                    OnDirectionSet(_direction);
                    StartCoroutine(MoveToPlayer(_direction));
                }
                else
                {
                    print("tlakf");

                    _animator.SetBool("Attack", true);

                }
            }


        }
        protected override void OnPlayerLost()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
            StopAllCoroutines();
            StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 2, 1f, 2f));
            _moveStateSubscription = isMonsterMoving.DistinctUntilChanged()
                .Subscribe(v => { _animator.SetBool(IsWalk, v); }).AddTo(gameObject);
        }
        #region 애니메이션 이벤트

        public void OnDieEnd()
        {
            Destroy(gameObject);
        }

        public void OnAttackEvent1()
        {
            AttackRange(20);
        }

        public void OnAttackEvent2()
        {
            _animator.SetBool("Attack", false);
            _isAttack = false;
            StartCoroutine(Wait(1.5f));
        }
        #endregion
    }


}