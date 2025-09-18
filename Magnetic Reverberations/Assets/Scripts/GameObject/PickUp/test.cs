using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public CharacterMove characterMove;
    public void Start()
    {
        characterMove.Initialize(DataManager.Instance.Characters[1], 1);
    }
}
