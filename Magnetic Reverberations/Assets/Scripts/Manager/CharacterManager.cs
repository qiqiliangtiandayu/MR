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
        public void Init()//初始化
        {
            //Character newChar = new Character();
            //Characters.Add(1, newChar);
        }
        public void ApplyCharacterState(int id, StateDefine state, CharacterDefine attributes)//更新角色的属性及状态
        {//如果角色不存在
            if (!Characters.ContainsKey(id))
            {
                Character newChar = new GameObject($"Character_{id}").AddComponent<Character>();
                newChar.UpdateAttributes(attributes, state);
                Characters.Add(id, newChar);
            }// 如果当前角色的状态优先级低于新状态的优先级，则更新角色状态
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
