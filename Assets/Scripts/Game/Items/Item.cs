namespace Items
{
    public class ItemBase
    {
        public int itemCode = -1;
        public string name;
        public string description;
        public int maxQuantity;
        public int currentQuantity;

        public ItemBase DeepCopy()
        {
            var newCopy = new ItemBase
            {
                itemCode = itemCode,
                name = name,
                description = description,
                maxQuantity = maxQuantity,
                currentQuantity = currentQuantity
            };
            return newCopy;
        }
    }

    public interface IItemAction
    {
        public void Use();
    }
}