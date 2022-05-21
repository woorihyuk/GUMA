using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoking : MonoBehaviour
{
    PlayerCtrl player;
    Rigidbody2D rigid;

    public float speed;
    int i;
    float a;
    bool isMove;

    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerCtrl>();
        ani = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        float direction = player.transform.position.x - transform.position.x;
        if (direction > 0)
        {
            i=1;
        }
        else if (direction < 0)
        {
            i=-1;
        }
        Vector2 bPos = transform.position;
        Vector2 aPos = new Vector2(speed * i, 0) * Time.deltaTime;
        if (isMove == true)
        {
            Debug.Log("kjsdfshe");
            transform.position = bPos + aPos;
        }
    }
    public void Move()
    {
        Debug.Log("¹«ºê");
        ani.SetBool("isMove", true);
        isMove = true;
        Jump();
        rigid.gravityScale = 0.5f;
    }
    public void End()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ground")
        {

            Destroy(gameObject);
        }
        else if (other.gameObject.name=="player")
        {
            Destroy(gameObject);
        }
    }
    void Jump()
    {
        a = Random.Range(2, 6);
        rigid.AddForce(Vector2.up * a, ForceMode2D.Impulse);
    }
}
