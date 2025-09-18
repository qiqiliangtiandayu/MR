using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

namespace Common.Data
{
    public interface IMoveDefine
    {
        public int Endurance { get; set; }  // ������
        public int RecoverSpeed { get; set; }  // �����Զ��ָ��ٶ�
        public float BaseSpeed { get; set; } // �����ٶ�
        public float Acceleration { get; set; } // ���ٶ�
        public float RunSpeed { get; set; } // ���ܱ���ϵ��
        public int RunDeplete { get; set; } // ������������
        public float JumpSpeed { get; set; } // ��Ծ�ٶ�
        public float JumpHeight { get; set; } // ��Ծ�߶�
        public int JumpDeplete { get; set; } // ��Ծ��������
        public int SquatRecoverSpeed { get; set; } // �������������ظ�
        public float SquatTime { get; set; } // ����ʱ��
        public float SquatHeight { get; set; } // ���¸߶�
        public float SquatSpeed { get; set; } // ����ʱ�ٶ�
        public float FallDamageHeight { get; set; } // ׹��߶�,���ڼ����˺������Լ��������
    } 
    public class MoveDefine:IMoveDefine
    {
        public int Endurance { get; set; }  // ������
        public int RecoverSpeed { get; set; }  // �����Զ��ָ��ٶ�
        public float BaseSpeed { get; set; } // �����ٶ�
        public float Acceleration { get; set; } // ���ٶ�
        public float RunSpeed { get; set; } // ���ܱ���ϵ��
        public int RunDeplete { get; set; } // ������������
        public float JumpSpeed { get; set; } // ��Ծ�ٶ�
        public float JumpHeight { get ; set ; }// ��Ծ�߶�
        public int JumpDeplete { get; set; } // ��Ծ��������
        public int SquatRecoverSpeed { get; set; } // �������������ظ�
        public float SquatTime { get; set; } // ����ʱ��
        public float SquatHeight { get; set; } // ���¸߶�
        public float SquatSpeed { get; set; } // ����ʱ�ٶ�
        public float FallDamageHeight { get; set; } // ׹��߶�,���ڼ����˺������Լ��������
        

        public override string ToString()
        {
            return
            $" ������: {Endurance}, " +
            $"�ָ��ٶ�: {RecoverSpeed}, �����ٶ�: {BaseSpeed}, " +
            $"�ܲ��ٶ�: {RunSpeed}, �ܲ�����: {RunDeplete}, " +
            $"��Ծ�ٶ�: {JumpSpeed}, ��Ծ����: {JumpDeplete}, " +
            $"�¶׻ָ��ٶ�: {SquatRecoverSpeed}, " +
            $"�¶�ʱ��: {SquatTime}, " +
            $"�¶׸߶�: {SquatHeight}, " +
            $"�¶��ٶ�: {SquatSpeed}, " +
            $"׹��߶�: {FallDamageHeight}, ";
        }
    }
    public class CharacterDefine: BaseDefine, IMoveDefine
    {
        
        public string Type { get; set; } // ����
        public string AI { get; set; } // AI����
        public float HP { get; set; } // ����ֵ
        public float HealthRecover { get; set; } // ����ֵ�Զ��ָ��ٶȣ����룬ÿ��ظ�һ������ֵ��
        public int Attack { get; set; } // ��ս������
        public int Defense { get; set; } // ������
        public float RemoteDiscrete { get; set; } // Զ�̹�����ɢ�̶�
        public int RangedAttack { get; set; } // Զ�̹�����
        public int Endurance { get; set; }  // ������
        public int RecoverSpeed { get; set; }  // �����Զ��ָ��ٶ�
        public float BaseSpeed { get; set; } // �����ٶ�
        public float Acceleration { get; set; } // ���ٶ�
        public float RunSpeed { get; set; } // ���ܱ���ϵ��
        public int RunDeplete { get; set; } // ������������
        public float JumpSpeed { get; set; } // ��Ծ�ٶ�
        public float JumpHeight { get; set; }// ��Ծ�߶�
        public int JumpDeplete { get; set; } // ��Ծ��������
        public int SquatRecoverSpeed { get; set; } // �������������ظ�
        public float SquatTime { get; set; } // ����ʱ��
        public float SquatHeight { get; set; } // ���¸߶�
        public float SquatSpeed { get; set; } // ����ʱ�ٶ�
        public float FallDamageHeight { get; set; } // ׹��߶�,���ڼ����˺������Լ��������
        public override string ToString()
        {
            return $"״̬ID: {ID}, ����: {Name}, ����: {Description}, " +
            $"�İ�: {Copywriting}, ";
        }
    }
}