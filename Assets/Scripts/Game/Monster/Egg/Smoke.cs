using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Monster.Egg
{
    public class Smoke : MonoBehaviour
    {
        private bool _stopCheckGround, _isGoingToSmall;

        public int direction;
        public Animator anim;
        public Rigidbody2D rb;

        public CircleCollider2D checkCollider;
        public ContactFilter2D groundFilter, attackFilter;

        private void OnEnable()
        {
            anim.Play("Make");
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            _stopCheckGround = false;
            _isGoingToSmall = false;
        }

        private void Update()
        {
            int count;
            if (!_stopCheckGround)
            {
                count = checkCollider.OverlapCollider(groundFilter, new List<Collider2D>());
                if (count != 0)
                {
                    _stopCheckGround = true;
                    rb.gravityScale = 0;
                    rb.velocity = new Vector2(0, 0);
                    if (!_isGoingToSmall)
                    {
                        transform.DOScale(0, 0.3f).OnComplete(() =>
                        {
                            FxPoolManager.Instance.eggGhostSmokePool.Release(this);
                        }).SetEase(Ease.Linear).Play();
                        _isGoingToSmall = true;
                    }
                }
            }

            var contacts = new List<Collider2D>();
            count = checkCollider.OverlapCollider(attackFilter, contacts);
            if (count == 0) return;
            foreach (var col in contacts)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(10);
                if (!_isGoingToSmall)
                {
                    transform.DOScale(0, 0.3f).OnComplete(() =>
                    {
                        FxPoolManager.Instance.eggGhostSmokePool.Release(this);
                    }).SetEase(Ease.Linear).Play();
                    _isGoingToSmall = true;
                }
            }
        }

        public void Move()
        {
            rb.AddForce(new Vector2(direction * Random.Range(1, 7), Random.Range(2, 10)), ForceMode2D.Impulse);
            rb.gravityScale = 0.5f;
        }
    }
}