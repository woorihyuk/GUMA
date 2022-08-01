using Store;
using UnityEngine;
using UnityEngine.UI;

public class StoreBuyPopUpTest : MonoBehaviour
{
    public Text nameText;
    public Button buyButton, cancelButton;
    public CanvasGroup canvasGroup;

    public void SetData(Item item)
    {
        nameText.text = item.name;
    }
}
