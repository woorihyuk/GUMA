using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attack;
    private Vector3 attackPos;
    private Animator anim;
    private SpriteRenderer sr;
    private bool _isGround;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        var bPos = (Vector2)transform.position;
        var aPos = new Vector2(0, -70) * Time.deltaTime;
        if (!_isGround)
        {
            transform.position = bPos + aPos;
        }
    }
    public void Move(int i)
    {
        transform.position = new Vector3(player.transform.position.x, 20, 0);
        _isGround = false;
        transform.rotation = Quaternion.Euler(0, i == -1 ? 0 : 180, 0); 
    }
    public void Attack()
    {
        sr.color = new Color(1, 1, 1, 1);
        anim.SetBool("isAttack", true);
    }
    public void AttackRange()
    {
        Instantiate(attack, transform.position, Quaternion.identity);
    }
    public void AttackEnd()
    {
        anim.SetBool("isAttack", false);
        sr.color = new Color(1, 1, 1, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            _isGround = true;
        }
    }

}
