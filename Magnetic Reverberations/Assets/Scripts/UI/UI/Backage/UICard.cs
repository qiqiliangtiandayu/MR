using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public Item itemData;
    private int selectIdx;
    [SerializeField] private Text nameText;

    public void Init(Item itemData, int selectIdx)
    {
        this.itemData = itemData;
        this.selectIdx = selectIdx;

        // 其他初始化内容
        nameText.name = itemData.ToString();
    }

    public void OnClick()
    {
        UIBackage.Instance.SelectIdx = selectIdx;
    }
}
