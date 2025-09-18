using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public List<InventoryItem> items;
    private int MaxCapacity; // ��������

    public void Init()
    {
        items = new List<InventoryItem>();
        MaxCapacity = 10;
    }

    public bool AddItem(Item newItem)
    {
        if (newItem.CanBeStoredInBag)
        {
            // ���еĿɶѵ���Ʒ����
            var item = items.Find(i =>
                i.itemData == newItem /*&& 
                i.stackCount < i.itemData.MaxStack*/
            );

            if (item != null)
            {
                item.stackCount++;
                Debug.Log("���еĿɶѵ���Ʒ����");
                return true;
            }

            // ����Ʒ�򲻿ɶѵ����
            if (items.Count < MaxCapacity)
            {
                items.Add(new InventoryItem
                {
                    itemData = newItem,
                    stackCount = 1,
                    instanceID = System.Guid.NewGuid().ToString(),
                });
                Debug.Log("����Ʒ�򲻿ɶѵ����");
                return true;
            }
            Debug.Log("��������");
            return false;
        }
        Debug.Log("��Ʒ�����Խ��뱳��");
        return false;
    }

    public bool RemoveItem(string instanceID)
    {
        var item = items.Find(i => i.instanceID == instanceID);
        if (item != null)
        {
            item.stackCount--;
            if (item.stackCount == 0)
            {
                items.Remove(item);
            }
            return true;
        }
        return false;
    }

    //public void SaveInventory()
    //{
    //    string inventoryJson = JsonUtility.ToJson(this);
    //    PlayerPrefs.SetString("InventoryData", inventoryJson);
    //    PlayerPrefs.Save();
    //}

    //public List<InventoryItem> LoadInventory()
    //{
    //    if (PlayerPrefs.HasKey("InventoryData"))
    //    {
    //        string inventoryJson = PlayerPrefs.GetString("InventoryData");
    //        items = JsonUtility.FromJson<Inventory>(inventoryJson).items;
    //        return items;
    //    }
    //    else
    //    {
    //        items = new List<InventoryItem>();
    //        return items;
    //    }
    //}
}

[System.Serializable]
public class InventoryItem
{
    public string instanceID; // Ψһʵ��ID
    public Item itemData; // ��Ʒ��������
    public int stackCount; // �ѵ�����
}
