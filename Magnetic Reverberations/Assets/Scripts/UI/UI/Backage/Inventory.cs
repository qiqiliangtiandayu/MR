using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public List<InventoryItem> items;
    private int MaxCapacity; // 背包容量

    public void Init()
    {
        items = new List<InventoryItem>();
        MaxCapacity = 10;
    }

    public bool AddItem(Item newItem)
    {
        if (newItem.CanBeStoredInBag)
        {
            // 已有的可堆叠物品处理
            var item = items.Find(i =>
                i.itemData == newItem /*&& 
                i.stackCount < i.itemData.MaxStack*/
            );

            if (item != null)
            {
                item.stackCount++;
                Debug.Log("已有的可堆叠物品处理");
                return true;
            }

            // 新物品或不可堆叠添加
            if (items.Count < MaxCapacity)
            {
                items.Add(new InventoryItem
                {
                    itemData = newItem,
                    stackCount = 1,
                    instanceID = System.Guid.NewGuid().ToString(),
                });
                Debug.Log("新物品或不可堆叠添加");
                return true;
            }
            Debug.Log("背包满了");
            return false;
        }
        Debug.Log("物品不可以进入背包");
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
    public string instanceID; // 唯一实例ID
    public Item itemData; // 物品基础数据
    public int stackCount; // 堆叠数量
}
