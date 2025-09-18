using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractionController : MonoSingleton<InteractionController>
{
    [Header("拾取设置")]
    [Tooltip("交互按键")]
    [SerializeField] private KeyCode pickupKey = KeyCode.E;

    //[Header("音效设置")]
    //[SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip equipSound;

    [Header("其他设置")]
    [Tooltip("手持位置")]
    [SerializeField] private Transform handTransform;
    public Transform HandTransform => handTransform;

    // 事件系统
    public delegate void ItemEquippedHandler(ItemPickupHandler item);
    public event ItemEquippedHandler OnItemEquipped;


    public ItemPickupHandler currentItem;//当前拾取的物品
    public bool IsHoldingItem => currentItem != null;//是否持有物品

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
            OnInteractionKeyPress();
        if (Input.GetMouseButtonDown(0))
            fire();
    }
    private void OnInteractionKeyPress()//按下交互键，可以采用InputSystem来实现，不过此处使用KeyCode实现
    {
        if (currentItem!=null)
        {
            if (currentItem.item.CanBeStoredInBag)
            {
                IntoBag();
            }
        }
        InteractionManager.Instance.ConfirmInteraction(currentItem);
        //PlayEquipmentSound();
    }
    private void fire()//按下左键，可以采用InputSystem来实现，不过此处使用KeyCode实现
    {
        currentItem?.interacter?.OnFire();
        //PlaySound();
    }
    public void NotifyItemEquipped(ItemPickupHandler item)//装备物品
    {
        if (item.IsDestroyed())
        {
            currentItem = null;
            return;
        }
        currentItem = item;
        // 预留背包系统接口
        // inventory?.UpdateItem(item);
        IntoBag();
        // 触发装备事件
        OnItemEquipped?.Invoke(item);
    }
    public void IntoBag()//进入背包
    {
        if (currentItem.item.CanBeStoredInBag)
        {
            // 预留背包系统接口
            // inventory?.AddItem(currentItem);
            Inventory.Instance?.AddItem(currentItem.item);
            Destroy(currentItem.gameObject);
            currentItem = null;
            Debug.Log("收入背包");
        }
        
    }

    internal void DropItem()//丢弃物品
    {
        currentItem = null;
    }

    //private void PlayEquipmentSound()
    //{
    //    if (equipSound != null && audioSource != null)
    //        audioSource.PlayOneShot(equipSound);
    //}

}