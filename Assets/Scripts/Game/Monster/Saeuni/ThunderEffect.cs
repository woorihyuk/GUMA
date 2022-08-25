using System.Collections.Generic;
using UnityEngine;

namespace Game.Monster.Saeuni
{
    public class ThunderEffect : MonoBehaviour
    {
        public enum ThunderType
        {
            Red,
            Blue
        }

        public Animator animator;
        public BoxCollider2D attackRangeCollider;
        public ThunderType type;
        public ContactFilter2D attackContactFilter;
        public int dmg;
        
        private void OnEnable()
        {
            animator.Play("Thunder", -1, 0);
        }

        public void OnAttackEvent()
        {
            var players = new List<Collider2D>();
            var counts = attackRangeCollider.OverlapCollider(attackContactFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(dmg);
            }
        }
    
        public void OnAttackAnimationEnd()
        {
            if (type == ThunderType.Red) FxPoolManager.Instance.saeuniThunderEffectRedPool.Release(this);
            else FxPoolManager.Instance.saeuniThunderEffectBluePool.Release(this);
        }
    }
}
