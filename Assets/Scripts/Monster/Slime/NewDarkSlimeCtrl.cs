using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDarkSlimeCtrl : MonsterMove
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*if (doAttack)
        {
            _isAttack = true;
            _animator.SetBool("isAttack", true);
        }*/
    }

    public void AttackEnd()
    {
        /*_animator.SetBool("isAttack", false);
        _isAttack = false;*/
    }
}
