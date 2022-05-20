using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public float jumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;
    public float moveSpeed = 6;
    public float dashSpeed = 18;
    public float defaultSpeed = 6;

    private float _gravity;
    private float _jumpVelocity;
    private Vector3 _velocity;
    private float _velocityXSmoothing;

    private Controller2D _controller;

    public enum JumpMode
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

    private AttackMode _currentAttack;

    private float _sinceLastDashTime = 10f;
    public float dashCoolTime = 0.09f;
    public GameObject[] attackPrefabs;

    public float wallCheckDistance = 0.95f;
    public Transform[] wallRayCheckTfs;

    private bool _isDash, _isAttack, _isWall;
    private float _lastInputX;
    private int _direction = 1;
    private JumpMode _currentJump;
    public Animator animator;
    public SpriteFlipper flipper;
    private static readonly int IsStap = Animator.StringToHash("isStap");
    private static readonly int Isdash = Animator.StringToHash("isdash");
    private static readonly int IsJump = Animator.StringToHash("isJump");
    private static readonly int DJump = Animator.StringToHash("dJump");
    private static readonly int IsRun = Animator.StringToHash("isRun");
    private static readonly int IsAttack = Animator.StringToHash("isAttack");
    private static readonly int IsAttack2 = Animator.StringToHash("isAttack2");
    private static readonly int IsAttack3 = Animator.StringToHash("isAttack3");
    private static readonly int Iswall = Animator.StringToHash("iswall");

    private void Start()
    {
        _controller = GetComponent<Controller2D>();

        defaultSpeed = moveSpeed;
        _lastInputX = 1;

        _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // f의 p승
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        print("Gravity: " + _gravity + " Jump Velocity: " + _jumpVelocity);
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
            animator.SetBool(IsJump, false);
            animator.SetBool(DJump, false);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Jump();

        if (dashCoolTime < _sinceLastDashTime)
        {
            if (input.x == 0)
            {
                BackStep();
            }
            else
            {
                Dash();
            }
        }

        // 벽 확인용
        Debug.DrawRay(wallRayCheckTfs[0].position, wallRayCheckTfs[0].right * wallCheckDistance, Color.red, 0, false);
        Debug.DrawRay(wallRayCheckTfs[1].position, wallRayCheckTfs[1].right * wallCheckDistance, Color.red, 0, false);

        // 월드 레이어만 레이캐스트
        int mask = 1 << LayerMask.NameToLayer("World");

        // 벽 확인 레이캐스트 배열
        var wallCheckRays = new RaycastHit2D[2];
        wallCheckRays[0] =
            Physics2D.Raycast(wallRayCheckTfs[0].position, wallRayCheckTfs[0].right, wallCheckDistance, mask);
        wallCheckRays[1] =
            Physics2D.Raycast(wallRayCheckTfs[1].position, wallRayCheckTfs[1].right, wallCheckDistance, mask);

        // 벽 확인 결과 저장용
        var checkWallResult = new bool[2];
        if (wallCheckRays[0].transform) checkWallResult[0] = true;
        if (wallCheckRays[1].transform) checkWallResult[1] = true;

        // 확인코드
        if (checkWallResult[0] && checkWallResult[1])
        {
            _isWall = true;
            _currentJump = JumpMode.None;
            animator.SetBool(IsJump, false);
            animator.SetBool(DJump, false);
            print("벽 맞음");
        }
        else
        {
            _isWall = false;
        }

        animator.SetBool(Iswall, _isWall);

        float targetVelocityX = input.x * moveSpeed;

        if (_isDash)
        {
            targetVelocityX = _direction * moveSpeed;
        }
        else
        {
            if (!_isAttack && input.x != 0) _lastInputX = input.x;
        }

        Attack();

        if (_isAttack)
        {
            targetVelocityX = 0;
        }

        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
            (_controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime;

        flipper.Flip(_lastInputX);

        if (targetVelocityX < 0 || targetVelocityX > 0)
        {
            animator.SetBool(IsRun, true);
        }
        else if (targetVelocityX == 0)
        {
            animator.SetBool(IsRun, false);
        }

        _controller.Move(_velocity * Time.deltaTime);
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentAttack == AttackMode.Second)
            {
                _isAttack = true;
                Debug.Log("공3");
                _currentAttack = AttackMode.Third;
            }

            else if (_currentAttack == AttackMode.First)
            {
                _isAttack = true;
                Debug.Log("공2");
                _currentAttack = AttackMode.Second;
            }

            else if (_currentAttack == AttackMode.None)
            {
                _isAttack = true;
                Debug.Log("공1");
                _currentAttack = AttackMode.First;
                animator.SetBool(IsAttack, true);
            }
        }
    }

    private void Jump()
    {
        if (_currentJump == JumpMode.None)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _controller.collisions.below)
            {
                _velocity.y = _jumpVelocity;
                _currentJump = JumpMode.Normal;
                animator.SetBool(IsJump, true);
            }
        }
        else if (_currentJump == JumpMode.Normal)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity.y = _jumpVelocity;
                _currentJump = JumpMode.Double;
                animator.SetBool(DJump, true);
            }
        }
    }

    private void BackStep()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetMouseButtonDown(1) && _controller.collisions.below)
            {
                animator.SetBool(IsStap, true);
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
                animator.SetBool(Isdash, true);
                _direction = (int)_lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
            }
        }
    }

    public void OnAnimationDashEnd()
    {
        _sinceLastDashTime = 0;
        animator.SetBool(Isdash, false);
        _isDash = false;
        moveSpeed = defaultSpeed;
        //dTime = 0;
    }

    public void OnAnimationBackStepEnd()
    {
        _sinceLastDashTime = 0;
        animator.SetBool(IsStap, false);
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
            animator.SetBool(IsAttack, false);
            if ((int)_currentAttack >= (int)AttackMode.Second)
            {
                animator.SetBool(IsAttack2, true);
            }
            else
            {
                _isAttack = false;
                _currentAttack = AttackMode.None;
            }
        }

        if (attackMode == AttackMode.Second)
        {
            animator.SetBool(IsAttack2, false);
            if ((int)_currentAttack >= (int)AttackMode.Third)
            {
                animator.SetBool(IsAttack3, true);
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
            animator.SetBool(IsAttack3, false);
            _currentAttack = AttackMode.None;
        }
    }
}