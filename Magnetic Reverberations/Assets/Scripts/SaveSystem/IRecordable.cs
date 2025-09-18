//using System;
//using System.Collections.Generic;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
//using Spine;
//using Unity.VisualScripting;
//using UnityEngine;
//using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


///// <summary>
///// �ɼ�¼����ӿڣ��κ���Ҫ��¼��/�طŵ���Ϸ����Ӧʵ�ִ˽ӿ�
///// ��GetDataType()�����У�ֻ��ӱ仯������
///// CharacterMove����ʾ����ע�Ͳ���
///// </summary>
//internal interface IRecordable
//{
//    /// <summary>
//    /// ��ȡ�����Ψһ��ʶ��
//    /// </summary>
//    /// <returns>ΨһID�ַ���</returns>
//    string GetID();

//    /// <summary>
//    /// ����ǰ�����״̬
//    /// </summary>
//    /// <returns>����״̬���ݵ��ֵ�</returns>
//    Dictionary<string, object> CaptureState();

//    /// <summary>
//    /// �������лָ�����״̬
//    /// </summary>
//    /// <param name="state">״̬�����ֵ�</param>
//    void RestoreState(Dictionary<string, object> state);

//    /// <summary>
//    /// ���ö����Ƿ�Ӧ�ñ���¼
//    /// </summary>
//    /// <returns>�Ƿ�Ӧ�ü�¼</returns>
//    bool ShouldRecord();
//}



///// <summary>
///// �����Ļط�������������������֡���ݺ�Ԫ��Ϣ
///// </summary>
//[Serializable]
//public class ReplayData
//{
//    public int totalFrames; // ��֡��
//    public float totalTime; // ��ʱ��
//    public List<SerializableKeyValuePair> frames = new List<SerializableKeyValuePair>(); // ����֡�����б�

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

//    // ��֡�������л���װ��
//    [System.Serializable]
//    public class SerializableKeyValuePair
//    {
//        public int frame;
//        public float time;
//        public string objectStates;
//    }
//}



///// <summary>
///// ��֡�����������洢ĳһʱ�����пɼ�¼�����״̬
///// </summary>
//[Serializable]
//public class FrameData
//{
//    public int frame;  // ֡���
//    public float time; // ʱ���
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