using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Spine;
using UnityEngine;


/// <summary>
/// 可记录对象接口，任何需要被录制/回放的游戏对象都应实现此接口
/// 针对不同类型的可记录对象，要创建不同类型的数据类，并实现插值方法
/// CharacterMove类中示例：注释上下部分（不包含注释）
/// </summary>
internal interface IRecordable
{
    /// <summary>
    /// 获取对象的唯一标识符
    /// </summary>
    /// <returns>唯一ID字符串</returns>
    string GetID();

    /// <summary>
    /// 获取对象数据类型
    /// </summary>
    /// <returns>对象数据类的</returns>
    string GetDataType();

    /// <summary>
    /// 捕获当前对象的状态
    /// </summary>
    /// <returns>包含状态数据的字典</returns>
    object CaptureState();

    /// <summary>
    /// 从数据中恢复对象状态
    /// </summary>
    /// <param name="state">状态数据字典</param>
    void RestoreState(object state);

    /// <summary>
    /// 检查该对象是否应该被记录
    /// </summary>
    /// <returns>是否应该记录</returns>
    bool ShouldRecord();
}



/// <summary>
/// 完整的回放数据容器，包含所有帧数据和元信息
/// </summary>
[Serializable]
public class ReplayData
{
    public int totalFrames; // 总帧数
    public float totalTime; // 总时长
    public List<FrameData> frames = new List<FrameData>(); // 所有帧数据列表
}



/// <summary>
/// 单帧数据容器，存储某一时刻所有可记录对象的状态
/// </summary>
[Serializable]
public class FrameData
{
    public int frame;  // 帧编号
    public float time; // 时间戳
    public List<ObjectData> objectStates; //对象状态信息列表

    public FrameData(int frame, float time)
    {
        this.frame = frame;
        this.time = time;
        this.objectStates = new List<ObjectData>();
    }

    public void AddState(string recordID, object state)
    {
        objectStates.Add(new ObjectData(recordID, state));
    }

    public object GetState(string recordID, string typeName)
    {
        var foundObject = objectStates.Find(obj => obj.id == recordID);
        switch (typeName)
        {
            case "PlayerSaveData": return JsonUtility.FromJson<PlayerSaveData>(foundObject.state);
        }
        return default;
    }
}




/// <summary>
/// 对象数据封装类
/// </summary>
[Serializable]
public class ObjectData
{
    public string id;
    public string state;

    public ObjectData(string recordID, object state)
    {
        this.id = recordID;
        this.state = JsonUtility.ToJson(state);
    }
}
