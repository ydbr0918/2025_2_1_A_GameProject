using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{

    [Header("��ȣ �ۿ� ����")]
    public float interactionRange = 2.0f;                          //��ȣ�ۿ� ����
    public LayerMask interactionLayerMask = 1;                     //��ȣ�ۿ��� ���̾�
    public KeyCode interactionKey = KeyCode.E;                     //��ȣ�ۿ�Ű (EŰ)

    [Header("UI ����")]
    public Text interactionText;                             //��ȣ�ۿ� UI �ؽ�Ʈ
    public GameObject interactionUI;                                  //��ȣ�ۿ� UI �г�

    private Transform playerTransform;
    private InteractableObject currentInteractable;                  //������ ������Ʈ ��� Ŭ����

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        HideInteractionUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
    }

    void CheckForInteractables()
    {
        Vector3 checkPosition = playerTransform.position + playerTransform.forward * (interactionRange * 0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, interactionRange, interactionLayerMask);

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

        if (closestInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnPlayerExit();
            }

            currentInteractable = closestInteractable;

            if (currentInteractable != null)
            {
                currentInteractable.OnPlayerEnter();
                ShowInteractionUI(currentInteractable.GetInteractionText());
            }
            else
            {
                HideInteractionUI();
            }
        }
    }

    void HandleInteractionInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.Interact();
        }
    }

    void ShowInteractionUI(string text)
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
