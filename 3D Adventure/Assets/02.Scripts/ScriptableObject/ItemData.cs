using System;
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
    Stamina,
}

[Serializable]
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
    public GameObject dropPrefab;
    public GameObject previewPrefab;

    [Header("Stacking")]
    public bool canStack;

    [Header("Consumable")]
    public ConsumableItemData[] consumableItemDatas;

    [Header("Equip")]
    public GameObject equipPrefab;
}
