using Game.Effect;
using Game.Monster.Egg;
using Game.Monster.Saeuni;
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
        public IObjectPool<DamageText> damageTextPool;
        public DamageText damageTextPrefab;
        public IObjectPool<AttackEffect> saeuniAttackEffectPool;
        public AttackEffect saeuniAttackEffectPrefab;
        public IObjectPool<ThunderEffect> saeuniThunderEffectRedPool;
        public ThunderEffect saeuniThunderEffectRedPrefab;
        public IObjectPool<ThunderEffect> saeuniThunderEffectBluePool;
        public ThunderEffect saeuniThunderEffectBluePrefab;

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
            damageTextPool = new ObjectPool<DamageText>(
                () => Instantiate(damageTextPrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
            saeuniAttackEffectPool = new ObjectPool<AttackEffect>(
                () => Instantiate(saeuniAttackEffectPrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
            saeuniThunderEffectRedPool = new ObjectPool<ThunderEffect>(
                () => Instantiate(saeuniThunderEffectRedPrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
            saeuniThunderEffectBluePool = new ObjectPool<ThunderEffect>(
                () => Instantiate(saeuniThunderEffectBluePrefab, group), o =>
                {
                    o.gameObject.SetActive(true);
                }, o =>
                {
                    o.gameObject.SetActive(false);
                }, o => Destroy(o.gameObject));
        }
    }
}