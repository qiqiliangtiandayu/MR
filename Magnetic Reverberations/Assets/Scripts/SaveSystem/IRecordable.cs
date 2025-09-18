//using System;
//using System.Collections.Generic;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using Spine;
//using Unity.VisualScripting;
//using UnityEngine;
//using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


///// <summary>
///// 可记录对象接口，任何需要被录制/回放的游戏对象都应实现此接口
///// 在GetDataType()方法中，只添加变化的数据
///// CharacterMove类中示例：注释部分
///// </summary>
//internal interface IRecordable
//{
//    /// <summary>
//    /// 获取对象的唯一标识符
//    /// </summary>
//    /// <returns>唯一ID字符串</returns>
//    string GetID();

//    /// <summary>
//    /// 捕获当前对象的状态
//    /// </summary>
//    /// <returns>包含状态数据的字典</returns>
//    Dictionary<string, object> CaptureState();

//    /// <summary>
//    /// 从数据中恢复对象状态
//    /// </summary>
//    /// <param name="state">状态数据字典</param>
//    void RestoreState(Dictionary<string, object> state);

//    /// <summary>
//    /// 检查该对象是否应该被记录
//    /// </summary>
//    /// <returns>是否应该记录</returns>
//    bool ShouldRecord();
//}



///// <summary>
///// 完整的回放数据容器，包含所有帧数据和元信息
///// </summary>
//[Serializable]
//public class ReplayData
//{
//    public int totalFrames; // 总帧数
//    public float totalTime; // 总时长
//    public List<SerializableKeyValuePair> frames = new List<SerializableKeyValuePair>(); // 所有帧数据列表

//    public void AddFrame(FrameData frame)
//    {
//        frames.Add(new SerializableKeyValuePair
//        {
//            frame = frame.frame,
//            time = frame.time,
//            objectStates = DictionarySerializer.Serialize(frame.objectStates)
//        });
//    }

//    public FrameData GetState(int i)
//    {
//        SerializableKeyValuePair result = frames[i];
//        return new FrameData
//        {
//            frame = result.frame,
//            time = result.time,
//            objectStates = DictionarySerializer.DeserializeString(result.objectStates)
//        };
//    }

//    // 单帧数据序列化封装类
//    [System.Serializable]
//    public class SerializableKeyValuePair
//    {
//        public int frame;
//        public float time;
//        public string objectStates;
//    }
//}



///// <summary>
///// 单帧数据容器，存储某一时刻所有可记录对象的状态
///// </summary>
//[Serializable]
//public class FrameData
//{
//    public int frame;  // 帧编号
//    public float time; // 时间戳
//    public Dictionary<string, string> objectStates;

//    public FrameData()
//    {

//    }

//    public FrameData(int frame, float time)
//    {
//        this.frame = frame;
//        this.time = time;
//        this.objectStates = new Dictionary<string, string>();
//    }

//    public void AddState(string recordID, Dictionary<string, object> state)
//    {
//        objectStates[recordID] = DictionarySerializer.Serialize(state);
//    }

//    public Dictionary<string, object> GetState(string recordID)
//    {
//        return DictionarySerializer.Deserialize(objectStates[recordID]);
//    }
//}