namespace Game.Items
{
    public class ItemBase
    {
        public int itemCode = -1;
        public string name;
        public int maxQuantity;
        public int currentQuantity;

        public ItemBase DeepCopy()
        {
            var newCopy = new ItemBase
            {
                itemCode = itemCode,
                name = name,
                maxQuantity = maxQuantity,
                currentQuantity = currentQuantity
            };
            return newCopy;
        }

        public virtual void Use()
        {
        }
    }
}