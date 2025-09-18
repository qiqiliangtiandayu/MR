using Common.Data;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Managers
{
    public class StateManager : Singleton<StateManager>
    {
        private Dictionary<int, StateDefine> States;// 角色状态字典
        public delegate void StateUpdateHandler(int id, StateDefine state);// 状态更新事件
        public static event StateUpdateHandler OnStateUpdated;// 状态更新事件
        public void Init()//初始化
        {
            States = DataManager.Instance.States;
        }
        public void UpdateCharacterState(int characterId, int stateId)// 更新角色状态的方法
        {
            StateDefine state = States[stateId];
            Character character = CharacterManager.Instance.Characters[characterId];
            if (DataManager.Instance.Characters.ContainsKey(characterId))
            {
                // 通过状态机处理属性映射，并应用新的状态和修改后的属性到角色上
                CharacterDefine FinalAttributes = CalculateStateEffects(character.Attribute, state);
                CharacterManager.Instance.ApplyCharacterState(characterId, state, FinalAttributes);
            }
        }
        private CharacterDefine CalculateStateEffects(CharacterDefine baseAttr, StateDefine state)// 状态系数叠加计算
        {
            // 计算角色属性在特定状态下受到的影响，返回新的角色属性值
            return new CharacterDefine
            {
                HP = (baseAttr.HP * state.HP),
                HealthRecover = (baseAttr.HealthRecover * state.HealthRecover),
                Attack = (int)(baseAttr.Attack * state.Attack),
                Defense =(int)( baseAttr.Defense * state.Defense),
                RemoteDiscrete = baseAttr.RemoteDiscrete * state.RemoteDiscrete,
                RangedAttack = (int)(baseAttr.RangedAttack * state.RangedAttack),
                Endurance = (int)(baseAttr.Endurance * state.Endurance),
                RecoverSpeed = (int)(baseAttr.RecoverSpeed * state.RecoverSpeed),
                BaseSpeed = baseAttr.BaseSpeed * state.BaseSpeeed,
                RunSpeed = baseAttr.RunSpeed * state.RunSpeed,
                RunDeplete = (int)(baseAttr.RunDeplete * state.RunDeplete),
                JumpDeplete = (int)(baseAttr.JumpDeplete * state.JumpDeplete),
                JumpSpeed = baseAttr.JumpSpeed * state.JumpSpeed,
                JumpHeight = baseAttr.JumpHeight * state.JumpHeight,
                SquatRecoverSpeed = (int)(baseAttr.SquatRecoverSpeed * state.SquatRecoverSpeed),
                SquatTime = baseAttr.SquatTime * state.SquatTime,
            };
        }
    }
}