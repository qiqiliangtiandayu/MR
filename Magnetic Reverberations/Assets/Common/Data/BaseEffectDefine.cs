using UnityEngine;

public abstract class BaseEffectDefine
{
    // 基础属性
    //public EffectType effectType { get; protected set; }
    public RectTransform cachedTransform;

    // 生命周期控制
    public abstract void Initialize(Vector2 startPos);
    public abstract void Play();
    public abstract void Pause();
    public abstract void Resume();
    public abstract void Stop();
}