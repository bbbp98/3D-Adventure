using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    private Transform dropPosition;

    private ItemData selectedItem;

    private Dictionary<ItemData, InventoryItem> inventoryDict = new();

    #region event
    public delegate void OnInventoryChanged(IEnumerable<InventoryItem> items);
    public event OnInventoryChanged onInventoryChanged;
    #endregion

    private void Start()
    {
        CharacterManager.Instance.Player.inventory = this;
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.onOpenInventory += Toggle;
        CharacterManager.Instance.Player.onAddItem += AddItem;

        inventoryWindow.SetActive(false);
    }

    #region Button Event
    public void OnUseButton()
    {
        if (selectedItem.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumableItemDatas.Length; i++)
            {
                switch(selectedItem.consumableItemDatas[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumableItemDatas[i].value);
                        break;
                    case ConsumableType.Stamina:
                        condition.RecoveryStamina(selectedItem.consumableItemDatas[i].value);
                        break;
                }
            }
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveItem(selectedItem);
    }
    #endregion

    private void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    private void Toggle()
    {
        if (IsOpen())
            inventoryWindow.SetActive(false);
        else
            inventoryWindow.SetActive(true);
    }

    private bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData newItem = CharacterManager.Instance.Player.itemData;

        // check item can stack
        if (newItem.canStack && inventoryDict.TryGetValue(newItem, out InventoryItem existing))
        {
            existing.quantity++;
        }
        else
        {
            inventoryDict[newItem] = new InventoryItem(newItem, 1);
        }

        onInventoryChanged?.Invoke(inventoryDict.Values);
    }

    public void RemoveItem(ItemData item)
    {
        if (!inventoryDict.TryGetValue(item, out InventoryItem target)) return;

        target.quantity--;

        if (target.quantity <= 0)
            inventoryDict.Remove(item);

        onInventoryChanged?.Invoke(inventoryDict.Values);
    }

    private void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public IEnumerable<InventoryItem> GetItems() => inventoryDict.Values;

    public void SelectItem(ItemData item)
    {
        if (!inventoryDict.TryGetValue(item, out InventoryItem existing)) return;

        selectedItem = item;

        selectedItemName.text = selectedItem.itemName;
        string description = selectedItem.itemDescription;
        description = description.Replace("\\n", "\n");
        selectedItemDescription.text = description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumableItemDatas.Length; i++)
        {
            selectedItemStatName.text += selectedItem.consumableItemDatas[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.consumableItemDatas[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.itemType == ItemType.Consumable);
        dropButton.SetActive(true);
    }
}
