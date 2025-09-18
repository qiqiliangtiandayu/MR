using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    public enum ItemType
    {
        None,//空
        Environmental,//环境物品
        Equipment,//装备
        Material,//材料
        Tool,//工具
        QuestItem,//任务物品
        Treasure,//收藏品，彩蛋
        SkillItem,//技能物品
    }

    public enum ItemState
    {
        Active,//激活
        Inactive,//未激活
        Held,//持有
    }

    public enum InteractionResult
    {
        Success,//成功
        Failed_OutOfRange,//超出范围
        Failed_InvalidState//无效状态
    }
    // ScriptableObject存储标签配置
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Items/ItemConfig")]
    public class ItemConfig : ScriptableObject
    {
        public string itemID;
        public ItemType type;
        public LayerMask interactionLayers;
        public List<string> tags = new List<string>(); // 支持多标签
    }

    public class ItemDefine
    {
        public int ID { get; set; } // 物品ID
        public string Name { get; set; } // 物品名称
        public string Description { get; set; } // 物品描述
        public ItemType ItemType { get; set; } // 物品类型
        public int ATK { get; set; } // 攻击力
        public string HurtType { get; set; } // 伤害类型
        public float Hurt { get; set; } // 伤害值
        public int Buff { get; set; } //对应BuffID
        public int CD { get; set; } // 使用冷却时间
        public string Audio { get; set; } // 音效路径
        public string Icon { get; set; } // 图标路径
        public string Copywriting { get; set; } // 文案

        public override string ToString()
        {
            return $"ID: {ID}, Name: {Name}, Description: {Description}，"+
                   $" ItemType: {ItemType}, ATK: {ATK}, Hurt: {Hurt}, " +
                   $"Buff: {Buff}, CD: {CD}, Audio: {Audio}, Icon: {Icon}, Iconbig: {Copywriting}";
        }
    }
}