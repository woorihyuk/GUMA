using UnityEngine;

namespace InteractiveObjects
{
    public class Tree : InteractiveObject
    {
        public GameObject apple;
        public void SpawnApple()
        {
            Instantiate(apple, transform.position, Quaternion.identity);
        }
    }
}
