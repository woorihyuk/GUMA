using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public GameObject hpUi;

    public float maxSpeed;
    public float jumpPower;
    public float dashPower;
    public float hp;

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

    float h;
    float mxHp;

    int isJump;

    Animator anim;
    Rigidbody2D rigid;
    CapsuleCollider2D box;
    SpriteRenderer spriteRenderer;
    Image hpBar;

    Hit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<CapsuleCollider2D>();
        hpBar = hpUi.GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hit = GetComponent<Hit>();
        bSpeed = maxSpeed;
        mxHp = hp;
        isMove = true;
    }


    void Update()
    {
        hpBar.fillAmount = hp / mxHp;
        dTime += Time.deltaTime;
        //이동
        if (isDash == false)
        {
            h = Input.GetAxisRaw("Horizontal");
            if (isMove)
            {
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
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
            if (isAttack1==false&&isAttack2==false&&isAttack3==false)
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
        //점프
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isJump < 1)
            {
                rigid.velocity = new Vector2(0, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJump", true);
                isJump += 1;
                isGround = false;
                box.enabled = false;
            }
            else if (isJump == 1)
            {
                rigid.velocity = new Vector2(0, 0);
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("dJump", true);
                isJump += 1;
                isGround = false;
                box.enabled = false;
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
            var j = new Vector2(direction*-1, 0);
            rigid.AddForce(j, ForceMode2D.Impulse);
        }
        //땅에 닿았는지
        if (isGround==true)
        {
            if (h != 0)
            {
                anim.SetBool("isRun", true);
                transform.localScale = new Vector3(h, 1, 1);
                direction = h;
            }
            else
            {
                anim.SetBool("isRun", false);
            }
        }
        if (rigid.velocity.y<0)
        {
            box.enabled = true;
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
    //공격끝
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("isJump", false);
            anim.SetBool("dJump", false);
            isGround = true;
            isJump = 0;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.tag=="EnemyAttack")
        {
            if (isHit==false&&!isDash&&!isSteap)
            {
                EnemyAttack enemyAttack = collision.gameObject.GetComponent<EnemyAttack>();
                hp -= enemyAttack.dmg;
                StartCoroutine(hit.HitAni());
            } 
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
