using JetBrains.Annotations;
using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
    Buff,
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

public enum BuffType
{
    SpeedUp,
}

[Serializable]
public class BuffItemData
{
    public BuffType buffType;
    public Sprite buffIcon;
    public string buffName;
    public float buffValue;
    public float duration;
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

    [Header("Buff")]
    public BuffItemData[] buffItemDatas;
}
