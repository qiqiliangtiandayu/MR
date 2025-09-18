using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackage : MonoSingleton<UIBackage>
{
    [SerializeField] private GameObject cardPrefab;

    [Tooltip("������ʾ�ĽǶȷ�Χ")]
    [SerializeField] private float angleRange;
    [Tooltip("����չ�����εİ뾶")]
    [SerializeField] private float radius;

    private List<UICard> uIcards = new List<UICard>();

    void Start()
    {
        Inventory.Instance.Init();
        CloseBag();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (uIcards.Count == 0)
            {
                OpenBag();
            }
            else
            {
                CloseBag();
            }
        }
    }

    private int selectIdx;//��ǰѡ����Ʒ�����
    public int SelectIdx
    {
        get { return selectIdx; }
        set
        {
            selectIdx = value;
            for (int i = 0; i < uIcards.Count; i++)
            {
                // ����ѡ���벻ѡ��״̬
                //.gameObject.SetActive(i == selectIdx);
            }
        }
    }



    public Item CloseBag()
    {
        if (uIcards.Count == 0) return null;

        Item item = uIcards[selectIdx].itemData;
        foreach (var uIcard in uIcards)
        {
            Destroy(uIcard.gameObject);
        }
        uIcards.Clear();
        selectIdx = 0;
        return item;
    }

    public void OpenBag()
    {
        // ��ȡ��Ҫչʾ����Ʒ�б�
        List<InventoryItem> items = new List<InventoryItem>();
        int count = 0;
        foreach (var item in Inventory.Instance.items)
        {
            if (item.itemData != null /*&& item.itemData.type == 0*/)
            {
                items.Add(item);    // ��ӵ����б�
                count += item.stackCount; // ͬʱ�ۼ�
            }
        }


        if (count == 0) return;
        if (count == 1)
        {
            // ʵ������Ƭ������λ�á���ת�Ͳ㼶��ʾ
            GameObject card = Instantiate(cardPrefab, transform);
            //card.transform.localPosition = new Vector3(0, 0, 0);
            //card.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //card.GetComponent<SpriteRenderer>().sortingOrder = 0;

            // ��ʼ����Ƭ
            UICard uICard = card.GetComponent<UICard>();
            uICard.Init(items[0].itemData, 0);
            uIcards.Add(uICard);
            return;
        }


        float startAngle = -angleRange / 2; // ��һ�ſ�Ƭ�Ƕ�
        float angleInterval = angleRange / (count - 1);
        int selectIdx = 0;
        foreach (var item in items)
        {
            for (int i = 0; i < item.stackCount; i++)
            {
                // ���㵱ǰ��Ƭ�ǶȺ�λ��
                float angle = startAngle + selectIdx * angleInterval;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad) - 1, 0) * radius;

                // ʵ������Ƭ������λ�á���ת�Ͳ㼶��ʾ
                GameObject card = Instantiate(cardPrefab, transform);
                //card.transform.localPosition = pos;
                //card.transform.localRotation = Quaternion.Euler(0, 0, -angle);
                //card.GetComponent<SpriteRenderer>().sortingOrder = selectIdx;

                // ��ʼ����Ƭ
                UICard uICard = card.GetComponent<UICard>();
                uICard.Init(items[0].itemData, 0);
                uIcards.Add(uICard);
                selectIdx++;
            }
        }

        SelectIdx = 0;
    }
}
