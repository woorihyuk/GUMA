using System;

namespace Items
{
    [Serializable]
    public class ItemBase
    {
        public string name;
        public string description;
        public int maxQuantity;
        public int currentQuantity;

        public ItemBase DeepCopy()
        {
            var newCopy = new ItemBase
            {
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