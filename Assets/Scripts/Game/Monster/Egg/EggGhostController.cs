using System;
using Game.Monster;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggGhostController : Monster
{
    private bool _isAttack;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _aiMoveCoroutine;
    private IDisposable _moveStateSubscription, _attack2WaitSubscription;
    public float attack2Distance;
    public Transform directionalObjectGroup, smokingPoint;

    #region 애니메이터 해쉬

    private static readonly int IsWalk = Animator.StringToHash("isWalk");
    private static readonly int Attack1 = Animator.StringToHash("attack1");
    private static readonly int Attack2 = Animator.StringToHash("attack2");

    #endregion

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _aiMoveCoroutine = StartCoroutine(AIMove(1, 3, 1f, 2f));
        _moveStateSubscription = isMonsterMoving.DistinctUntilChanged()
            .Subscribe(v => { _animator.SetBool(IsWalk, v); }).AddTo(gameObject);
    }

    private void OnDestroy()
    {
        GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
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
        directionalObjectGroup.localScale = new Vector3(direction, directionalObjectGroup.localScale.y);
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
        GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "달걀귀신", (float)hp.Value / maxHp.Value);
        RefreshHp();

        if (!_isAttack)
        {
            StopCoroutine(_aiMoveCoroutine);
            _animator.SetBool(IsWalk, false);
            NormalAttack();
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

    public void OnAttack1Event()
    {
    }

    public void OnAttack1End()
    {
        _animator.SetBool(Attack1, false);
        _isAttack = false;

        _attack2WaitSubscription = Observable.Timer(TimeSpan.FromMilliseconds(1000))
            .Subscribe(_ =>
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

                    _aiMoveCoroutine = StartCoroutine(AIMove(1, 3, 0.3f, 3f));
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
                        
                    _aiMoveCoroutine = StartCoroutine(AIMove(1, 3, 0.3f, 3f));
                    return;
                }
                    
                StopCoroutine(_aiMoveCoroutine);
                _animator.SetBool(IsWalk, false);
                NormalAttack();
            }).AddTo(gameObject);
    }

    public void OnAttack4Event()
    {
    }

    public void OnAttack4End()
    {
        _isAttack = false;

        _aiMoveCoroutine = StartCoroutine(AIMove(1, 3, 1f, 2f));
        _attack2WaitSubscription = Observable.Timer(TimeSpan.FromMilliseconds(1000)).Subscribe(
            _ =>
            {
                if (!isPlayerFounded.Value) return;
                StopCoroutine(_aiMoveCoroutine);
                _animator.SetBool(IsWalk, false);
                NormalAttack();
            });
        
        
    }

    #endregion
}