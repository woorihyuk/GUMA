using Game.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Monster.Seauni
{
    public class NewSeauniCtrl : Monster
    {
        public GameObject[] movePoint;
        public Lightning lightningPoint;

        private Animator _animator;
        private int position;
        private bool _isAttack;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();

        }
        protected override void OnDirectionSet(int direction)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnHpDrown()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnPlayerFound()
        {
            GameUIManager.Instance.TryPushHpBar(GetInstanceID().ToString(), "새우니", (float)hp.Value / maxHp.Value);
            if (!_isAttack)
            {
                Attack();
            }
        }

        protected override void OnPlayerLost()
        {
            throw new System.NotImplementedException();
        }

        private void Move()
        {
            switch (position)
            {

            }
            lightningPoint.LightningEffectPlay();
        }

        private void Attack()
        {
            _animator.Play("Attack_Motion");
            Move();
        }

        #region 애니메이션 이벤트
        
        public void OnDieEnd()
        {
            Destroy(gameObject);
        }

        public void AttackEvent1()
        {

        }

        public void AttackEvent2()
        {

        }
        #endregion
    }
}

