using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject hpUi;
    public GameObject attack1;
    public GameObject attack2;
    public GameObject attack3;
    public GameObject save;
    public Transform[] wallRayCheckTfs;

    public float maxSpeed;
    public float jumpPower;
    public float dashPower;
    public float hp;
    public float moving;
    public float wallCheckDistance = 0.95f;
    public float slidingSpeed;

    public bool isHit;

    float direction;
    float bSpeed;
    float dTime;

    bool isMove;
    bool isDash;
    bool isSteap;
    bool isGround;
    bool isAttack1;
    bool isAttack2;
    bool isAttack3;
    bool isWall;
    bool isSave;

    float h;
    float mxHp;

    int isJump;

    GameManager gameManager;
    Animator anim;
    Rigidbody2D rigid;
    Image hpBar;

    Hit hit;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        hpBar = hpUi.GetComponent<Image>();
        hit = GetComponent<Hit>();
        bSpeed = maxSpeed;
        mxHp = hp;
        isMove = true;
        save.SetActive(false);
    }


    void Update()
    {
        if (isSave)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.GameSave();
            }
        }
        
        hpBar.fillAmount = hp / mxHp;
        dTime += Time.deltaTime;
        if (hp <= 0)
        {
            anim.SetBool("isDie", true);
        }

        //이동
        if (isDash == false)
        {
            h = Input.GetAxisRaw("Horizontal");
            if (isMove)
            {
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
                if (h == 1)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (h == -1)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }

        //속도제한
        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        //공격
        if (Input.GetMouseButtonDown(0))
        {
            if (isAttack2)
            {
                isMove = false;
                Debug.Log("공3");
                isAttack3 = true;
            }

            if (isAttack1)
            {
                isMove = false;
                Debug.Log("공2");
                isAttack2 = true;
            }

            if (isAttack1 == false && isAttack2 == false && isAttack3 == false)
            {
                isMove = false;
                isAttack1 = true;
                anim.SetBool("isAttack", true);
            }
        }

        //멈춤
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }
        
        // 벽 확인용
        Debug.DrawRay(wallRayCheckTfs[0].position, wallRayCheckTfs[0].right * wallCheckDistance, Color.red, 0, false);
        Debug.DrawRay(wallRayCheckTfs[1].position, wallRayCheckTfs[1].right * wallCheckDistance, Color.red, 0, false);

        // 월드 레이어만 레이캐스트
        int mask = 1 << LayerMask.NameToLayer("World");

        // 벽 확인 레이캐스트 배열
        var wallCheckRays = new RaycastHit2D[2];
        wallCheckRays[0] = Physics2D.Raycast(wallRayCheckTfs[0].position, wallRayCheckTfs[0].right, wallCheckDistance, mask);
        wallCheckRays[1] = Physics2D.Raycast(wallRayCheckTfs[1].position, wallRayCheckTfs[1].right, wallCheckDistance, mask);

        // 벽 확인 결과 저장용
        var checkWallResult = new bool[2];
        if (wallCheckRays[0].transform) checkWallResult[0] = true;
        if (wallCheckRays[1].transform) checkWallResult[1] = true;

        // 확인코드
        if (checkWallResult[0] && checkWallResult[1])
        {
            isWall = true;
            isJump = 0;
            anim.SetBool("isJump", false);
            anim.SetBool("dJump", false);
            print("벽 맞음");
        }
        else
        {
            isWall = false;
        }
        
        anim.SetBool("iswall", isWall);
        
        if (isWall) rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * slidingSpeed);
        
        //점프
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (isJump)
            {
                case 0:
                    rigid.velocity = new Vector2(0, 0);
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("isJump", true);
                    isJump += 1;
                    break;
                case 1:
                    rigid.velocity = new Vector2(0, 0);
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    anim.SetBool("dJump", true);
                    isJump += 1;
                    isGround = false;
                    break;
            }
        }

        //구르기,백스텝
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isMove)
            {
                if (dTime >= 0.1f)
                {
                    if (isGround == true)
                    {
                        if (h != 0)
                        {
                            dTime = 0;
                            isDash = true;
                            maxSpeed = maxSpeed + dashPower;
                            anim.SetBool("isdash", true);
                        }
                        else
                        {
                            dTime = 0;
                            isSteap = true;
                            maxSpeed = maxSpeed + dashPower;
                            anim.SetBool("isStap", true);
                        }
                    }
                }
            }
        }

        //구르기
        if (isDash == true)
        {
            var i = new Vector2(direction, 0);
            rigid.AddForce(i, ForceMode2D.Impulse);
        }

        //스텝
        if (isSteap == true)
        {
            var j = new Vector2(direction * -1, 0);
            rigid.AddForce(j, ForceMode2D.Impulse);
        }

        //땅에 닿았는지
        if (isGround == true)
        {
            if (h != 0)
            {
                anim.SetBool("isRun", true);
                direction = h;
            }
            else
            {
                anim.SetBool("isRun", false);
            }
        }
    }

    //구르기 끝
    public void DashEnd()
    {
        maxSpeed = bSpeed;
        isDash = false;
        anim.SetBool("isdash", false);
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        dTime = 0;
    }

    //스텝 끝
    public void SteapEnd()
    {
        maxSpeed = bSpeed;
        isSteap = false;
        anim.SetBool("isStap", false);
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        dTime = 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            anim.SetBool("isJump", false);
            anim.SetBool("dJump", false);
            isGround = true;
            isJump = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isJump = 1;
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            if (isHit == false && !isDash && !isSteap)
            {
                Damage damage = collision.gameObject.GetComponent<Damage>();
                hp -= damage.dmg;
                StartCoroutine(hit.HitAni());
            }
        }

        if (collision.name == "Jangsung")
        {
            save.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Jangsung")
        {
            save.SetActive(false);
        }
    }


    public void Attack1End()
    {
        isAttack1 = false;
        anim.SetBool("isAttack", false);
        if (isAttack2)
        {
            anim.SetBool("isAttack2", true);
        }
        else if (!isAttack2)
        {
            isMove = true;
        }
    }

    public void Attack1Fx()
    {
        Instantiate(attack1, transform.position, transform.rotation);
    }

    public void Attack2Fx()
    {
        Instantiate(attack2, transform.position, transform.rotation);
    }

    public void Attack3Fx()
    {
        Instantiate(attack3, transform.position, transform.rotation);
    }

    public void Attack2End()
    {
        anim.SetBool("isAttack2", false);
        isAttack2 = false;
        if (isAttack3)
        {
            anim.SetBool("isAttack3", true);
        }
        else if (!isAttack3)
        {
            isMove = true;
        }
    }

    public void Attack3End()
    {
        isAttack3 = false;
        anim.SetBool("isAttack3", false);
        isMove = true;
    }
}