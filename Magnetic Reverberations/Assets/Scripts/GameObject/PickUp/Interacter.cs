using System;
using UnityEngine;
public class Interacter : MonoBehaviour
{
    // 通用交互方法
    public bool Interactive(Item item)
    {
        switch (item.type)
        {
            case ItemType.Environment:
                return OpenContainer();
            case ItemType.Consumable:
                return UseConsumable();
        }
        return false;
    }
    internal void OnFire()
    {
        Debug.Log("使用当前物品");
    }
    // 拆卸逻辑
    public virtual bool Dismantle(Item item)
    {
        Debug.Log($"Dismantling {item.type}");
        // 实现具体拆卸逻辑
        return false;
    }

    // 装配逻辑
    public virtual bool Assemble(Item item)
    {
        Debug.Log($"Assembling {item.type}");
        // 实现具体装配逻辑
        return false;
    }

    public virtual bool OpenContainer() { /* 开箱逻辑 */  return false; }
    public virtual bool UseConsumable() { /* 使用消耗品 */  return false; }


}