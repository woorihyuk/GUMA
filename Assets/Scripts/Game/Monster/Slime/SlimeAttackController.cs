using System.Collections.Generic;
using UnityEngine;

namespace Game.Monster.Slime
{
    public class SlimeAttackController : MonoBehaviour
    {
        public LayerMask attackContactLayerMask, groundContactLayerMask;
        
        private Animator _animator;
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private ContactFilter2D _attackCheckFilter, _groundCheckFilter;
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _attackCheckFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = attackContactLayerMask
            };
            _groundCheckFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = groundContactLayerMask
            };
        }

        public void Attack()
        {
            gameObject.SetActive(true);
            _animator.SetBool(IsAttack, true);
        }

        public void SetPosition(Vector3 newPos)
        {
            var list = new List<RaycastHit2D>();
            var count = Physics2D.Raycast(newPos, Vector2.down, _groundCheckFilter, list);
            if (count == 0) return;
            transform.position = new Vector3(newPos.x, list[0].point.y + _spriteRenderer.bounds.size.y / 2);
        }

        public void Flip(bool x)
        {
            _spriteRenderer.flipX = x;
        }

        public void AttackRange()
        {
            var players = new List<Collider2D>();
            var counts = _collider.OverlapCollider(_attackCheckFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(10);
            }
        }

        public void AttackEnd()
        {
            _animator.SetBool(IsAttack, false);
            gameObject.SetActive(false);
        }
    }
}