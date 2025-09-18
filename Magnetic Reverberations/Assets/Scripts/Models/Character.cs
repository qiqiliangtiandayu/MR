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
    private CharacterDefine attribute;//���� 
    public CharacterDefine Attribute//ֻ���������ⲿ��ȡ
    {
        get => attribute;
        set
        {
            attribute = value;
            move.Initialize(attribute, attribute.ID);
        }
    }
    public baseStateDefine currentState//��ǰ״̬�����漰���Ըı䣬���Թ���������״̬�л��� 
    {
        get => currentState;
        set 
        { 
            currentState = value;
        } 
    }
    private CharacterMove move;//��Ӧ�ƶ����
    public void OnEnable()//���ģ�����ʱ����
    {
        move = gameObject.AddComponent<CharacterMove>();
    }
    public void OnDisable()//ȡ�����ģ��ǻʱ����
    {
    }
    public void UpdateAttributes(CharacterDefine attributes, baseStateDefine state) // ���½�ɫ���Ժ�״̬
    {
        attribute = attributes;
        currentState = state;
        if (state.NextID!= 0) StateDurationTimer(state.StateTime);
    }
    private void HandleMoveStateChange()//��ʱ���˸�ʲô�õ���
    {
        //����֪����ʲô�õģ�������
    }
    private IEnumerator StateDurationTimer(float duration)//����״̬����ʱ���Э��
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
