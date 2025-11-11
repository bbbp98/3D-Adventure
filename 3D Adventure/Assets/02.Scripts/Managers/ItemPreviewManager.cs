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
    [SerializeField] private Slot slotPrefab;

    private WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void RefreshInventoryUI(List<ItemData> items)
    {
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        StartCoroutine(CreateItemIcons(items));
    }

    private IEnumerator CreateItemIcons(List<ItemData> items)
    {
        foreach (var item in items)
        {
            // create slot
            Slot slot = Instantiate(slotPrefab, slotParent);
            GameObject itemInstance = Instantiate(item.previewPrefab, previewRoot);
            itemInstance.transform.localPosition = Vector3.zero;
            itemInstance.transform.localRotation = Quaternion.identity;

            previewCamara.targetTexture = previewTexture;

            yield return waitForEnd;

            previewCamara.Render();

            yield return waitForEnd;
            yield return null;

            Texture2D snapshot = new Texture2D(previewTexture.width, previewTexture.height);
            RenderTexture.active = previewTexture;
            snapshot.ReadPixels(new Rect(0, 0, previewTexture.width, previewTexture.height), 0, 0);
            snapshot.Apply();
            RenderTexture.active = null;

            // 소모템, 장비템 구분
            slot.SetItem(snapshot, item);
            Destroy(itemInstance);
        }
    }
}
