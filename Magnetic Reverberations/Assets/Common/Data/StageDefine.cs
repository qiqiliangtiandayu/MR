using JetBrains.Annotations;
using System;

namespace Common.Data
{
    public enum LevelType 
    {
        None = 0,

    }

    public enum LimitType
    {
        None = 0,

    }

    public enum GameLevel
    { 
        None = 0,
    }


    public class StageDefine
    {     //�������ݿ�����ϲ�
        public int ID { get; set; } // �ؿ�ID
        public string Name { get; set; } // �ؿ�����
        public string Description { get; set; } // �ؿ�����
        public ItemType Type { get; set; } // �ؿ�ģʽ?���ÿ�����
        public Array enemy { get; set; }//
        public GameLevel gameLevel { get; set; } // �˻��Ѷ�
        public Array rewardType { get; set; } // �������ͣ�
        public Array rewardCount { get; set; } // ����������
        public Array Pretask { get; set; } // ǰ�����񣬲������飬�ɷ�������������
        public Array Posttask { get; set; } // �������񣬲������飬�ɷ�������������
        public bool isRepeat { get; set; } // �Ƿ�����ظ�����
        public bool isFinish { get; set; } // ������
        public int scores { get; set; } // �÷�

        //���·����ѡ���Ƿ�����

        public int Number { get; set; } // ��������
        public int Tasknodes { get; set; }//����ڵ�
        public int CurrentNode { get; set; }//��ǰ�ڵ�


        //ͬʱ���Կ����Ƿ��RoomDefine�ϲ������������Ҫ�����������͵�һЩ���ƣ�����˵�ȼ����Ƿ�ӵ�к��ѣ��ȵȣ����Կ���
        public override string ToString()
        {
            return $"ID: {ID}, " +
                   $"Name: {Name}, " +
                   $"Description: {Description}, " +
                   $"HurtType: {Type}, " +
                   $"GameLevel: {gameLevel}, " +
                   $"RewardType: {rewardType}, " +
                   $"RewardCount: {rewardCount}, " +
                   $"Pretask: {Pretask}, " +
                   $"Posttask: {Posttask}, " +
                   $"IsRepeat: {isRepeat}, " +
                   $"IsFinish: {isFinish}, " +
                   $"Scores: {scores}, " +
                   $"Number: {Number}, " +
                   $"Tasknodes: {Tasknodes}, " +
                   $"CurrentNode: {CurrentNode}";
        }
    }
}