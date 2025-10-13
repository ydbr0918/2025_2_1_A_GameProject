using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiverNPC : InteractableObject
{
    [Header("NPC Quest Settings")]
    public QuestData questToGive;
    public string npcName = "NPC";
    public string questStartMessage = "���ο� ����Ʈ�� �ֽ��ϴ�";
    public string noQuestMessage = "����Ʈ�� �����ϴ�";
    public string QuestAlreadyActiveMessage = "�̹� �������� ����Ʈ�� �ֽ��ϴ�";

    private QuestManager questManager;


    protected override void Start()
    {
        base.Start();
        questManager = FindObjectOfType<QuestManager>();

        if (questManager == null)
        {
            Debug.LogError("QuestManager �� �����ϴ�.");
        }
        interactionText = "[E]" + npcName + "�� ��ȭ�ϱ�";
    }


    public override void Interact()
    {
        base.Interact();

        questManager.StartQuest(questToGive);
    }

    private void Update()
    {
        if (questToGive != null && questManager != null && questManager.currentQuest == null)
        {
            interactionText = "[E]" + npcName + " �� ��ȭ�ϱ�";
        }
        else if (questManager != null && questManager.currentQuest != null)
        {
            interactionText = "[E]" + npcName;
        }
    }
}
