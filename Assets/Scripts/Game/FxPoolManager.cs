using Game.Monster.Egg;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class FxPoolManager : PrefabSingleton<FxPoolManager>
    {
        public Transform group;
        
        public IObjectPool<GameFxObject> playerHitFxPool;
        public GameFxObject playerHitFxPrefab;
        public IObjectPool<CannonBullet> bulletPool;
        public CannonBullet bulletPrefab;
        public IObjectPool<Smoke> eggGhostSmokePool;
        public Smoke eggGhostSmokePrefab;

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
            bulletPool = new ObjectPool<CannonBullet>(
                () =>Instantiate(bulletPrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
            eggGhostSmokePool = new ObjectPool<Smoke>(
                () => Instantiate(eggGhostSmokePrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
        }
    }
}