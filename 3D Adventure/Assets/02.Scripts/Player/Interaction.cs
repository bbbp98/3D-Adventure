using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Ray Settings")]
    [SerializeField] private float checkRate = 0.05f;
    private float lastCheckTime;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform modelTransform;

    private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    [SerializeField] GameObject interactUI;
    [SerializeField] TextMeshProUGUI itemNameText;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Debug.DrawRay(transform.position, modelTransform.forward, Color.red);

            if (Physics.Raycast(transform.position, modelTransform.forward , out RaycastHit hit, checkDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();

                    // set UI
                    interactUI.SetActive(true);
                    itemNameText.text = curInteractable.GetItemData().itemName;
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                interactUI.SetActive(false);
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
        }
    }
}
