using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attack;
    private Animator anim;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(int i)
    {
        transform.position = new Vector3(player.transform.position.x + 2 * i, 20, 0);
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
        sr.color = new Color(1, 1, 1, 0);
    }
}
