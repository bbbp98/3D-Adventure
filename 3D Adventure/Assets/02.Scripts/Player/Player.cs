using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public AnimationHandler animationHandler;

    public bool isRun;

    public ItemData itemData;
    public Transform dropPosition;

    public Action onAddItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        animationHandler = GetComponent<AnimationHandler>();
    }
}
