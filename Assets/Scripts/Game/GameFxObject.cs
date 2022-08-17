using UnityEngine;

namespace Game
{
    public class GameFxObject : MonoBehaviour
    {
        public Animator animator;
        
        public virtual void OnAnimationEnd()
        {
            FxPoolManager.Instance.playerHitFxPool.Release(this);
        }
    }
}