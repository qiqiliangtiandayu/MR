using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

namespace Common.Data
{
    public interface IMoveDefine
    {
        public int Endurance { get; set; }  // 耐力条
        public int RecoverSpeed { get; set; }  // 耐力自动恢复速度
        public float BaseSpeed { get; set; } // 基础速度
        public float Acceleration { get; set; } // 加速度
        public float RunSpeed { get; set; } // 奔跑倍率系数
        public int RunDeplete { get; set; } // 奔跑消耗耐力
        public float JumpSpeed { get; set; } // 跳跃速度
        public float JumpHeight { get; set; } // 跳跃高度
        public int JumpDeplete { get; set; } // 跳跃消耗耐力
        public int SquatRecoverSpeed { get; set; } // 蹲下缩减耐力回复
        public float SquatTime { get; set; } // 蹲下时间
        public float SquatHeight { get; set; } // 蹲下高度
        public float SquatSpeed { get; set; } // 蹲下时速度
        public float FallDamageHeight { get; set; } // 坠落高度,用于计算伤害比例以及负伤情况
    } 
    public class MoveDefine:IMoveDefine
    {
        public int Endurance { get; set; }  // 耐力条
        public int RecoverSpeed { get; set; }  // 耐力自动恢复速度
        public float BaseSpeed { get; set; } // 基础速度
        public float Acceleration { get; set; } // 加速度
        public float RunSpeed { get; set; } // 奔跑倍率系数
        public int RunDeplete { get; set; } // 奔跑消耗耐力
        public float JumpSpeed { get; set; } // 跳跃速度
        public float JumpHeight { get ; set ; }// 跳跃高度
        public int JumpDeplete { get; set; } // 跳跃消耗耐力
        public int SquatRecoverSpeed { get; set; } // 蹲下缩减耐力回复
        public float SquatTime { get; set; } // 蹲下时间
        public float SquatHeight { get; set; } // 蹲下高度
        public float SquatSpeed { get; set; } // 蹲下时速度
        public float FallDamageHeight { get; set; } // 坠落高度,用于计算伤害比例以及负伤情况
        

        public override string ToString()
        {
            return
            $" 耐力条: {Endurance}, " +
            $"恢复速度: {RecoverSpeed}, 基础速度: {BaseSpeed}, " +
            $"跑步速度: {RunSpeed}, 跑步消耗: {RunDeplete}, " +
            $"跳跃速度: {JumpSpeed}, 跳跃消耗: {JumpDeplete}, " +
            $"下蹲恢复速度: {SquatRecoverSpeed}, " +
            $"下蹲时间: {SquatTime}, " +
            $"下蹲高度: {SquatHeight}, " +
            $"下蹲速度: {SquatSpeed}, " +
            $"坠落高度: {FallDamageHeight}, ";
        }
    }
    public class CharacterDefine: BaseDefine, IMoveDefine
    {
        
        public string Type { get; set; } // 类型
        public string AI { get; set; } // AI类型
        public float HP { get; set; } // 生命值
        public float HealthRecover { get; set; } // 生命值自动恢复速度（存秒，每秒回复一点生命值）
        public int Attack { get; set; } // 近战攻击力
        public int Defense { get; set; } // 防御力
        public float RemoteDiscrete { get; set; } // 远程攻击离散程度
        public int RangedAttack { get; set; } // 远程攻击力
        public int Endurance { get; set; }  // 耐力条
        public int RecoverSpeed { get; set; }  // 耐力自动恢复速度
        public float BaseSpeed { get; set; } // 基础速度
        public float Acceleration { get; set; } // 加速度
        public float RunSpeed { get; set; } // 奔跑倍率系数
        public int RunDeplete { get; set; } // 奔跑消耗耐力
        public float JumpSpeed { get; set; } // 跳跃速度
        public float JumpHeight { get; set; }// 跳跃高度
        public int JumpDeplete { get; set; } // 跳跃消耗耐力
        public int SquatRecoverSpeed { get; set; } // 蹲下缩减耐力回复
        public float SquatTime { get; set; } // 蹲下时间
        public float SquatHeight { get; set; } // 蹲下高度
        public float SquatSpeed { get; set; } // 蹲下时速度
        public float FallDamageHeight { get; set; } // 坠落高度,用于计算伤害比例以及负伤情况
        public override string ToString()
        {
            return $"状态ID: {ID}, 名称: {Name}, 描述: {Description}, " +
            $"文案: {Copywriting}, ";
        }
    }
}