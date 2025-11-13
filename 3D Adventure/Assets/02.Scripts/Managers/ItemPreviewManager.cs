using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemPreviewManager : MonoBehaviour
{
    [Header("Preview Settings")]
    [SerializeField] private Camera previewCamara;
    [SerializeField] private Transform previewRoot;
    [SerializeField] private RenderTexture previewTexture;
    [SerializeField] private Transform slotParent;
    [SerializeField] private InventorySlot slotPrefab;

    private WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

    private CharacterManager characterManager;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
        characterManager.Player.inventory.onInventoryChanged += RefreshInventoryUI;
        RefreshInventoryUI(characterManager.Player.inventory.GetItems());
    }

    private void OnDisable()
    {
        if (characterManager != null &&
        characterManager.Player != null &&
        characterManager.Player.inventory != null)
            characterManager.Player.inventory.onInventoryChanged -= RefreshInventoryUI;
    }

    private void RefreshInventoryUI(IEnumerable<InventoryItem> items)
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        StartCoroutine(CreateItemIcons(items));
    }

    private IEnumerator CreateItemIcons(IEnumerable<InventoryItem> items)
    {
        foreach (var itemEntry in items)
        {
            ItemData item = itemEntry.data;
            int quantity = itemEntry.quantity;

            InventorySlot slot = Instantiate(slotPrefab, slotParent);

            // create 3d item preview
            GameObject itemInstance = Instantiate(item.previewPrefab, previewRoot);
            //itemInstance.transform.localPosition = Vector3.zero;
            //itemInstance.transform.localRotation = Quaternion.Euler(0, 180, 0);

            previewCamara.targetTexture = previewTexture;
            yield return waitForEnd;
            previewCamara.Render();
            yield return waitForEnd;

            // convert texture2d
            Texture2D snapshot = new(previewTexture.width, previewTexture.height);
            RenderTexture.active = previewTexture;
            snapshot.ReadPixels(
                new Rect(0, 0, previewTexture.width, previewTexture.height)
                , 0, 0);
            snapshot.Apply();
            RenderTexture.active = null;

            // slot setting
            slot.SetItem(snapshot, item, quantity);

            Destroy(itemInstance);
        }
    }
}
