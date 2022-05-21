using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public int _direction = 1;
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 6;
    public float dashSpeed = 18;
    public float defaultSpeed = 6;
    public float dashCoolTime = 0.09f;
    public float _lastInputX;
    public float wallCheckDistance = 0.64f;
    public float maxHP;
    public bool isHit;
    public GameObject E;
    public GameObject textBox;
    public GameObject[] attackPrefabs;
    public Transform[] wallRayCheckTfs;
    public Animator animator;
    public SpriteFlipper flipper;

    private static readonly int AnimIsBackStep = Animator.StringToHash("isBackStep");
    private static readonly int AnimIsDash = Animator.StringToHash("isDash");
    private static readonly int AnimIsJump = Animator.StringToHash("isJump");
    private static readonly int AnimDJump = Animator.StringToHash("dJump");
    private static readonly int AnimIsRun = Animator.StringToHash("isRun");
    private static readonly int AnimIsAttack = Animator.StringToHash("isAttack");
    private static readonly int AnimIsAttack2 = Animator.StringToHash("isAttack2");
    private static readonly int AnimIsAttack3 = Animator.StringToHash("isAttack3");
    private static readonly int AnimIsWall = Animator.StringToHash("isWall");
    private static readonly int AnimIsDiy = Animator.StringToHash("isDie");
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;
    private float _gravity;
    private float _jumpVelocity;
    private float _velocityXSmoothing;
    private float _sinceLastDashTime = 10f;
    public float _HP;
    private bool _isDash, _isAttack, _isWall, _isTalk;
    private Vector2 _input;
    private Vector3 _velocity;
    private AttackMode _currentAttack;
    private JumpMode _currentJump;
    private Hit _hit;
    private Controller2D _controller;
    private GameObject hpBar;
    private Image _hpBar;
    private Sine sine;

    private enum JumpMode
    {
        None,
        Normal,
        Double
    }

    public enum AttackMode
    {
        None,
        First,
        Second,
        Third
    }

    

    private void Start()
    {
       
        Application.targetFrameRate = 60;
        _controller = GetComponent<Controller2D>();

        defaultSpeed = moveSpeed;
        _lastInputX = 1;

        _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // f??p??
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        print("Gravity: " + _gravity + " Jump Velocity: " + _jumpVelocity);


        hpBar = GameObject.Find("PlayerHp");
        _hpBar = hpBar.GetComponent<Image>();
        _HP = maxHP;
        
        _hit = GetComponent<Hit>();
        isHit = true;

        E.SetActive(false);
        textBox.SetActive(false);
    }

    private void Update()
    {
        _sinceLastDashTime += Time.deltaTime;

        if (_controller.collisions.above || _controller.collisions.below)
        {
            _velocity.y = 0;
        }

        if (_controller.collisions.below)
        {
            _currentJump = JumpMode.None;
            animator.SetBool(AnimIsJump, false);
            animator.SetBool(AnimDJump, false);
        }

        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!_isDash)
        {
            WallCheck();

            if (!_isWall)
            {
                if (dashCoolTime < _sinceLastDashTime)
                {
                    if (_input.x == 0)
                    {
                        BackStep();
                    }
                    else
                    {
                        Dash();
                    }
                }
            }
        }

        float targetVelocityX = _input.x * moveSpeed;

        if (_isDash)
        {
            targetVelocityX = _direction * moveSpeed;
        }
        else
        {
            if (!_isAttack && _input.x != 0) _lastInputX = _input.x;
        }

        if (!_isAttack) Jump();

        var gravityMultiplier = 1f;

        if (!_isWall) Attack();
        else gravityMultiplier = 0.4f;

        if (_isAttack) targetVelocityX = 0;

        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
            _controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime * gravityMultiplier;

        flipper.Flip(_lastInputX);

        if (!_isDash)
        {
            if (targetVelocityX < 0 || targetVelocityX > 0)
            {
                animator.SetBool(AnimIsRun, true);

            }
            else if (targetVelocityX == 0)
            {
                animator.SetBool(AnimIsRun, false);
            }
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void WallCheck()
    {
        // 甕??類ㅼ뵥??
        Debug.DrawRay(wallRayCheckTfs[0].position, Vector3.right * (_lastInputX * wallCheckDistance), Color.red, 0,
            false);
        Debug.DrawRay(wallRayCheckTfs[1].position, Vector3.right * (_lastInputX * wallCheckDistance), Color.red, 0,
            false);

        // ?遺얜굡 ??됱뵠??彛???됱뵠筌?Ŋ???
        var mask = 1 << LayerMask.NameToLayer("WorldGround");

        // 甕??類ㅼ뵥 ??됱뵠筌?Ŋ???獄쏄퀣肉?
        var wallCheckRays = new RaycastHit2D[2];
        wallCheckRays[0] =
            Physics2D.Raycast(wallRayCheckTfs[0].position, Vector3.right * (_lastInputX * wallCheckDistance),
                wallCheckDistance, mask);
        wallCheckRays[1] =
            Physics2D.Raycast(wallRayCheckTfs[1].position, Vector3.right * (_lastInputX * wallCheckDistance),
                wallCheckDistance, mask);

        // 甕??類ㅼ뵥 野껉퀗?????關??
        var checkWallResult = new bool[2];
        if (wallCheckRays[0].transform) checkWallResult[0] = true;
        if (wallCheckRays[1].transform) checkWallResult[1] = true;

        // ?類ㅼ뵥?꾨뗀諭?
        if (checkWallResult[0] && checkWallResult[1])
        {
            if (!_isWall)
            {
                _isWall = true;
                _isDash = false;
                _currentJump = JumpMode.None;
                animator.SetBool(AnimIsJump, false);
                animator.SetBool(AnimDJump, false);

                _velocity.y = 0;
            }
        }
        else
        {
            _isWall = false;
        }

        animator.SetBool(AnimIsWall, _isWall);
        if (_isTalk)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sine.NextText();
            }
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentAttack == AttackMode.Second)
            {
                _isAttack = true;
                _currentAttack = AttackMode.Third;
            }

            else if (_currentAttack == AttackMode.First)
            {
                _isAttack = true;
                _currentAttack = AttackMode.Second;
            }

            else if (_currentAttack == AttackMode.None)
            {
                _isAttack = true;
                _currentAttack = AttackMode.First;
                animator.SetBool(AnimIsAttack, true);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_currentJump)
            {
                case JumpMode.None:
                {
                    if (_isWall && _controller.collisions.below)
                    {
                        _velocity.y = _jumpVelocity;
                        _velocity.x = -20 * _lastInputX;
                        _currentJump = JumpMode.Normal;
                        animator.SetBool(AnimIsJump, true);
                        _lastInputX *= -1;
                    }
                    else if (_controller.collisions.below)
                    {
                        _velocity.y = _jumpVelocity;
                        _currentJump = JumpMode.Normal;
                        animator.SetBool(AnimIsJump, true);
                    }
                    else if (!_controller.collisions.below)
                    {
                        _velocity.y = _jumpVelocity;
                        _currentJump = JumpMode.Double;
                        animator.SetBool(AnimDJump, true);
                    }
                    else if (_isWall)
                    {
                        _velocity.y = _jumpVelocity;
                        _velocity.x = -20 * _lastInputX;
                        _currentJump = JumpMode.Normal;
                        animator.SetBool(AnimIsJump, true);
                        _lastInputX *= -1;
                    }
                    break;
                }
                case JumpMode.Normal:
                {
                    _velocity.y = _jumpVelocity;
                    _currentJump = JumpMode.Double;
                    animator.SetBool(AnimDJump, true);
                    break;
                }
            }
        }
    }

    private void BackStep()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetMouseButtonDown(1) && _controller.collisions.below)
            {
                animator.SetBool(AnimIsBackStep, true);
                _direction = -(int)_lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
            }
        }
    }

    private void Dash()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetMouseButtonDown(1) && _controller.collisions.below)
            {
                animator.SetBool(AnimIsDash, true);
                _direction = (int)_lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
            }
        }
    }

    public void OnAnimationDashEnd()
    {
        _sinceLastDashTime = 0;
        animator.SetBool(AnimIsDash, false);
        _isDash = false;
        moveSpeed = defaultSpeed;
        //dTime = 0;
    }

    public void OnAnimationBackStepEnd()
    {
        _sinceLastDashTime = 0;
        animator.SetBool(AnimIsBackStep, false);
        _isDash = false;
        moveSpeed = defaultSpeed;
        //dTime = 0;
    }

    public void OnAnimationAttackFx(AttackMode attackMode)
    {
        var thisTransform = transform;

        if (attackMode == AttackMode.First)
        {
            Instantiate(attackPrefabs[0], thisTransform.position, thisTransform.rotation);
        }

        if (attackMode == AttackMode.Second)
        {
            Instantiate(attackPrefabs[1], thisTransform.position, thisTransform.rotation);
        }

        if (attackMode == AttackMode.Third)
        {
            Instantiate(attackPrefabs[2], thisTransform.position, thisTransform.rotation);
        }
    }

    public void OnAnimationAttackEnd(AttackMode attackMode)
    {
        if (attackMode == AttackMode.First)
        {
            animator.SetBool(AnimIsAttack, false);
            if ((int)_currentAttack >= (int)AttackMode.Second)
            {
                animator.SetBool(AnimIsAttack2, true);
            }
            else
            {
                _isAttack = false;
                _currentAttack = AttackMode.None;
            }
        }

        if (attackMode == AttackMode.Second)
        {
            animator.SetBool(AnimIsAttack2, false);
            if ((int)_currentAttack >= (int)AttackMode.Third)
            {
                animator.SetBool(AnimIsAttack3, true);
            }
            else
            {
                _isAttack = false;
                _currentAttack = AttackMode.None;
            }
        }

        if (attackMode == AttackMode.Third)
        {
            _isAttack = false;
            animator.SetBool(AnimIsAttack3, false);
            _currentAttack = AttackMode.None;
        }
    }

    public void IsDie()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Damage damage = collision.GetComponent<Damage>();
            if (isHit)
            {
                _HP -= damage.dmg;
                _hpBar.fillAmount = _HP / maxHP;
                if (_HP<=0)
                {
                    animator.SetBool(AnimIsDiy, true);
                }
                StartCoroutine(_hit.HitAni());
            }
        }
        if (collision.CompareTag("Sine"))
        {
            _isTalk = true;
            E.SetActive(true);
            sine = collision.GetComponent<Sine>();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sine"))
        {
            _isTalk = false;
            E.SetActive(false);
        }
    }

}