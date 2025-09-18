using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingManager : MonoBehaviour
{
    private void Awake()
    {
        DataManager.Instance.Load();
    }
    void Start()

    {
        //DataManager.Instance.Load();
        CharacterManager.Instance.Init();
        StateManager.Instance.Init();
    }
}
