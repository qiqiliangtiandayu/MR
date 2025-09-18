using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

namespace Common.Data
{
    public class baseStateDefine
    {
        public string Name { get; set; } // 状态名称
        public string Description { get; set; } // 状态描述
        public string Copywriting { get; set; } // 文案
        public int Weight { get; set; } // 状态权重
        public float StateTime { get; set; } // 状态持续时间
        public float CD { get; set; } // 冷却时间
        public float Restore { get; set; } // 状态恢复
        public int CorrelationID { get; set; } // 关联ID
        public int NextID { get; set; } // 下一个状态ID
        public int PrevID { get; set; } // 上一个状态ID
        public int Buff { get; set; } // 对应BuffID
        public string Audio { get; set; } // 音效路径
        public string Icon { get; set; } // 图标路径
    }
        public class StateDefine:baseStateDefine
    {
        public int ID { get; set; } // 状态ID
        public float HP { get; set; } // 生命值
        public float HealthRecover { get; set; } // 生命值恢复速度
        public float Attack { get; set; } // 近战攻击力
        public float Defense { get; set; } // 防御力
        public float RemoteDiscrete { get; set; } // 远程离散程度
        public float RangedAttack { get; set; } // 远程攻击力
        public float Endurance  { get; set; }  // 耐力条
        public float RecoverSpeed { get; set; }  // 耐力恢复速度
        public float BaseSpeeed { get; set; } // 基础速度
        public float RunSpeed { get; set; } // 奔跑倍率系数
        public float RunDeplete { get; set; } // 奔跑消耗耐力
        public float JumpHeight { get; set; } // 跳跃高度
        public float JumpDeplete { get; set; } // 跳跃消耗耐力
        public float JumpSpeed { get; set; } // 跳跃速度
        public float SquatRecoverSpeed { get; set; } // 蹲下回复耐力系数
        public float SquatTime { get; set; } // 蹲下时间


        public override string ToString()
        {
            return $"状态ID: {ID}, 名称: {Name}, 描述: {Description}, " +
                   $"文案: {Copywriting}, 权重: {Weight}, " +
                   $"状态持续时间: {StateTime}, 耐力条: {Endurance}, " +
                   $"恢复速度: {RecoverSpeed}, 基础速度: {BaseSpeeed}, " +
                   $"跑步速度: {RunSpeed}, 跑步消耗: {RunDeplete}, " +
                   $"跳跃高度: {JumpHeight}, 跳跃消耗: {JumpDeplete}, " +
                   $"跳跃速度: {JumpSpeed} "+
                   $"下蹲恢复速度: {SquatRecoverSpeed}," +
                   $"下蹲时间: {SquatTime},  " +
                   $"上一个状态ID: {PrevID} , 下一个状态ID: {NextID}," +
                   $" Buff: {Buff}, " +
                   $"冷却时间: {CD},  " +
                   $"图标: {Icon},音频: {Audio}" +
                   $" }}";
        }
    }
}