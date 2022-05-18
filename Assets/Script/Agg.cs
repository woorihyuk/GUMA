using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Agg : MonoBehaviour
{
    public GameObject prefab;
    public GameObject shootPoint1;
    public GameObject shootPoint2;
    public GameObject hpBar;
    public GameObject[] attacks;

    public float speed;
    public float foundRange;
    public float hp;
    public float attackRange;
    public float jumpPower;

    int[] attackType;

    int i;
    int attackCount;
    int ifSmoking;

    public bool isGround;
    bool isMov;
    bool isAttack;
    bool isFound;

    float gravty;
    float bSpeed;
    float foundTime;
    float walkTime;
    float direction;
    float mxHp;
   
    PlayerCtrl player;
    Animator anim;
    Image hpGauge;
    private void Start()
    {
        hpBar.SetActive(false);
        mxHp = hp;
        attackType = new int[] {1,2,1,1,2,1,2,1,1,1,2,2,1,1,1,2 };
        i = 1;
        isGround = false;
        bSpeed = speed;
        isMov = true;
        player = FindObjectOfType<PlayerCtrl>();
        anim = GetComponent<Animator>();
        hpGauge = hpBar.GetComponent<Image>();
        isAttack = true;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.right * -5);


        foundTime += Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position.x - transform.position.x;
        Vector2 bPos = transform.position;
        Vector2 aPos = new Vector2(speed * i, 0) * Time.deltaTime;
        Vector2 gPos = new Vector2(0, gravty) * Time.deltaTime;
        hpGauge.fillAmount = hp / mxHp;
        transform.position = bPos + gPos;
        //못찾을때
        if (hp<=0)
        {
            anim.SetBool("isDie", true);
        }
        if (foundTime>=3)
        {
            if (!isFound)
            {
                hpBar.SetActive(false);
                if (i == -1)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                transform.position = bPos + aPos;
                anim.SetBool("isWalk", true);
                walkTime += Time.deltaTime;
                if (walkTime >= 2)
                {
                    i = i * -1;
                    anim.SetBool("isWalk", false);
                    walkTime = 0;
                    foundTime = 0;
                }
            }
        }
        //인식했을떄
        if (dist<=foundRange)
        {
            isFound = true;
            
        }
        else if (direction>=10)
        {
            isFound = false;
        }
        if (isFound)
        {
            hpBar.SetActive(true);
            anim.SetBool("isWalk", false);
            foundTime = 0;
            if (isAttack == true)
            {
                if (direction > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    if (dist > attackRange)
                    {
                        i = 1;
                        transform.position = bPos + aPos + gPos;
                        anim.SetBool("isWalk", true);
                    }
                }
                else if (direction < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    if (dist > attackRange)
                    {
                        i = -1;
                        transform.position = bPos + aPos + gPos;
                        anim.SetBool("isWalk", true);
                    }
                }
                if (dist <= attackRange)
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
        if (other.CompareTag("ground"))
        {
            isGround = true;
        }
        if (other.CompareTag("PlayerAttack"))
        {
            Damage damage = other.gameObject.GetComponent<Damage>();
            hp -= damage.dmg;
            Debug.Log("맞음");
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            isGround = false;
        }
    }
    public void A1()
    {
        anim.SetBool("attack1R", false);
        anim.SetBool("attack1", true);
    }
    public void A1_1()
    {
        Instantiate(attacks[0], transform.position, transform.rotation);
    }
    public void A1_2()
    {
        Instantiate(attacks[1], transform.position, transform.rotation);
    }
    public void A1_3()
    {
        Instantiate(attacks[2], transform.position, transform.rotation);
    }
    public void A1_4()
    {
        //attack1_3.SetActive(false);
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
    public void A2_1()
    {
        Instantiate(attacks[3], transform.position, transform.rotation);
    }
    public void A2_2()
    {
        Instantiate(attacks[4], transform.position, transform.rotation);
    }
    public void A2_3()
    {
        Instantiate(attacks[5], transform.position, transform.rotation);
    }
    public void A2_4()
    {
        Instantiate(attacks[6], transform.position, transform.rotation);
    }
    public void A2_5()
    {
        Instantiate(attacks[7], transform.position, transform.rotation);
    }
    public void A2_6()
    {
        Instantiate(attacks[8], transform.position, transform.rotation);
    }
    public void A2_7()
    {
        Instantiate(attacks[9], transform.position, transform.rotation);
    }
    public void A2_8()
    {
        Instantiate(attacks[10], transform.position, transform.rotation);
    }
    public void A2_9()
    {
        Instantiate(attacks[11], transform.position, transform.rotation);
    }
    public void A3()
    {
        anim.SetBool("attack2", false);
        anim.SetBool("attack3", true);
    }
    public void A3_1()
    {
        Instantiate(attacks[12], transform.position, transform.rotation);
    }
    public void A3_2()
    {
        Instantiate(attacks[13], transform.position, transform.rotation);
    }
    public void A3_3()
    {
        Instantiate(attacks[14], transform.position, transform.rotation);
    }
    public void A3End()
    {
        anim.SetBool("attack3", false);
        ifSmoking = Random.Range(0, 2);
        if (ifSmoking==0)
        {
            isAttack = true;
        }
        else if (ifSmoking==1)
        {
            anim.SetBool("isSit", true);
        }
        if (attackCount <= 14)
        {
            attackCount += 1;
        }
        
    }
    public void A4()
    {
        anim.SetBool("isSit", false);
        anim.SetBool("isSmoking", true);
    }
    public void A4End()
    {
        anim.SetBool("isSmoking", false);
        isAttack = true;
        for (int i = 0; i < 3; i++)
        {
            if (direction > 0)
            {
                Instantiate(prefab, shootPoint2.transform.position, Quaternion.identity);
            }
            else if (direction < 0)
            {
                Instantiate(prefab, shootPoint1.transform.position, Quaternion.identity);
            }
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
