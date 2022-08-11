using System.Collections.Generic;
using UnityEngine;

namespace Game.Monster.Slime
{
    public class SlimeAttackController : MonoBehaviour
    {
        public LayerMask attackContactLayerMask;
        private Collider2D _collider;
        
        private Animator _animator;
        private ContactFilter2D _attackCheckFilter;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider2D>();
            _attackCheckFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = attackContactLayerMask
            };
            gameObject.SetActive(false);
        }

        public void Attack()
        {
            gameObject.SetActive(true);
            _animator.SetBool(IsAttack, true);
        }

        public void SetPosition(float newX)
        {
            var transform1 = transform;
            Vector3 transformPosition = transform1.position;
            transformPosition.x = newX;
            transform1.position = transformPosition;
        }

        public void AttackRange()
        {
            var players = new List<Collider2D>();
            var counts = _collider.OverlapCollider(_attackCheckFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player>();
                player.hp -= 10;
                player.RefreshHp();
            }
        }

        public void AttackEnd()
        {
            _animator.SetBool(IsAttack, false);
            gameObject.SetActive(false);
        }
    }
}