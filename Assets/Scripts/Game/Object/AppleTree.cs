using UnityEngine;

namespace InteractiveObjects
{
    public class AppleTree : InteractiveObject
    {
        public GameObject apple;
        public void SpawnApple()
        {
            Instantiate(apple, transform.position, Quaternion.identity);
        }
    }
}
