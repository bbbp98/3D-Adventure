using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
}

public class ConsumableItemData
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;
    public GameObject previewPrefab;

    [Header("Consumable")]
    public ConsumableItemData[] consumableItemDatas;

    [Header("Equip")]
    public GameObject equipPrefab;
}
