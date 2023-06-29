using Game.Items;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectableObject : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text quantityText;

    public void SetData(ItemBase item)
    {
        nameText.text = item.name;
        if (item.currentQuantity == 1)
        {
            quantityText.gameObject.SetActive(false);
        }
        else
        {
            quantityText.text = item.currentQuantity.ToString();
        }
    } 
}
