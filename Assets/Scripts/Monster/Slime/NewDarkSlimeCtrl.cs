using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDarkSlimeCtrl : MonsterMove
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(base.AIMove(3f, 0.3f, 3f));
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void AttackEnd()
    {
        /*_animator.SetBool("isAttack", false);
        _isAttack = false;*/
    }

    protected override void OnPlayerFound()
    {
    }
}
