using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class FxPoolManager : PrefabSingleton<FxPoolManager>
    {
        public Transform group;
        
        public IObjectPool<GameFxObject> playerHitFxPool;
        public GameFxObject playerHitFxPrefab;

        protected override void Awake()
        {
            base.Awake();
            playerHitFxPool = new ObjectPool<GameFxObject>(
                () =>Instantiate(playerHitFxPrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                    o.animator.Play("PlayerHit");
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
        }
    }
}