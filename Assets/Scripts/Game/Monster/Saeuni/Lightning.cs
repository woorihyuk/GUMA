using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Monster
{
    public class Lightning : MonoBehaviour
    {
        public LayerMask attackContactLayerMask;
        public BoxCollider2D attackRange;
        private ContactFilter2D _attackContactFilter;
        private void AttackRange(int index, int dmg)
        {
            var players = new List<Collider2D>();
            var counts = attackRange.OverlapCollider(_attackContactFilter, players);
            if (counts == 0) return;
            foreach (var col in players)
            {
                var player = col.GetComponent<Player.Player>();
                player.GetDamage(dmg);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LightningAnim());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator LightningAnim()
        {
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            Destroy(gameObject);
        }


    }
    
}
