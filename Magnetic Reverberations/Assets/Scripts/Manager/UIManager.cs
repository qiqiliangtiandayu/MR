using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Scene currentScene = Scene.LoginSence;

    public enum Scene
    {
        LoadingSence=0,
        LoginSence=1,
        MainSence =2,
        BattleScene=3,
    }

    class UIElent
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;
    }

    private Dictionary<Type, UIElent> UIResources = new Dictionary<Type, UIElent>();

    public UIManager()
    {//������Ҫ��Ԥ�ƽ���
        this.UIResources.Add(typeof(UISetting), new UIElent() { Resources = "UI/UISetting", Cache = true });
    }

    ~UIManager() { }

    public  T Show<T>()
    {
        //SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);��Ч
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElent info = this.UIResources[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if (prefab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }
    public void Close(Type type)
    {
        if (this.UIResources.ContainsKey(type))
        {
            UIElent info = this.UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }

    public void Close<T>()
    {
        this.Close(typeof(T));
    }
}
