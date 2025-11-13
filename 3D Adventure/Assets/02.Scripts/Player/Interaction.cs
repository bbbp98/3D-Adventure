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

    private CharacterManager characterManager;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Vector3 pos = transform.position;
            pos += Vector3.up * 0.5f;

            Debug.DrawRay(pos, modelTransform.forward, Color.red);

            if (Physics.Raycast(pos, modelTransform.forward, out RaycastHit hit, checkDistance, layerMask))
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
        if (context.phase == InputActionPhase.Started && curInteractable != null &&
            characterManager.Player.controller.isGrounded)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            characterManager.Player.animationHandler.OnInteract();
        }
    }
}
