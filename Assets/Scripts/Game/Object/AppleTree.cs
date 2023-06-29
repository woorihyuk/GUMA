using UnityEngine;

namespace Game.Object
{
    public class AppleTree : InteractiveObject
    {
        public GameObject applePrefab;

        public override void OnInteract()
        {
            var position = transform.position;
            var itemPosX = Random.Range(position.x - 1.75f, position.x + 2);
            position.x = itemPosX;
            Instantiate(applePrefab, position, Quaternion.identity);
        }
    }
}
