using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("���� ����")]
    public int playerScore = 0;
    public int itemsCollected = 0;

    [Header("UI ����")]
    public Text scoreText;
    public Text itemCountText;
    public Text gameStatusText;

    public static GameManager Instance;    //�̤��Ѥ��� ���ͤ�

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);                  //�� ��ȣ������ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log($"������ ����!(��:{itemsCollected}��");
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "����:" + playerScore;
        }
        if (itemCountText != null)
        {
            itemCountText.text= "������:" + itemsCollected + "��";
        }
    }
}
