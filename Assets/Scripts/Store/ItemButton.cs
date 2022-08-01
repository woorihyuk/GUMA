using Store;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Button button;
    public Text nameText, costText, descriptionText, quantityText;

    public void SetTextData(Item item)
    {
        nameText.text = item.name;
        costText.text = $"가격: {item.cost.ToString()}";
        descriptionText.text = item.description;
        if (item.currentQuantity == -1)
        {
            quantityText.gameObject.SetActive(false);
        }
        else
        {
            quantityText.text = $"구매 가능한 수량: {item.currentQuantity}";
        }
    }
}