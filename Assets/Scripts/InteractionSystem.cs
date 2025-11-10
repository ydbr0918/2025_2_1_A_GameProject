using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("상호 작용 설정")]
    public float interactionRange = 2.0f;       //범위
    public LayerMask interactionLayerMask = 1;  //레이어
    public KeyCode interactionKey = KeyCode.E;

    [Header("UI 설정")]
    public Text interactionText;                //UI텍스트
    public GameObject interactionUI;            //UI패널

    private Transform playerTransform;
    private InteractableObject currentInteractiable;
    void Start()
    {
        playerTransform = transform;
        HideInteractionUI();
    }

    void Update()
    {
        CheckForInteactacbles();
        HandleinteractionInput();
    }

    void CheckForInteactacbles()
    {
        Vector3 CheckPosition = playerTransform.position + playerTransform.forward * (interactionRange * 0.5f);
        Collider[] hitColliders = Physics.OverlapSphere(CheckPosition, interactionRange, interactionLayerMask);
        InteractableObject closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            InteractableObject interactable = collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);
                Vector3 directionToObject = (collider.transform.position - playerTransform.position).normalized;
                float angle = Vector3.Angle(playerTransform.forward, directionToObject);

                if (angle < 90f && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != currentInteractiable)
        {
            if (currentInteractiable != null)
            {
                currentInteractiable.onPlayerExit();
            }

            currentInteractiable = closestInteractable;

            if (currentInteractiable != null)
            {
                currentInteractiable.onPlayerEnter();
                ShowinteractionUI(currentInteractiable.GetInteractionText());
            }
            else
            {
                HideInteractionUI();
            }
        }
    }

    void HandleinteractionInput()
    {
        if (currentInteractiable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractiable.Interact();
        }
    }

    void ShowinteractionUI(string text)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        if (interactionText != null)
        {
            interactionText.text = text;
        }
    }

    void HideInteractionUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }
}
