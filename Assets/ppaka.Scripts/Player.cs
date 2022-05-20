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

    private float gravity;
    private float jumpVelocity;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;

    public enum JumpMode
    {
        None,
        Normal,
        Double
    }

    public enum AttackMode
    {None,
        First,
        Second,Third
    }

    private AttackMode _currentAttack;

    private float _sinceLastDashTime = 10f;
    public float dashCoolTime = 0.08f;
    public GameObject[] attackPrefabs;
    
    private bool _isDash, _isAttack;
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

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        defaultSpeed = moveSpeed;
        _lastInputX = 1;

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2); // f의 p승
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    private void Update()
    {
        _sinceLastDashTime += Time.deltaTime;
        
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (controller.collisions.below)
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

        float targetVelocityX = input.x * moveSpeed;
        
        if (_isDash)
        {
            targetVelocityX = _direction * moveSpeed;
        }
        else
        {
            if (input.x != 0) _lastInputX = input.x;
        }
        
        Attack();

        if (_isAttack)
        {
            targetVelocityX = 0;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        flipper.Flip(_lastInputX);
        
        if (targetVelocityX < 0 || targetVelocityX > 0)
        {
            animator.SetBool(IsRun, true);
        }
        else if (targetVelocityX == 0)
        {
            animator.SetBool(IsRun, false);
        }
        
        controller.Move(velocity * Time.deltaTime);
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

            if (_currentAttack == AttackMode.None)
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
            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
                _currentJump = JumpMode.Normal;
                animator.SetBool(IsJump, true);
            }
        }
        else if (_currentJump == JumpMode.Normal)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = jumpVelocity;
                _currentJump = JumpMode.Double;
                animator.SetBool(DJump, true);
            }
        }
    }

    private void BackStep()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetMouseButtonDown(1) && controller.collisions.below)
            {
                animator.SetBool(IsStap, true);
                _direction = -(int) _lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
            }
        }
    }

    private void Dash()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetMouseButtonDown(1) && controller.collisions.below)
            {
                animator.SetBool(Isdash, true);
                _direction = (int) _lastInputX;
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
        if (attackMode == AttackMode.First)
        {
            Instantiate(attackPrefabs[0], transform.position, transform.rotation);
        }
        if (attackMode == AttackMode.Second)
        {
            Instantiate(attackPrefabs[1], transform.position, transform.rotation);
        }
        if (attackMode == AttackMode.Third)
        {
            Instantiate(attackPrefabs[2], transform.position, transform.rotation);
        }
    }

    public void OnAnimationAttackEnd(AttackMode attackMode)
    {
        if (attackMode == AttackMode.First)
        {
            animator.SetBool("isAttack", false);
            if (_currentAttack == AttackMode.Second)
            {
                animator.SetBool("isAttack2", true);
            }
            else
            {
                _isAttack = false;
                _currentAttack = AttackMode.None;
            }
        }
        if (attackMode == AttackMode.Second)
        {
            animator.SetBool("isAttack2", false);
            if (_currentAttack == AttackMode.Third)
            {
                animator.SetBool("isAttack3", true);
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
            animator.SetBool("isAttack3", false);
            _currentAttack = AttackMode.None;
        }
    }
}
