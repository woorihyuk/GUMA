using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    public float maxSpeed;
    public float jumpPower;
    public float dashPower;

    float direction;
    bool isGround;
    float bSpeed;
    float dTime;
    bool isDash;
    float h;

    Animator anim;
    Rigidbody2D rigid;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        bSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(direction);

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
            if (isGround == true)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                isGround = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dTime >= 0.3f)
            {
                dTime = 0;
                isDash = true;
                maxSpeed = maxSpeed + 2;
                anim.SetBool("isdash", true);

            }
        }

        if (isDash == true)
        {
            var i = new Vector2(direction, 0);
            rigid.AddForce(i, ForceMode2D.Impulse);
        }

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
    public void e()
    {
        maxSpeed = bSpeed;
        isDash = false;
        anim.SetBool("isdash", false);
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        dTime = 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGround = true;
        }
    }
}
