using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData data;
    private float rotateSpeed = 50f;

    private CharacterManager characterManager;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    public ItemData GetItemData()
    {
        return data;
    }

    public void OnInteract()
    {
        characterManager.Player.itemData = data;
        characterManager.Player.onAddItem?.Invoke();
        Destroy(gameObject);
    }
}
