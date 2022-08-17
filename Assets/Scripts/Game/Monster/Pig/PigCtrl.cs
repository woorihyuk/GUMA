using System.Collections;
using System.Collections.Generic;
using Game.Player;
using UnityEngine;
using UnityEngine.UI;

public class PigCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attack1;
    public GameObject attack2;
    public GameObject hpBar;

    public int speed;
    public float HP;
    public float attackRagne;
    public float foundRange;

    private Animator animator;
    private Image _hpGauge;
    private Player playerCtrl;
    private int _attackCount;
    private float _direction;
    private float _mxHP;
    private float _i;
    private bool _isWalk;
    private bool _isAttack;
    private bool _attackDelay;
    private bool _isFound;
    private bool _isWait;
    private bool _isWaiting;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _hpGauge = hpBar.GetComponent<Image>();
        playerCtrl = FindObjectOfType<Player>();
        _i = 1;
        _mxHP = HP;
        hpBar.SetActive(false);
    }

    IEnumerator WalkTime()
    {
        _isWalk = true;
        yield return YieldInstructionCache.WaitForSeconds(2);
        _isWait = true;
    }

    IEnumerator Wait()
    {
        animator.SetBool("isWalk", false);
        _isWaiting = true;
        _i = _i == -1 ? 1 : -1;
        yield return YieldInstructionCache.WaitForSeconds(1);
        transform.rotation = Quaternion.Euler(0, _i == -1 ? 0 : -180, 0);
        animator.SetBool("isWalk", true);
        _isWaiting = false;
        _isWait = false;
        _isWalk = false;
    }

    IEnumerator AttackDelay()
    {
         
        yield return YieldInstructionCache.WaitForSeconds(1.5f);
        print('d');
        _isAttack = false;

    }
    // Update is called once per frame
    void Update()
    {
        _hpGauge.fillAmount = HP / _mxHP;
        var dist = Vector2.Distance(transform.position, player.transform.position);
        _direction = player.transform.position.x - transform.position.x;
        var bPos = (Vector2)transform.position;
        var aPos = new Vector2(speed * _i, 0) * Time.deltaTime;
        if (dist <= foundRange)
        {
            _isFound = true;
        }
        else
        {
            _isFound = false;
        }
        if (playerCtrl.hp<=0)
        {
            _isFound = false;
        }
        if (!_isFound)
        {
            hpBar.SetActive(false);
            print("못찾음");
            if (!_isWait)
            {
                if (!_isWalk)
                {
                    StartCoroutine(WalkTime());
                }
                animator.SetBool("isWalk", true);
                transform.rotation = Quaternion.Euler(0, _i == -1 ? 0 : -180, 0);
                transform.position = bPos + aPos;
            }
            else
            {
                if (!_isWaiting)
                {
                    StartCoroutine(Wait());
                }
            }
        }
        else
        {
            hpBar.SetActive(true);
            if (!_isAttack)
            {
                if (dist <= attackRagne)
                {
                    if (!_attackDelay)
                    {
                        Attack();
                    }
                }
                else
                {
                    print("사거리밖");
                    _attackCount = 0;
                    animator.SetBool("isWalk", true);
                    switch (_direction)
                    {
                        case > 0://플레이어가 왼쪽
                            print("오른쪽으로");
                            _i = 1;
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            transform.position = bPos + aPos;
                            break;
                        case < 0://플레이어가 오른쪽
                            print("왼쪽으로");
                            _i = -1;
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            transform.position = bPos + aPos;
                            break;
                    }
                }
            }
            
        }
    }
    void Attack()
    {
        _isAttack = true;
        animator.SetBool("isAttack", true);
    }

    public void AttackEffect()
    {
        if (_attackCount==0)
        {

        }
        else
        {

        }
    }

    public void AttackEnd()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isAttack", false);
        
        StartCoroutine(AttackDelay());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Damage damage = other.GetComponent<Damage>();
            HP -= damage.dmg;
            if (HP<=0)
            {
                animator.SetBool("isDie", true);
            }
        }
        if (other.name=="TurnPoint1")
        {
            if (!_isFound)
            {
                _i = 1;
            }
        }
        if (other.name == "TurnPoint2")
        {
            if (!_isFound)
            {
                _i = -1;
            }
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
