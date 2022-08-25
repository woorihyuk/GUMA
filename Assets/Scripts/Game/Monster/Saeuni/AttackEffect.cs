using UnityEngine;

namespace Game.Monster.Saeuni
{
    public class AttackEffect : MonoBehaviour
    {
        public Animator animator;

        private void OnEnable()
        {
            animator.Play("AttackEffect", -1, 0);
        }

        public void OnAttackAnimationEnd()
        {
            FxPoolManager.Instance.saeuniAttackEffectPool.Release(this);
        }
    }
}