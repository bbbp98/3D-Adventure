using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;

    private PlayerController controller;
    private PlayerCondition condition;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    private Transform dropPosition;

    private ItemData selectedItem;
    private int selectedItemIndex = -1;

    private int curEquipIndex;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.onOpenInventory += Toggle;

        inventoryWindow.SetActive(false);
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
}
