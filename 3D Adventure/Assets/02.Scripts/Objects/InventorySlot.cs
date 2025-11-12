using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [field: SerializeField] public RawImage Icon { get; private set; }
    [field: SerializeField] public TextMeshProUGUI QuantityText { get; private set; }

    private ItemData itemData;
    public void OnClick()
    {
        CharacterManager.Instance.Player.inventory.SelectItem(itemData);
    }

    public void SetItem(Texture2D iconTexture, ItemData item, int quantity)
    {
        itemData = item;
        Icon.texture = iconTexture;
        QuantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    public void Clear()
    {
        Icon.texture = null;
        QuantityText.text = string.Empty;
    }
}
