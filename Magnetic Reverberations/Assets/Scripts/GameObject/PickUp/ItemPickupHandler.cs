using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

/// <summary>
/// 后续添加接口，使用策略模式，可扩展性更好
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ItemPickupHandler : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    // 交互配置
    [Header("订阅事件")]
    [Tooltip("物品装备/卸载事件")]
    public UnityEvent OnEquipped;// 装备事件
    public UnityEvent OnDropped;// 卸载事件

    [Header("物品数据")]
    [Tooltip("物品关联数据")]
    public Item item; // 物品数据对象
    public Interacter interacter; // 交互处理器
    public string showText;// 展示文本,之后放入item
    public float surfaceOffset;// 表面偏移

    // 运行时状态
    private Transform originalParent;// 物品原来的父物体
    private bool isPickedUp;// 是否被拾取
    private bool isEquipped;// 是否被装备
    public bool IsPickedUp => isPickedUp;// 对外调用
    public bool IsEquipped => isEquipped;// 对外调用
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        originalParent = transform.parent;
    }
    private void OnDestroy()
    {
        OnEquipped.RemoveAllListeners();
        OnDropped.RemoveAllListeners();
    }
    public bool PickUp(Transform parentTransform)// 统一交互入口
    {
        if (item == null) return false;

        if (item.CanPickUp)
        {
            return ExecutePickUp(parentTransform);
        }
        else
        {
            return (interacter?.Interactive(item) == true); // 直接交互
        }
    }
    public void DropDown(Vector3 position)// 统一放置入口
    {
        if (item == null) return;

        if (item.CanEquip)
        {
            ExecuteDrop(position);
        }
        else
        {
            interacter?.Interactive(item); // 环境交互
        }
    }
    private bool ExecutePickUp(Transform targetParent)//拾取
    {
        if (interacter != null && isEquipped)// 物品拆解
            if (interacter.Dismantle(item) == true) 
            {
                isEquipped = false;
            }
        // 物理状态切换
        rb.isKinematic = true;
        rb.detectCollisions = false;
        col.enabled = false;

        // 层级关系管理
        transform.SetParent(targetParent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isPickedUp = true;
        OnEquipped?.Invoke();
        return true;
    }
    private void ExecuteDrop(Vector3 position)//放下
    {
        if (interacter != null && !isEquipped)
            if (interacter.Assemble(item) == true)
            {
                isEquipped = true;
                return;
            }
        // 恢复物理特性
        rb.isKinematic = false;
        rb.detectCollisions = true;
        col.enabled = true;

        // 位置重置
        transform.SetParent(originalParent);
        transform.position = position;
        transform.rotation = Quaternion.identity;

        isPickedUp = false;
        OnDropped?.Invoke();
    }
}