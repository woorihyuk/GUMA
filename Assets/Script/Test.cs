using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed;
    int i;
    bool isGround;
    bool isFight;
    bool isMov;
    bool isAttack;
    float gravty;
    float bSpeed;

    SpriteRenderer sr;
    PlayerCtrl player;
    Animator anim;
    private void Start()
    {
        bSpeed = speed;
        isMov = true;
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerCtrl>();
        anim = GetComponent<Animator>();
        //StartCoroutine(LookPlayer());
        //transform.DOMove(new Vector3(0, transform.position.y), 8).SetEase(Ease.Linear);
    }

    void Update()
    {
        var pos = transform.position.x - player.transform.position.x;
        Debug.Log(pos);
        float dist = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(dist);
        if (isGround==false)
        {
            gravty = -3;
        }
        else
        {
            gravty = 0;
        }
        if (dist<=6)
        {
            isFight = true;
        }

        else
        {
            isFight = false;
        }
        if (dist<=3)
        {
            anim.SetBool("attack1R", true);
            isAttack = true;
        }
        else
        {
            anim.SetBool("attack1R", false);
        }
        if (isFight==true)
        {
            if (isAttack==false)
            {
                if (pos > 0)
                {
                    sr.flipX = false;
                    i = -1;
                }
                else if (pos < 0)
                {
                    sr.flipX = true;
                    i = 1;
                }
            }
        }
        else
        {
            
        }
        
        Vector2 bPos = transform.position;
        Vector2 aPos = new Vector2(speed*i, gravty) * Time.deltaTime;
        if (isMov==false)
        {
            speed = 0;
        }
        else
        {
            speed=bSpeed;
        }
        transform.position = bPos + aPos;
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

    public void Arrack1Ready()
    {
        anim.SetBool("attack1", true);
    }
    public void Attack1And()
    {
        anim.SetBool("attack1", false);
        isAttack = false;
    }
    /*IEnumerator LookPlayer()
    {

        while (true)
        {
            var pos = transform.position.x - player.transform.position.x;
            Debug.Log(pos);
            if (pos > 0)
            {
                if(sr.flipX)
                {
                    sr.flipX = false;
                    yield return new WaitForSeconds(2);
                }
            }
            else if (pos < 0)
            {
                if(!sr.flipX)
                {
                    sr.flipX = true;
                    yield return new WaitForSeconds(2);
                }
            }
            Vector2 bPos = transform.position;
            Vector2 aPos = new Vector2(speed, 0) * i * Time.deltaTime;
            transform.position = bPos + aPos;
            yield return null;
        }
    }*/
}
