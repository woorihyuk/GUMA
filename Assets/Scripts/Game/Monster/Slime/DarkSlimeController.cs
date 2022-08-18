using System;
using UniRx;
using UnityEngine;

namespace Game.Monster.Slime
{
    public class DarkSlimeController : Monster
    {
        public SlimeAttackController slimeAttackController;

        private bool _isAttack;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _aiMoveCoroutine;
        private IDisposable _playerFoundSubscription;

        #region 애니메이터 해쉬

        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        #endregion

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(0, 0, 0.3f, 3f));
            slimeAttackController.Initialize();
            _playerFoundSubscription = isPlayerFounded.DistinctUntilChanged().Subscribe(v =>
            {
                if (!v) GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
            }).AddTo(gameObject);
        }

        private void OnDestroy()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
        }

        protected override void OnHpDrown()
        {
            _animator.SetBool(IsAttack, false);
            _animator.Play("Die");
            _playerFoundSubscription.Dispose();
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
            slimeAttackController.Flip(direction == 1);
        }

        protected override void OnPlayerLost()
        {
            GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "어둑시니", (float)hp.Value / maxHp.Value);
            RefreshHp();

            if (!_isAttack)
            {
                StopCoroutine(_aiMoveCoroutine);
                _animator.SetBool(IsAttack, true);
                _animator.Update(0);
                _isAttack = true;
                SetDirection();

                slimeAttackController.SetPosition(lastTargetPlayer.player.transform.position);
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

        public void AttackEffect()
        {
            slimeAttackController.Attack();
        }

        public void AttackEnd()
        {
            _animator.SetBool(IsAttack, false);
            _animator.Update(0);
            _isAttack = false;

            if (isPlayerFounded.Value)
            {
                _animator.SetBool(IsAttack, true);
                _animator.Update(0);
                _isAttack = true;
                SetDirection();
                slimeAttackController.SetPosition(lastTargetPlayer.player.transform.position);
            }
            else
            {
                StartCoroutineWithRunningCheck(ref _aiMoveCoroutine, AIMove(0, 0, 0.3f, 3f));
            }
        }

        #endregion
    }
}