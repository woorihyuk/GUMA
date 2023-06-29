namespace Game.Object.Items
{
    public class Apple : InteractiveObject
    {
        public override void OnInteract()
        {
            Managers.GetInstance().inventoryManager.AddItem("apple");
            Destroy(gameObject);
        }
    }
}