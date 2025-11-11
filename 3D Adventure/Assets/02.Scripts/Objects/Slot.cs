using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [field: SerializeField] public RawImage Icon { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuantityText { get; private set; }

    public ItemData linkedItem;

    public void SetItem(Texture2D iconTexture, ItemData item, int quantity = 1)
    {
        Icon.texture = iconTexture;
        linkedItem = item;
        QuantityText.text = quantity.ToString();
    }

    public void Clear()
    {
        Icon.texture = null;
        linkedItem = null;
        QuantityText.text = string.Empty;
    }
}
