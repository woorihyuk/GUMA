using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// ReSharper disable Unity.InefficientPropertyAccess

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public int direction = 1;
    public float jumpHeight = 6;
    public float timeToJumpApex = 0.4f;
    public float moveSpeed = 8;
    public float dashSpeed = 24;
    public float defaultSpeed = 8;
    public float dashCoolTime = 0.09f;
    public float lastInputX;
    public float wallCheckDistance = 0.64f;
    public float maxHp = 100;
    public bool isHit, isCombo;
    public AttackMode currentAttack;
    public GameObject[] attackPrefabs;
    public Transform[] wallRayCheckTfs;
    public AudioClip[] clip;
    public Animator animator;
    public SpriteFlipper flipper;
    public PlayerAttachedCamera playerAttached;
    public SpriteRenderer sr;

    private static readonly int AnimIsBackStep = Animator.StringToHash("isBackStep");
    private static readonly int AnimIsDash = Animator.StringToHash("isDash");
    private static readonly int AnimIsJump = Animator.StringToHash("isJump");
    private static readonly int AnimDoubleJump = Animator.StringToHash("dJump");
    private static readonly int AnimIsRun = Animator.StringToHash("isRun");
    private static readonly int AnimIsAttack = Animator.StringToHash("isAttack");
    private static readonly int AnimIsAttack2 = Animator.StringToHash("isAttack2");
    private static readonly int AnimIsAttack3 = Animator.StringToHash("isAttack3");
    private static readonly int AnimIsWall = Animator.StringToHash("isWall");
    private static readonly int AnimIsDie = Animator.StringToHash("isDie");
    private static readonly int AnimIsShoot = Animator.StringToHash("isShoot");
    private static readonly int AnimIsShoot2 = Animator.StringToHash("isShoot2");

    private AudioSource _audio;
    private int _attackMode;
    private const float AccelerationTimeAirborne = 0.2f;
    private const float AccelerationTimeGrounded = 0.1f;
    private float _gravity;
    private float _jumpVelocity;
    private float _velocityXSmoothing;
    private float _sinceLastDashTime = 10f;
    private float _comboTime;
    public float hp;
    private bool _isDash, _isAttack, _isWall, _isTalk, _isTalking, _isSave, _isDoor, _isDoAttack, _isAttackYet;
    private Vector2 _input;
    private Vector3 _velocity;
    private Vector3 _doorPos;
    private JumpMode _currentJump;
    private Hit _hit;
    private Controller2D _controller;
    public Image hpBar;
    private Sign _sign;
    private Doer _door;
    private GameUIManager _gameUIManager;

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
        Third,
        FirstShoot,
        SecondShoot
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _gameUIManager = FindObjectOfType<GameUIManager>();

        _isAttackYet = true;
        _controller = GetComponent<Controller2D>();

        defaultSpeed = moveSpeed;
        lastInputX = 1;

        _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        // print("Gravity: " + _gravity + " Jump Velocity: " + _jumpVelocity);

        hp = maxHp;

        _hit = GetComponent<Hit>();
        isHit = true;
        
        switch (GameManager.Instance.savePoint)
        {
            case 0:
                transform.position = new Vector3(0.5f, -6.07f, 0);
                Time.timeScale = 1;
                break;
            case 1:
                transform.position = new Vector3(89.41f, 2.95f, 0);
                Time.timeScale = 1;
                break;
        }

        playerAttached.isIn = true;
        _gameUIManager.gameOverImage.gameObject.SetActive(false);
        _gameUIManager.keyHintE.gameObject.SetActive(false);
        _audio = GetComponent<AudioSource>();
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
            animator.SetBool(AnimDoubleJump, false);
        }

        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            _isAttackYet = true;
            _isDoAttack = false;
            _isAttack = false;
            animator.SetBool(AnimIsAttack, false);
            animator.SetBool(AnimIsShoot, false);
            animator.SetBool(AnimIsAttack, false);
            animator.SetBool(AnimIsAttack2, false);
            animator.SetBool(AnimIsAttack3, false);
            animator.SetBool(AnimIsShoot, false);
            animator.SetBool(AnimIsShoot2, false);
            currentAttack = AttackMode.None;
        }

        if (!_isDash)
        {
            WallCheck();
            if (!_isAttack)
            {
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
        }

        float targetVelocityX = _input.x * moveSpeed;

        if (_isDash)
        {
            targetVelocityX = direction * moveSpeed;
        }
        else
        {
            if (!_isAttack && _input.x != 0) lastInputX = _input.x;
        }

        Jump();

        var gravityMultiplier = 1f;

        if (!_isWall) Attack();
        else gravityMultiplier = 0.4f;

        if (_isAttack) targetVelocityX = 0;

        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
            _controller.collisions.below ? AccelerationTimeGrounded : AccelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime * gravityMultiplier;

        flipper.Flip(lastInputX);

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_isTalk)
            {
                if (!_isTalking)
                {
                    // Debug.Log(_isTalking);
                    _isTalking = true;
                    _gameUIManager.keyHintE.gameObject.SetActive(false);
                    _sign.TalkStart();
                }
                else
                {
                    _sign.NextText();
                }
            }

            if (_isSave)
            {
                GameManager.Instance.savePoint = 1;
                GameManager.Instance.GameSave();
                StartCoroutine(_hit.Save());
            }

            if (_isDoor)
            {
                transform.position = _doorPos;
                _isDoor = false;
                playerAttached.isIn = !playerAttached.isIn;

                _gameUIManager.keyHintE.gameObject.SetActive(false);
                _door = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SceneManager.LoadScene("Title");
        }

        if (isCombo)
        {
            if (!_isAttack)
            {
                if (_isDoAttack)
                {
                    DoAttack();
                }
            }
        }
    }

    private void WallCheck()
    {
        // 체크 용도
        Debug.DrawRay(wallRayCheckTfs[0].position, Vector3.right * (lastInputX * wallCheckDistance), Color.red, 0,
            false);
        Debug.DrawRay(wallRayCheckTfs[1].position, Vector3.right * (lastInputX * wallCheckDistance), Color.red, 0,
            false);

        // 레이어 마스크
        var mask = 1 << LayerMask.NameToLayer("WorldGround");

        // 위 아레로 플레이어 보는 방향으로 레이 쏘기
        var wallCheckRays = new RaycastHit2D[2];
        wallCheckRays[0] =
            Physics2D.Raycast(wallRayCheckTfs[0].position, Vector3.right * (lastInputX * wallCheckDistance),
                wallCheckDistance, mask);
        wallCheckRays[1] =
            Physics2D.Raycast(wallRayCheckTfs[1].position, Vector3.right * (lastInputX * wallCheckDistance),
                wallCheckDistance, mask);

        // 값 확인
        var checkWallResult = new bool[2];
        if (wallCheckRays[0].transform) checkWallResult[0] = true;
        if (wallCheckRays[1].transform) checkWallResult[1] = true;

        // 벽인가?
        if (checkWallResult[0] && checkWallResult[1])
        {
            if (!_isWall)
            {
                _isWall = true;
                _isDash = false;
                _currentJump = JumpMode.None;
                animator.SetBool(AnimIsJump, false);
                animator.SetBool(AnimDoubleJump, false);

                _velocity.y = 0;
            }
        }
        else
        {
            _isWall = false;
        }

        animator.SetBool(AnimIsWall, _isWall);
    }

    private void DoAttack()
    {
        switch ((int)currentAttack)
        {
            case 0:
                break;
            case 1:
                _isAttack = true;
                animator.SetBool(AnimIsAttack, true);
                break;
            case 2:
                _isAttack = true;
                animator.SetBool(AnimIsAttack2, true);
                break;
            case 3:
                _isAttack = true;
                animator.SetBool(AnimIsAttack3, true);
                break;
            case 4:
                _isAttack = true;
                animator.SetBool(AnimIsShoot, true);
                break;
            case 5:
                _isAttack = true;
                animator.SetBool(AnimIsShoot2, true);
                break;
        }
    }

    private void Attack()
    {
        if (_isAttackYet)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAttack == AttackMode.Second || currentAttack == AttackMode.SecondShoot)
                {
                    currentAttack = AttackMode.Third;
                    _isAttackYet = false;
                }

                else if (currentAttack == AttackMode.First || currentAttack == AttackMode.FirstShoot)
                {
                    currentAttack = AttackMode.Second;
                    _isAttackYet = false;
                }
                else if (currentAttack == AttackMode.None)
                {
                    _isAttack = true;
                    animator.SetBool(AnimIsAttack, true);
                    currentAttack = AttackMode.First;
                }

                _isDoAttack = true;
            }

            else if (Input.GetMouseButtonDown(1))
            {
                if (currentAttack == AttackMode.None)
                {
                    _isAttack = true;
                    animator.SetBool(AnimIsShoot, true);
                    currentAttack = AttackMode.FirstShoot;
                }

                else if (currentAttack == AttackMode.FirstShoot || currentAttack == AttackMode.First)
                {
                    currentAttack = AttackMode.SecondShoot;
                    _isAttackYet = false;
                }

                _isDoAttack = true;
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
                    _audio.clip = clip[0];
                    _audio.Play();
                    switch (_isWall)
                    {
                        case true when _controller.collisions.below:
                            _velocity.y = _jumpVelocity;
                            _velocity.x = -20 * lastInputX;
                            _currentJump = JumpMode.Normal;
                            animator.SetBool(AnimIsJump, true);
                            lastInputX *= -1;
                            break;
                        case false when _controller.collisions.below:
                            _velocity.y = _jumpVelocity;
                            _currentJump = JumpMode.Normal;
                            animator.SetBool(AnimIsJump, true);
                            break;
                        case false when !_controller.collisions.below:
                            _velocity.y = _jumpVelocity;
                            _currentJump = JumpMode.Double;
                            animator.SetBool(AnimDoubleJump, true);
                            break;
                        case true when !_controller.collisions.below:
                            _velocity.y = _jumpVelocity;
                            _velocity.x = -20 * lastInputX;
                            _currentJump = JumpMode.Normal;
                            animator.SetBool(AnimIsJump, true);
                            lastInputX *= -1;
                            break;
                    }

                    break;
                }
                case JumpMode.Normal:
                {
                    _velocity.y = _jumpVelocity;
                    _currentJump = JumpMode.Double;
                    animator.SetBool(AnimDoubleJump, true);
                    break;
                }
            }

            _isAttackYet = true;
            _isDoAttack = false;
            _isAttack = false;
            animator.SetBool(AnimIsAttack, false);
            animator.SetBool(AnimIsShoot, false);
            animator.SetBool(AnimIsAttack, false);
            animator.SetBool(AnimIsAttack2, false);
            animator.SetBool(AnimIsAttack3, false);
            animator.SetBool(AnimIsShoot, false);
            animator.SetBool(AnimIsShoot2, false);
            currentAttack = AttackMode.None;
        }
    }

    private void BackStep()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _controller.collisions.below)
            {
                _audio.PlayOneShot(clip[3]);
                animator.SetBool(AnimIsBackStep, true);
                direction = -(int)lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
                isHit = false;
            }
        }
    }

    private void Dash()
    {
        if (_currentJump == JumpMode.None && !_isDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _controller.collisions.below)
            {
                _audio.PlayOneShot(clip[3]);
                animator.SetBool(AnimIsDash, true);
                direction = (int)lastInputX;
                moveSpeed = dashSpeed;
                _isDash = true;
                isHit = false;
            }
        }
    }

    public void OnAnimationDashEnd()
    {
        _sinceLastDashTime = 0;
        animator.SetBool(AnimIsDash, false);
        _isDash = false;
        isHit = true;
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

        switch (attackMode)
        {
            case AttackMode.First:
                _audio.PlayOneShot(clip[1]);
                Instantiate(attackPrefabs[0], thisTransform.position,
                    !sr.flipX ? thisTransform.rotation : Quaternion.Euler(0, -180, 0));
                break;
            case AttackMode.Second:
                _audio.PlayOneShot(clip[1]);
                Instantiate(attackPrefabs[1], thisTransform.position,
                    !sr.flipX ? thisTransform.rotation : Quaternion.Euler(0, -180, 0));
                break;
            case AttackMode.Third:
                _audio.PlayOneShot(clip[1]);
                Instantiate(attackPrefabs[2], thisTransform.position,
                    !sr.flipX ? thisTransform.rotation : Quaternion.Euler(0, -180, 0));
                break;
            case AttackMode.FirstShoot:
            case AttackMode.SecondShoot:
                _audio.PlayOneShot(clip[2]);
                Instantiate(attackPrefabs[3], thisTransform.position,
                    !sr.flipX ? thisTransform.rotation : Quaternion.Euler(0, -180, 0));
                break;
        }
    }

    public void OnAnimationAttackEnd(AttackMode attackMode)
    {
        _isAttackYet = true;
        if (attackMode == AttackMode.First)
        {
            animator.SetBool(AnimIsAttack, false);
            _isAttack = false;
        }

        if (attackMode == AttackMode.Second)
        {
            animator.SetBool(AnimIsAttack2, false);
            _isAttack = false;
        }

        if (attackMode == AttackMode.Third)
        {
            _isAttack = false;
            animator.SetBool(AnimIsAttack3, false);
            currentAttack = AttackMode.None;
        }

        if (attackMode == AttackMode.FirstShoot)
        {
            _isAttack = false;
            animator.SetBool(AnimIsShoot, false);
        }

        if (attackMode == AttackMode.SecondShoot)
        {
            _isAttack = false;
            animator.SetBool(AnimIsShoot2, false);
        }

        if (currentAttack == attackMode)
        {
            _isDoAttack = false;
            Debug.Log(currentAttack);
        }

        isCombo = true;
        StartCoroutine(_hit.AttackWait(attackMode));
    }

    public void IsDie()
    {
        GameManager.Instance.GameLoad();
        Time.timeScale = 0;
        _gameUIManager.gameOverImage.gameObject.SetActive(true);
        GameEvents.OnPlayerDie.Invoke();
        transform.position = GameManager.Instance.savePoint switch
        {
            0 => new Vector3(0, -1.5f, 0),
            1 => new Vector3(86, 8.25f, 0),
            _ => transform.position
        };
    }

    public void Restart()
    {
        hp = maxHp;
        hpBar.fillAmount = hp / maxHp;
        _gameUIManager.gameOverImage.gameObject.SetActive(false);
        animator.SetBool(AnimIsDie, false);
        Time.timeScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            Damage damage = collision.GetComponent<Damage>();
            if (!isHit)
            {
                return;
            }

            hp -= damage.dmg;
            hpBar.fillAmount = hp / maxHp;
            if (hp <= 0)
            {
                animator.SetBool(AnimIsDie, true);
            }

            StartCoroutine(_hit.HitAni());
        }

        else if (collision.CompareTag("Sine"))
        {
            _isTalk = true;
            _gameUIManager.keyHintE.gameObject.SetActive(true);
            _sign = collision.GetComponent<Sign>();
        }

        else if (collision.CompareTag("SavePoint"))
        {
            _isSave = true;
            _gameUIManager.keyHintE.gameObject.SetActive(true);
        }

        else if (collision.CompareTag("Door"))
        {
            _door = collision.GetComponent<Doer>();
            _doorPos = _door.monePosition;
            _isDoor = true;
            _gameUIManager.keyHintE.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sine"))
        {
            _isTalk = false;
            _isTalking = false;
            _gameUIManager.keyHintE.gameObject.SetActive(false);
            _sign.TextEnd();
        }
        else if (other.CompareTag("SavePoint"))
        {
            _isSave = false;
            _gameUIManager.keyHintE.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Door"))
        {
            _isDoor = false;
            _gameUIManager.keyHintE.gameObject.SetActive(false);
            _door = null;
        }
    }
}