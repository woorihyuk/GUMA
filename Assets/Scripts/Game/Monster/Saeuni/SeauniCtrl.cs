//using System;
//using Game;
//using Game.Player;
//using UnityEngine;
//using UnityEngine.UI;
//using Random = UnityEngine.Random;

//public class SeauniCtrl : MonoBehaviour
//{
//    public GameObject[] movePoint;
//    public GameObject[] lightning2Point;
//    public GameObject attackEffect;
//    public GameObject lightning;
//    public GameObject lightningPoint;
//    public GameObject detectPoint;
//    public GameObject point4_2;

//    public float attackRange;
//    public float hp;
//    public float foundRange;

//    private Player _player;
//    private Animator anim;
//    private Lightning _lightning1;
//    private Lightning _lightning2;
//    private Lightning _lightning3;
//    private Lightning _lightning4;
//    private Lightning _lightning5;

//    private int _nowPosition;
//    private int _moveDirection;

//    private float _direction;
//    private float _mxHp, _lastHp;
//    private GameUIManager _gameUIManager;

//    private bool _isAttack;
//    private static readonly int IsAttack = Animator.StringToHash("isAttack");

//    private void Start()
//    {
//        _gameUIManager = FindObjectOfType<GameUIManager>();
//        _player = FindObjectOfType<Player>();
//        anim = GetComponent<Animator>();
//        transform.position = movePoint[0].transform.position;
//        _nowPosition = 1;
//        _lightning1 = lightning2Point[0].GetComponent<Lightning>();
//        _lightning2 = lightning2Point[1].GetComponent<Lightning>();
//        _lightning3 = lightning2Point[2].GetComponent<Lightning>();
//        _lightning4 = lightning2Point[3].GetComponent<Lightning>();
//        _lightning5 = lightning2Point[4].GetComponent<Lightning>();
//        _mxHp = hp;
//    }

//    private void OnDestroy()
//    {
//        _gameUIManager.TryPopHpBar(GetInstanceID().ToString());
//    }

//    private void Update()
//    {
//        var dist = Vector2.Distance(_player.transform.position, detectPoint.transform.position);
//        _direction = _player.transform.position.x - detectPoint.transform.position.x;

//        if (dist < foundRange)
//        {
//            _gameUIManager.TryPushHpBar(GetInstanceID().ToString(), "느그우니", hp);
//            RefreshHp(hp);
//        }
//        else
//        {
//            _gameUIManager.TryPopHpBar(GetInstanceID().ToString());
//        }
        
//        if (dist < attackRange)
//        {
//            Attack1();
//        }
//    }

//    private void Attack1()
//    {
//        if (!_isAttack)
//        {
//            switch (_direction)
//            {
//                case > 0:
//                    transform.rotation = Quaternion.Euler(0, 1, 0);
//                    break;
//                case < 0:
//                    transform.rotation = Quaternion.Euler(0, -180, 0);
//                    break;
//            }
//        }

//        _isAttack = true;
//        anim.SetBool(IsAttack, true);
//    }

//    private void Attack2()
//    {
//        switch (_nowPosition)
//        {
//            case 1:
//                _moveDirection = Random.Range(0, 2);
//                switch (_moveDirection)
//                {
//                    case 0:
//                        transform.position = movePoint[1].transform.position;
//                        StartCoroutine(_lightning1.StartLightning());
//                        _nowPosition = 2;
//                        break;
//                    case 1:
//                        transform.position = movePoint[2].transform.position;
//                        StartCoroutine(_lightning4.StartLightning());
//                        _nowPosition = 3;
//                        break;
//                }

//                break;
//            case 2:
//                transform.position = movePoint[2].transform.position;
//                _nowPosition = 3;
//                break;
//            case 3:
//                transform.position = movePoint[3].transform.position;
//                StartCoroutine(_lightning5.StartLightning());
//                _nowPosition = 4;
//                break;
//            case 4:
//                _moveDirection = Random.Range(0, 2);
//                switch (_moveDirection)
//                {
//                    case 0:
//                        transform.position = movePoint[1].transform.position;
//                        StartCoroutine(_lightning2.StartLightning());
//                        _nowPosition = 2;
//                        break;
//                    case 1:
//                        transform.position = movePoint[0].transform.position;
//                        StartCoroutine(_lightning3.StartLightning());
//                        _nowPosition = 1;
//                        break;
//                }

//                break;
//        }
//    }

//    public void Die()
//    {
//        Destroy(gameObject);
//    }

//    public void AttackEffect()
//    {
//        Instantiate(attackEffect, transform.position, transform.rotation);
//        Instantiate(lightning, lightningPoint.transform.position, transform.rotation);
//    }

//    public void AttackEnd()
//    {
//        anim.SetBool("isAttack", false);
//        _isAttack = false;
//        Attack2();
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("PlayerAttack"))
//        {
//            Damage damage = collision.gameObject.GetComponent<Damage>();
//            hp -= damage.dmg;
//            Attack2();
//            if (hp <= 0)
//            {
//                anim.SetBool("isDie", true);
//            }
//        }
//    }

//    private void PlayerDie()
//    {
//        hp = _mxHp;
//    }
    
//    private void RefreshHp(float newHp)
//    {
//        if (!newHp.Equals(_lastHp))
//        {
//            try
//            {
//                _gameUIManager.SetHpBarPercent(GetInstanceID().ToString(), newHp/_mxHp);
//            }
//            catch
//            {
//                return;
//            }
//            _lastHp = newHp;
//        }
//    }
//}