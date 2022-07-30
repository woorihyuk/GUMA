using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSlimeCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attackEffect;
    public int speed;
    public int foundRange;
    public float HP;
    private Animator anim;
    private AttackCtrl attackCtrl;
    private float _direction;
    private float _i;
    private bool _isFound;
    private bool _isWalk;
    private bool _isAttack;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackCtrl = attackEffect.GetComponent<AttackCtrl>();
        _i = -1;
    }
    IEnumerator WalkTime()
    {
        print("p");
        _isWalk = true;
        transform.rotation = Quaternion.Euler(0, _i == 1 ? 0 : -180, 0);
        _i = _i == -1 ? 1 : -1;
        yield return YieldlnstructionCache.WaitForSeconds(1.5f);
        _isWalk = false;
    }
    // Update is called once per frame
    void Update()
    {
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
            _isAttack = false;
            anim.SetBool("isAttack", false);
        }
        if (!_isFound)
        {
            if (!_isWalk)
            {
                StartCoroutine(WalkTime());
            }
            else
            {
                transform.position = bPos + aPos;
            }
        }
        else
        {
            if (!_isAttack)
            {
                anim.SetBool("isAttack", true);
                switch (_direction)
                {
                    case > 0:
                        attackCtrl.Move(1);
                        break;
                    case < 0:
                        attackCtrl.Move(-1);
                        break;
                }
                _isAttack = true;
            }
            
        }
    }
    public void AttackEffect()
    {
        attackCtrl.Attack();
    }
    public void AttackEnd()
    {
        print("1");
        _isAttack = false;
        anim.SetBool("isAttack", false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Damage damage = other.GetComponent<Damage>();
            HP -= damage.dmg;
            if (HP <= 0)
            {
                anim.SetBool("isDie", true);
            }
        }
        if (other.name == "TurnPoint")
        {
            _i = _i == 1 ? -1 : 1;
            transform.rotation = Quaternion.Euler(0, _i == 1 ? 0 : -180, 0);
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
