//using System.Collections.Generic;
//using UnityEngine;

//public class UIEffectManager : MonoSingleton<UIEffectManager>
//{
//    // 对象池管理
//    private Dictionary<EffectType, Queue<UIBaseEffect>> effectPools;

//    // 特效配置表
//    private Dictionary<string, EffectConfig> effectConfigs;

//    // 核心接口
//    public UIBaseEffect PlayEffect(string effectID, Vector2 startPos, Transform target = null);
//    public void PauseEffect(UIBaseEffect effect);
//    public void UpdateEffectTarget(UIDirectionalEffect effect, Transform newTarget);
//    public void RecycleEffect(UIBaseEffect effect);

//    // 对象池预热
//    public void PrewarmPool(string effectID, int count);
//}