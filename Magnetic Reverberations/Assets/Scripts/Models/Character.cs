using Common.Data;
using JetBrains.Annotations;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class Character : MonoBehaviour
{
    private CharacterDefine attribute;//属性 
    public CharacterDefine Attribute//只读，用于外部读取
    {
        get => attribute;
        set
        {
            attribute = value;
            move.Initialize(attribute, attribute.ID);
        }
    }
    public baseStateDefine currentState//当前状态，不涉及属性改变，所以公开，用于状态切换等 
    {
        get => currentState;
        set 
        { 
            currentState = value;
        } 
    }
    private CharacterMove move;//对应移动组件
    public void OnEnable()//订阅，启用时调用
    {
        move = gameObject.AddComponent<CharacterMove>();
    }
    public void OnDisable()//取消订阅，非活动时调用
    {
    }
    public void UpdateAttributes(CharacterDefine attributes, baseStateDefine state) // 更新角色属性和状态
    {
        attribute = attributes;
        currentState = state;
        if (state.NextID!= 0) StateDurationTimer(state.StateTime);
    }
    private void HandleMoveStateChange()//暂时忘了干什么用的了
    {
        //还不知道干什么用的，先留着
    }
    private IEnumerator StateDurationTimer(float duration)//计算状态持续时间的协程
    {
        yield return new WaitForSeconds(duration);
        StateManager.Instance.UpdateCharacterState(attribute.ID, currentState.NextID);
    }

    internal void ApplyPercentageDamage(float damage)
    {
        attribute.HP -= damage;
        if (attribute.HP <= 0)
        {
            StateManager.Instance.UpdateCharacterState(attribute.ID, 0);
        }   
    }
}
