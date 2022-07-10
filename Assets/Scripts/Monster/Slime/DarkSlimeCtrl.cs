using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSlimeCtrl : MonoBehaviour
{
    public GameObject player;
    public GameObject attackEffect;
    public int speed;
    public int foundRange;
    private Animator anim;
    private AttackCtrl attackCtrl;
    private float _direction;
    private float _i;
    private bool _isFound;
    private bool _isWalk;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackCtrl = FindObjectOfType<AttackCtrl>();
        _i = -1;
    }
    IEnumerator WalkTime()
    {
        print("p");
        _isWalk = true;
        transform.rotation = Quaternion.Euler(0, _i == 1 ? 0 : -180, 0);
        _i = _i == -1 ? 1 : -1;
        yield return YieldlnstructionCache.WaitForSeconds(2);
        _isWalk = false;
    }
    // Update is called once per frame
    void Update()
    {
        var dist = Vector2.Distance(transform.position, player.transform.position);
        _direction = player.transform.position.x - transform.position.x;
        var bPos = (Vector2)transform.position;
        var aPos = new Vector2(speed * _i, 0) * Time.deltaTime;
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
        }
    }
    public void AttackEffect()
    {
        attackCtrl.Attack();
    }
    public void AttackEnd()
    {
        anim.SetBool("isAttack", false);
    }
}
