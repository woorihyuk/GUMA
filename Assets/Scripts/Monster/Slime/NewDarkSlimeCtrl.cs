using Game.Monster.Slime;
using UnityEngine;

public class NewDarkSlimeCtrl : MonsterMove
{
    public SlimeAttackController slimeAttackController;
    
    private bool _isAttack;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private Coroutine _aiMoveCoroutine;
    private static readonly int IsDie = Animator.StringToHash("isDie");

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _aiMoveCoroutine = StartCoroutine(base.AIMove(0, 0.3f, 3f));
        slimeAttackController.Initialize();
    }

    protected override void Update()
    {
        if (_isAttack)
        {
            StopCoroutine(_aiMoveCoroutine);
            return;
        }
        base.Update();
    }
    
    private void OnDestroy()
    {
        GameUIManager.Instance.TryPopHpBar(GetInstanceID().ToString());
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
        _aiMoveCoroutine = StartCoroutine(AIMove(0, 0.3f, 3f));
    }

    protected override void OnHpDrown()
    {
        _animator.SetBool(IsAttack, false);
        _animator.SetBool(IsDie, true);
        _animator.Update(0);
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }

    protected override void OnDirectionSet(int direction)
    {
        _spriteRenderer.flipX = direction == 1;
        slimeAttackController.Flip(direction == 1);
    }

    protected override void OnPlayerFound(Player player)
    {
        GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "어둑시니", hp);
        RefreshHp(hp);
        
        if (!_isAttack)
        {
            StopCoroutine(_aiMoveCoroutine);
            _animator.SetBool(IsAttack, true);
            _animator.Update(0);
            _isAttack = true;
            if (transform.position.x - player.transform.position.x > 0) OnDirectionSet(-1);
            else if (transform.position.x - player.transform.position.x < 0) OnDirectionSet(1);
            slimeAttackController.SetPosition(player.transform.position);
        }
    }
}
