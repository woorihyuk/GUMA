using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Game.Object.Items
{
    public class Apple : InteractiveObject
    {
        public override void OnInteract()
        {
            Managers.Instance.inventoryManager.AddItem("apple");
            var go = gameObject;
            transform.DOJump(new Vector3(0, 0.27f, 0), 0.8f, 1, 0.35f).SetEase(Ease.Linear).SetRelative().OnComplete(() =>
            {
                Destroy(go);
            }).Play();
            Destroy(this);
        }
    }
}