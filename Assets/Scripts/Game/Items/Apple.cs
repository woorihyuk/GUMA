namespace Game.Items
{
    public class Apple : ItemBase
    {
        public Apple()
        {
            itemCode = 0;
            name = "Apple";
            currentQuantity = 0;
            maxQuantity = 1;
        }
        
        public override void Use()
        {
            var player = UnityEngine.Object.FindAnyObjectByType<Player.Player>();
            player.hp.Value = player.maxHp;
        }
    }
}