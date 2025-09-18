using Common.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
namespace Managers
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();
        public void Init()//��ʼ��
        {
            //Character newChar = new Character();
            //Characters.Add(1, newChar);
        }
        public void ApplyCharacterState(int id, StateDefine state, CharacterDefine attributes)//���½�ɫ�����Լ�״̬
        {//�����ɫ������
            if (!Characters.ContainsKey(id))
            {
                Character newChar = new GameObject($"Character_{id}").AddComponent<Character>();
                newChar.UpdateAttributes(attributes, state);
                Characters.Add(id, newChar);
            }// �����ǰ��ɫ��״̬���ȼ�������״̬�����ȼ�������½�ɫ״̬
            else if (Characters[id].currentState.Weight < state.Weight)
            {
                Characters[id].UpdateAttributes(attributes, state);
            }
        }

        internal void ApplyPercentageDamage(int iD, float damage)
        {
            if (Characters.ContainsKey(iD))
            {
                Characters[iD].ApplyPercentageDamage(damage);
            }   
        }
    }
}
