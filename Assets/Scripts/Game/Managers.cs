using Game.Inventory;
using UnityEngine;

namespace Game
{
    public class Managers : MonoBehaviour
    {
        private static Managers _instance;

        public static Managers GetInstance()
        {
            Init();
            return _instance;
        }

        private void Start()
        {
            Init();
            inventoryManager = Instantiate(inventoryPrefab, transform);
        }

        private static void Init()
        {
            if (_instance != null) return;
            //@Managers 가 존재하는지 확인
            var go = GameObject.Find("@Managers");
            //없으면 생성
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
            }

            if (go.GetComponent<Managers>() == null)
            {
                go.AddComponent<Managers>();
            }

            //없어지지 않도록 해줌
            DontDestroyOnLoad(go);
            //instance 할당
            _instance = go.GetComponent<Managers>();
        }

        public InventoryManager inventoryPrefab;
        public InventoryManager inventoryManager;
    }
}