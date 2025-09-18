using JetBrains.Annotations;
using System;
using System.Runtime.InteropServices;
using UnityEditor;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

namespace Common.Data
{
    public class baseStateDefine
    {
        public string Name { get; set; } // ״̬����
        public string Description { get; set; } // ״̬����
        public string Copywriting { get; set; } // �İ�
        public int Weight { get; set; } // ״̬Ȩ��
        public float StateTime { get; set; } // ״̬����ʱ��
        public float CD { get; set; } // ��ȴʱ��
        public float Restore { get; set; } // ״̬�ָ�
        public int CorrelationID { get; set; } // ����ID
        public int NextID { get; set; } // ��һ��״̬ID
        public int PrevID { get; set; } // ��һ��״̬ID
        public int Buff { get; set; } // ��ӦBuffID
        public string Audio { get; set; } // ��Ч·��
        public string Icon { get; set; } // ͼ��·��
    }
        public class StateDefine:baseStateDefine
    {
        public int ID { get; set; } // ״̬ID
        public float HP { get; set; } // ����ֵ
        public float HealthRecover { get; set; } // ����ֵ�ָ��ٶ�
        public float Attack { get; set; } // ��ս������
        public float Defense { get; set; } // ������
        public float RemoteDiscrete { get; set; } // Զ����ɢ�̶�
        public float RangedAttack { get; set; } // Զ�̹�����
        public float Endurance  { get; set; }  // ������
        public float RecoverSpeed { get; set; }  // �����ָ��ٶ�
        public float BaseSpeeed { get; set; } // �����ٶ�
        public float RunSpeed { get; set; } // ���ܱ���ϵ��
        public float RunDeplete { get; set; } // ������������
        public float JumpHeight { get; set; } // ��Ծ�߶�
        public float JumpDeplete { get; set; } // ��Ծ��������
        public float JumpSpeed { get; set; } // ��Ծ�ٶ�
        public float SquatRecoverSpeed { get; set; } // ���»ظ�����ϵ��
        public float SquatTime { get; set; } // ����ʱ��


        public override string ToString()
        {
            return $"״̬ID: {ID}, ����: {Name}, ����: {Description}, " +
                   $"�İ�: {Copywriting}, Ȩ��: {Weight}, " +
                   $"״̬����ʱ��: {StateTime}, ������: {Endurance}, " +
                   $"�ָ��ٶ�: {RecoverSpeed}, �����ٶ�: {BaseSpeeed}, " +
                   $"�ܲ��ٶ�: {RunSpeed}, �ܲ�����: {RunDeplete}, " +
                   $"��Ծ�߶�: {JumpHeight}, ��Ծ����: {JumpDeplete}, " +
                   $"��Ծ�ٶ�: {JumpSpeed} "+
                   $"�¶׻ָ��ٶ�: {SquatRecoverSpeed}," +
                   $"�¶�ʱ��: {SquatTime},  " +
                   $"��һ��״̬ID: {PrevID} , ��һ��״̬ID: {NextID}," +
                   $" Buff: {Buff}, " +
                   $"��ȴʱ��: {CD},  " +
                   $"ͼ��: {Icon},��Ƶ: {Audio}" +
                   $" }}";
        }
    }
}