using Game.Inventory;

namespace Game
{
    public class Managers : PrefabSingleton<Managers>
    {
        private void Start()
        {
            inventoryManager = Instantiate(inventoryPrefab, transform);
        }

        public InventoryManager inventoryPrefab;
        public InventoryManager inventoryManager;
    }
}