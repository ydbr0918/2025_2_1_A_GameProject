using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUsePopup : MonoBehaviour
{
    public static ItemUsePopup instance;

    public GameObject popupPanel;
    public Text itemNameText;
    public Image itemIconImage;
    public Button useButton;
    public Button closeButton;

    private ItemData currentItem;
    private InventorySlot currentSlot;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        popupPanel.SetActive(false);
        useButton.onClick.AddListener(UseItem);
        closeButton.onClick.AddListener(ClosePopup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopup(ItemData item, InventorySlot slot)        //팝업 세팅 함수
    {
        currentItem = item;                                         //클릭한 아이템 데이터 모두 가져오기
        currentSlot = slot;                                         //슬롯 정보도 가져온다

        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.itemIcon;

        useButton.interactable = item.isUsable;

        popupPanel.SetActive(true);
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    void UseItem()
    {
        if (currentItem.isUsable)
        {
            PlayerStatus player = FindObjectOfType<PlayerStatus>();

            if (currentItem.healAmount > 0)
            {
                player.Heal(currentItem.healAmount);
                Debug.Log(currentItem.itemIcon + " 사용 : 체력 회복 "+ currentItem.healAmount);
            }
            else if (currentItem.healAmount < 0)
            {
                player.TakeDamage(currentItem.healAmount);
                //Debug.Log(currentItem.itemIcon + " 사용 : 체력 감소 " + currentI)
            }
            currentSlot.RemoveAmount(1);

        }
        ClosePopup();
    }
}
