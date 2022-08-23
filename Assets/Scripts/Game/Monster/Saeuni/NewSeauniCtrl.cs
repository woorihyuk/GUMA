using Game.Monster;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Monster.Seauni
{
    public class NewSeauniCtrl : Monster
    {
        public Vector2[] movePoint;
        public Transform lightningPoint;
        public GameObject lightning;

        private Animator _animator;
        private int position;
        private bool _isAttack;

        protected override void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            transform.position = movePoint[0];
            position = 1;

        }
        protected override void OnDirectionSet(int direction)
        {
        }

        protected override void OnHpDrown()
        {
            _animator.Play("die");
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
        }

        private void Move()
        {
            int i = Random.Range(1, 3);
            Vector2 startPos = transform.position;
            switch (position)
            {
                case 1:
                    switch (i)
                    {
                        case 1:
                            transform.position = movePoint[1];
                            break;
                        case 2:
                            transform.position = movePoint[2];
                            break;
                    }
                    break;
                case 2:
                    transform.position = movePoint[2];
                    break;
                case 3:
                    transform.position = movePoint[3];
                    break;

                case 4:
                    switch (i)
                    {
                        case 1:
                            transform.position = movePoint[0];

                            break;
                        case 2:
                            transform.position = movePoint[1];
                            break;
                    }
                    break;


            }
            LightningEffectPlay(startPos, transform.position);

        }

        private void Attack()
        {
            _animator.Play("Attack_Motion");
        }

        public void LightningEffectPlay(Vector2 startPos, Vector2 endPos)
        {

        }

        #region 애니메이션 이벤트

        public void OnDieEnd()
        {
            Destroy(gameObject);
        }

        public void AttackEvent1()
        {
            Instantiate(lightning, lightningPoint.position, Quaternion.identity);
        }

        public void AttackEvent2()
        {
            _animator.Play("New State");
            Move();
        }
        #endregion
    }
}

