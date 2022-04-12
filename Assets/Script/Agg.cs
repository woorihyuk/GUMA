using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Agg : MonoBehaviour
{
    public float speed;
    public float foundRange;

    int[] attackType;

    int i;
    int attackCount;

    bool isGround;
    bool isMov;
    bool isAttack;

    float gravty;
    float bSpeed;
    float foundTime;
    float walkTime;

    SpriteRenderer sr;
    PlayerCtrl player;
    Animator anim;
    private void Start()
    {
        attackType = new int[] {1,2,1,1,2,1,2,1,1,1,2,2,1,1,1,2 };
        i = 1;
        isGround = false;
        bSpeed = speed;
        isMov = true;
        player = FindObjectOfType<PlayerCtrl>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isAttack = true;
    }

    void Update()
    {
        foundTime += Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.transform.position);
        Vector2 bPos = transform.position;
        Vector2 aPos = new Vector2(speed * i, 0) * Time.deltaTime;
        Vector2 gPos = new Vector2(0, gravty) * Time.deltaTime;
        transform.position = bPos + gPos;
        //못찾을때
        if (foundTime>=3)
        {
            if (i == -1)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }
            transform.position = bPos + aPos;
            anim.SetBool("isWalk", true);
            walkTime += Time.deltaTime;
            if (walkTime>=2)
            {
                i = i * -1;
                anim.SetBool("isWalk", false);
                walkTime = 0;
                foundTime = 0;
            }
        }
        //인식했을떄
        if (dist<=foundRange)
        {
            anim.SetBool("isWalk", false);
            foundTime = 0;
            if (isAttack==true)
            {
                if (attackType[attackCount] == 1)
                {
                    anim.SetBool("attack1R", true);
                    isAttack = false;
                }
                else if (attackType[attackCount] == 2)

                {
                    anim.SetBool("attack2R", true);
                    isAttack = false;
                }
                Debug.Log(attackType[attackCount]);
            } 
        }
        //중력
        if (isGround==false)
        {
            gravty = -3;
        }
        else
        {
            gravty = 0;
        }
        //이동통제
        if (isMov==false)
        {
            speed = 0;
        }
        else
        {
            speed=bSpeed;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="ground")
        {
            
            isGround = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="ground")
        {
            isGround = false;
        }
    }
    public void A1()
    {
        anim.SetBool("attack1R", false);
        anim.SetBool("attack1", true);
    }
    public void A1End()
    {
        anim.SetBool("attack1", false);
        if (attackCount<=14)
        {
            attackCount += 1;
        }
        else
        {
            attackCount = 0;
        }
        isAttack = true;
    }
    public void A2()
    {
        anim.SetBool("attack2R", false);
        anim.SetBool("attack2", true);
    }
    public void A3()
    {
        anim.SetBool("attack2", false);
        anim.SetBool("attack3", true);
    }
    public void A3End()
    {
        anim.SetBool("attack3", false);
        if (attackCount <= 14)
        {
            attackCount += 1;
        }
        else
        {
            attackCount = 0;
        }
        isAttack = true;
    }
}
