using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class SeauniCtrl : MonoBehaviour
{
    public GameObject[] movePoint;
    public GameObject[] lightning2Point;
    public GameObject attackEffect;
    public GameObject lightning;
    public GameObject lightningPoint;
    public GameObject detectPoint;
    public GameObject point4_2;
    public GameObject hpBar;

    public float attackRange;
    public float hp;
    public float foundRange;

    private Player _player;
    private Animator anim;
    private Lightning _lightning1;
    private Lightning _lightning2;
    private Lightning _lightning3;
    private Lightning _lightning4;
    private Lightning _lightning5;
    private Image _hpBar;

    private int _nowPosition;
    private int _moveDirection;

    private float _direction;
    private float _mxHp;

    private bool _isAttack;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].transform.position;
        _nowPosition = 1;
        _lightning1 = lightning2Point[0].GetComponent<Lightning>();
        _lightning2 = lightning2Point[1].GetComponent<Lightning>();
        _lightning3 = lightning2Point[2].GetComponent<Lightning>();
        _lightning4 = lightning2Point[3].GetComponent<Lightning>();
        _lightning5 = lightning2Point[4].GetComponent<Lightning>();
        _hpBar = hpBar.GetComponent<Image>();
        _mxHp = hp;
        hpBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _hpBar.fillAmount = hp / _mxHp;
        var dist = Vector2.Distance(_player.transform.position, detectPoint.transform.position);
        _direction = _player.transform.position.x - detectPoint.transform.position.x;
        if (dist<foundRange)
        {
            hpBar.SetActive(true);
        }
        else
        {
            hpBar.SetActive(false);
        }
        if (dist<attackRange)
        {
            Attack1();
        }
    }
    void Attack1()
    {
        if (!_isAttack)
        {
            switch (_direction)
            {
                case > 0:
                    transform.rotation = Quaternion.Euler(0, 1, 0);
                    break;
                case < 0:
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    break;
            }
        }
        _isAttack = true;
        anim.SetBool("isAttack", true);
        
    }
    void Attack2()
    {
        switch (_nowPosition)
        {
            case 1:
                _moveDirection = Random.Range(0, 2);
                switch (_moveDirection)
                {
                    case 0:
                        transform.position = movePoint[1].transform.position;
                        StartCoroutine(_lightning1.StartLightning());
                        _nowPosition = 2;
                        break;
                    case 1:
                        transform.position = movePoint[2].transform.position;
                        StartCoroutine(_lightning4.StartLightning());
                        _nowPosition = 3;
                        break;
                }
                break;
            case 2:
                transform.position = movePoint[2].transform.position;
                _nowPosition = 3;
                break;
            case 3:
                transform.position = movePoint[3].transform.position;
                StartCoroutine(_lightning5.StartLightning());
                _nowPosition = 4;
                break;
            case 4:
                _moveDirection = Random.Range(0, 2);
                switch (_moveDirection)
                {
                    case 0:
                        transform.position = movePoint[1].transform.position;
                        StartCoroutine(_lightning2.StartLightning());
                        _nowPosition = 2;
                        break;
                    case 1:
                        transform.position = movePoint[0].transform.position;
                        StartCoroutine(_lightning3.StartLightning());
                        _nowPosition = 1;
                        break;
                }
                break;
        }
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void AttackEffect()
    {
        Instantiate(attackEffect, transform.position, transform.rotation);
        Instantiate(lightning, lightningPoint.transform.position, transform.rotation);
    }

    public void AttackEnd()
    {
        anim.SetBool("isAttack", false);
        _isAttack = false;
        Attack2();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Damage damage = collision.gameObject.GetComponent<Damage>();
            hp -= damage.dmg;
            Attack2();
            if (hp<=0)
            {
                anim.SetBool("isDie", true);
            }
        }
    }
}
