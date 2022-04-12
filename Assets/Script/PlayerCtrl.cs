using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float dashPower;

    float direction;
    float bSpeed;
    float dTime;


    bool isDash;
    bool isSteap;
    bool isGround;
    bool isRjump;

    float h;
    int isJump;

    Animator anim;
    Rigidbody2D rigid;
    CapsuleCollider2D box;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<CapsuleCollider2D>();
        bSpeed = maxSpeed;

    }

    // Update is called once per frame
    void Update()
    {

        dTime += Time.deltaTime;

        if (isDash == false)
        {
            h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        }

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }



        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

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
        if (Input.GetKeyDown(KeyCode.Space))
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

        if (isDash == true)
        {
            var i = new Vector2(direction, 0);
            rigid.AddForce(i, ForceMode2D.Impulse);
        }
        if (isSteap == true)
        {
            var j = new Vector2(direction*-1, 0);
            rigid.AddForce(j, ForceMode2D.Impulse);
        }
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
    public void e()
    {
        maxSpeed = bSpeed;
        isDash = false;
        anim.SetBool("isdash", false);
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        dTime = 0;
    }

    public void q()
    {
        maxSpeed = bSpeed;
        isSteap = false;
        anim.SetBool("isStap", false);
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        dTime = 0;
    }

    public void r()
    {
        rigid.velocity = new Vector2(0, 0);
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        anim.SetBool("dJump", true);
        isJump += 1;
        isGround = false;
    }
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
}
