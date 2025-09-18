using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBackage : MonoSingleton<UIBackage>
{
    [SerializeField] private GameObject cardPrefab;

    [Tooltip("卡牌显示的角度范围")]
    [SerializeField] private float angleRange;
    [Tooltip("卡牌展开扇形的半径")]
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

    private int selectIdx;//当前选中物品的序号
    public int SelectIdx
    {
        get { return selectIdx; }
        set
        {
            selectIdx = value;
            for (int i = 0; i < uIcards.Count; i++)
            {
                // 设置选中与不选中状态
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
        // 获取需要展示的物品列表
        List<InventoryItem> items = new List<InventoryItem>();
        int count = 0;
        foreach (var item in Inventory.Instance.items)
        {
            if (item.itemData != null /*&& item.itemData.type == 0*/)
            {
                items.Add(item);    // 添加到新列表
                count += item.stackCount; // 同时累加
            }
        }


        if (count == 0) return;
        if (count == 1)
        {
            // 实例化卡片并设置位置、旋转和层级显示
            GameObject card = Instantiate(cardPrefab, transform);
            //card.transform.localPosition = new Vector3(0, 0, 0);
            //card.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //card.GetComponent<SpriteRenderer>().sortingOrder = 0;

            // 初始化卡片
            UICard uICard = card.GetComponent<UICard>();
            uICard.Init(items[0].itemData, 0);
            uIcards.Add(uICard);
            return;
        }


        float startAngle = -angleRange / 2; // 第一张卡片角度
        float angleInterval = angleRange / (count - 1);
        int selectIdx = 0;
        foreach (var item in items)
        {
            for (int i = 0; i < item.stackCount; i++)
            {
                // 计算当前卡片角度和位置
                float angle = startAngle + selectIdx * angleInterval;
                float rad = angle * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(Mathf.Sin(rad), Mathf.Cos(rad) - 1, 0) * radius;

                // 实例化卡片并设置位置、旋转和层级显示
                GameObject card = Instantiate(cardPrefab, transform);
                //card.transform.localPosition = pos;
                //card.transform.localRotation = Quaternion.Euler(0, 0, -angle);
                //card.GetComponent<SpriteRenderer>().sortingOrder = selectIdx;

                // 初始化卡片
                UICard uICard = card.GetComponent<UICard>();
                uICard.Init(items[0].itemData, 0);
                uIcards.Add(uICard);
                selectIdx++;
            }
        }

        SelectIdx = 0;
    }
}
