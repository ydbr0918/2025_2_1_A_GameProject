
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Item")]

public class ItemData : ScriptableObject
{
    public string itemName;                   //������ �̸�
    public Sprite itemIcon;           //������ ������ �̹���
    public int maxStack = 99;                 //�ִ� ��Ĩ ����
}
