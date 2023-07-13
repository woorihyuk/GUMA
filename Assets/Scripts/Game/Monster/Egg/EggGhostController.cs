using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game.Monster.Egg
{
    public class EggGhostController : Monster
    {
        private bool _isAttack, _attack1Waiting, _attack4Waiting;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _aiMoveCoroutine;
        private IDisposable _moveStateSubscription, _attack2WaitSubscription;
        public float attack2Distance;
        public Transform directionalObjectGroup, smokingPoint;
        public LayerMask attackContactLayerMask;
        public GameObject smoke;

        public PolygonCollider2D[] attackColliders;
        private ContactFilter2D _attackContactFilter;

        private bool _initialized;

        #region 애니메이터 해쉬

        private static readonly int IsWalk = Animator.StringToHash("isWalk");
        private static readonly int Attack1 = Animator.StringToHash("attack1");
        private static readonly int Attack2 = Animator.StringToHash("attack2");

        #endregion

        private void Awake()
        {
            if (GameManager.Instance.isEndWatched)
            {
                gameObject.SetActive(false);
                _initialized = false;
            }
        }

        protected override void Start()
        {
            base.Start();
            _initialized = true;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 3, 1f, 3.5f));
            _moveStateSubscription = isMonsterMoving.DistinctUntilChanged()
                .Subscribe(v => { _animator.SetBool(IsWalk, v); }).AddTo(gameObject);
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
            if (hp.Value <= 0 && _initialized)
            {
                var player = FindObjectOfType<Player.Player>();
                GameManager.Instance.lastPosition = player.transform.position;
                GameManager.Instance.lastDirection = player.lastInputX;
                SceneManager.LoadScene("02");
            }
        }

        protected override void OnHpDrown()
        {
            _animator.Play("Die");
            _animator.Update(0);
            PlayerFoundSubscription.Dispose();
            _moveStateSubscription.Dispose();
            _attack2WaitSubscription?.Dispose();
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
            directionalObjectGroup.localScale = new Vector3(-direction, directionalObjectGroup.localScale.y);
        }

        private void NormalAttack()
        {
            StopCoroutine(_aiMoveCoroutine);
            _animator.SetBool(IsWalk, false);
            _animator.SetBool(Attack1, true);
            _isAttack = true;
            SetDirection();
        }

        protected override void OnPlayerLost()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "달걀귀신", (float) hp.Value / maxHp.Value);
            RefreshHp();

            if (!_isAttack && !_attack4Waiting && !_attack1Waiting)
            {
                StopCoroutine(_aiMoveCoroutine);
                _animator.SetBool(IsWalk, false);
                NormalAttack();
            }
        }

        private void AttackRange(int index, int dmg)
        {
            var players = new List<Collider2D>();
            var counts = attackColliders[index].OverlapCollider(_attackContactFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(dmg);
            }
        }

        private void SetDirection()
        {
            switch (transform.position.x - lastTargetPlayer.player.transform.position.x)
            {
                case > 0:
                    OnDirectionSet(-1);
                    break;
                case < 0:
                    OnDirectionSet(1);
                    break;
            }
        }

        #region 애니메이션 이벤트

        public void OnDieEnd()
        {
            Destroy(gameObject);
        }
        
        public void OnAttack1Event1()
        {
            AttackRange(0, 20);
        }

        public void OnAttack1Event2()
        {
            AttackRange(1, 20);
        }

        public void OnAttack1Event3()
        {
            AttackRange(2, 20);
        }

        public void OnAttack1Event4()
        {
            AttackRange(3, 20);
        }

        public void OnAttack1Event5()
        {
            AttackRange(4, 20);
        }

        public void OnAttack1End()
        {
            _animator.SetBool(Attack1, false);
            _isAttack = false;
            _attack1Waiting = true;

            _attack2WaitSubscription = Observable.Timer(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ =>
                {
                    _attack1Waiting = false;
                    if (!isPlayerFounded.Value)
                    {
                        var playAttack4 = Random.value >= 0.5f;

                        if (playAttack4)
                        {
                            StopCoroutine(_aiMoveCoroutine);
                            _animator.SetBool(IsWalk, false);
                            _isAttack = true;
                            _animator.Play("Attack4Wait");
                            return;
                        }
                        
                        StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 3, 1f, 2f));
                        return;
                    }

                    StopCoroutine(_aiMoveCoroutine);
                    _animator.SetBool(IsWalk, false);

                    if (lastTargetPlayer.distance <= attack2Distance)
                    {
                        SetDirection();
                        _animator.SetBool(Attack2, true);
                        _isAttack = true;
                    }
                    else
                    {
                        NormalAttack();
                    }
                }).AddTo(gameObject);
        }

        public void OnAttack2Event1()
        {
            AttackRange(5, 20);
        }
        
        public void OnAttack2Event2()
        {
            AttackRange( 6, 20);
        }
        
        public void OnAttack2Event3()
        {
            AttackRange(7, 20);
        }
        
        public void OnAttack2Event4()
        {
            AttackRange(8, 20);
        }
        
        public void OnAttack2Event5()
        {
            AttackRange(9, 20);
        }
        
        public void OnAttack2Event6()
        {
            AttackRange(10, 20);
        }
        
        public void OnAttack2Event7()
        {
            AttackRange(11, 20);
        }
        
        public void OnAttack2Event8()
        {
            AttackRange(12, 20);
        }
        
        public void OnAttack2Event9()
        {
            AttackRange(13, 20);
        }
        
        public void OnAttack3Event1()
        {
            AttackRange(14, 30);
        }
        
        public void OnAttack3Event2()
        {
            AttackRange(15, 30);
        }
        
        public void OnAttack3Event3()
        {
            AttackRange(16, 30);
        }

        public void OnAttack3End()
        {
            _animator.SetBool(Attack2, false);
            _isAttack = false;

            _attack2WaitSubscription = Observable.Timer(TimeSpan.FromMilliseconds(1000))
                .Subscribe(_ => { }, () =>
                {
                    if (!isPlayerFounded.Value)
                    {
                        var playAttack4 = Random.value >= 0.5f;

                        if (playAttack4)
                        {
                            StopCoroutine(_aiMoveCoroutine);
                            _animator.SetBool(IsWalk, false);
                            _isAttack = true;
                            _animator.Play("Attack4Wait");
                            return;
                        }
                        
                        StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 3, 1f, 2f));
                        return;
                    }

                    StopCoroutine(_aiMoveCoroutine);
                    _animator.SetBool(IsWalk, false);
                    NormalAttack();
                }).AddTo(gameObject);
        }

        public void OnAttack4Event()
        {
            var dirMultiplier = directionalObjectGroup.localScale.x switch
            {
                > 0 => -1,
                < 0 => 1,
                _ => 0
            };

            for (var i = 0; i < 20; i++)
            {
                FxPoolManager.Instance.eggGhostSmokePool.Get(out var v);
                v.transform.position = smokingPoint.transform.position;
                v.direction = dirMultiplier;
            }
        }

        public void OnAttack4End()
        {
            _isAttack = false;
            _attack4Waiting = true;

            StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(1, 3, 1f, 2f));
            _attack2WaitSubscription = Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(
                _ =>
                {
                    _attack4Waiting = false;
                    if (!isPlayerFounded.Value) return;
                    StopCoroutine(_aiMoveCoroutine);
                    _animator.SetBool(IsWalk, false);
                    NormalAttack();
                });
        }

        #endregion
    }
}