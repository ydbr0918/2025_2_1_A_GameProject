using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("상호작용 정보")]
    public string objectName = "아이템 ";
    public string interactionText = "[E} 상호작용";
    public InteractionType interactionType = InteractionType.item;

    [Header("하이라이트 설정")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    private Color originalColor;
    private bool isHighlighted = false;


    public enum InteractionType
    {
        item,
        Machine,
        Building,
        NPC,
        Collectible
    }

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;
    }

    public virtual void onPlayerEnter()
    {
        HighlightObject();
    }

    public virtual void onPlayerExit()
    {
        RemoveHighlight();
    }

    public virtual void Interact()
    {
        switch (interactionType)
        {
            case InteractionType.item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Building:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;

        }
    }

    protected virtual void HighlightObject()
   {
        if (objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighlighted = true;
        }
   }

   protected virtual void RemoveHighlight()
   {
        if (objectRenderer != null && isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
   }
    
       
   protected virtual void CollectItem()
   {
        Debug.Log($"{objectName}을(를) 획득했습니다");
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
   }
    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}을 작동시켰습니다");
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
    }
    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}와 대화를 시작합니다");
        transform.Rotate(Vector3.up * 90f);
        
    }

    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}와 대화를 시작합니다");
    }

    public string GetInteractionText()
    {
        return interactionText;
    }


}
