//using UnityEngine;
//using System.Collections.Generic;
//using System;

//public static class DictionarySerializer
//{
//    // 字典序列化封装类
//    [System.Serializable]
//    public class SerializableDictionary
//    {
//        public List<SerializableKeyValuePair> items = new List<SerializableKeyValuePair>();
//    }

//    [System.Serializable]
//    public class SerializableKeyValuePair
//    {
//        public string key;
//        public string value;
//    }


//    public static string Serialize(Dictionary<string, string> dict)
//    {
//        SerializableDictionary wrapper = new SerializableDictionary();

//        foreach (var kvp in dict)
//        {
//            wrapper.items.Add(new SerializableKeyValuePair
//            {
//                key = kvp.Key,
//                value = kvp.Value
//            });
//        }

//        return JsonUtility.ToJson(wrapper);
//    }

//    public static Dictionary<string, string> DeserializeString(string json)
//    {
//        SerializableDictionary wrapper = JsonUtility.FromJson<SerializableDictionary>(json);
//        Dictionary<string, string> dict = new Dictionary<string, string>();

//        foreach (var item in wrapper.items)
//        {
//            dict[item.key] = item.value;
//        }

//        return dict;
//    }


//    public static string Serialize(Dictionary<string, object> dict)
//    {
//        SerializableDictionary wrapper = new SerializableDictionary();

//        foreach (var kvp in dict)
//        {
//            wrapper.items.Add(new SerializableKeyValuePair
//            {
//                key = kvp.Key,
//                value = SmartConvertToString(kvp.Value)
//            });
//        }

//        return JsonUtility.ToJson(wrapper);
//    }

//    public static Dictionary<string, object> Deserialize(string json)
//    {
//        SerializableDictionary wrapper = JsonUtility.FromJson<SerializableDictionary>(json);
//        Dictionary<string, object> dict = new Dictionary<string, object>();

//        foreach (var item in wrapper.items)
//        {
//            dict[item.key] = SmartParseString(item.value);
//        }

//        return dict;
//    }

//    private static string SmartConvertToString(object obj)
//    {
//        if (obj == null) return "null";

//        Type type = obj.GetType();

//        if (type == typeof(string)) return (string)obj;
//        if (type == typeof(int)) return obj.ToString();
//        if (type == typeof(float)) return "f:" + ((float)obj).ToString("F6");
//        if (type == typeof(bool)) return obj.ToString().ToLower();
//        if (type == typeof(double)) return "d:" + ((double)obj).ToString("F6");

//        if (type == typeof(Vector3)) return "v3:" + JsonUtility.ToJson(obj);
//        if (type == typeof(Vector2)) return "v2:" + JsonUtility.ToJson(obj);
//        if (type == typeof(Quaternion)) return "q:" + JsonUtility.ToJson(obj);

//        if (type == typeof(int[])) return "ai:" + JsonUtility.ToJson(obj);
//        if (type == typeof(float[])) return "af:" + JsonUtility.ToJson(obj);

//        // 其他复杂类型
//        try
//        {
//            return "j:" + JsonUtility.ToJson(obj);
//        }
//        catch
//        {
//            Debug.Log($"序列化尝试失败：[type:{type.AssemblyQualifiedName}, obj:{obj.ToString()}]");
//            return obj.ToString();
//        }
//    }

//    private static object SmartParseString(string value)
//    {
//        if (value == "null") return null;

//        // 布尔值
//        if (value == "true") return true;
//        if (value == "false") return false;

//        // 带前缀的值
//        if (value.StartsWith("f:")) return float.Parse(value.Substring(2));
//        if (value.StartsWith("d:")) return double.Parse(value.Substring(2));
//        if (value.StartsWith("v3:")) return JsonUtility.FromJson<Vector3>(value.Substring(3));
//        if (value.StartsWith("v2:")) return JsonUtility.FromJson<Vector2>(value.Substring(3));
//        if (value.StartsWith("q:")) return JsonUtility.FromJson<Quaternion>(value.Substring(2));
//        if (value.StartsWith("ai:")) return JsonUtility.FromJson<int[]>(value.Substring(3));
//        if (value.StartsWith("af:")) return JsonUtility.FromJson<float[]>(value.Substring(3));
//        if (value.StartsWith("j:")) return value.Substring(2);

//        // 尝试解析数字
//        if (int.TryParse(value, out int intValue)) return intValue;
//        if (float.TryParse(value, out float floatValue)) return floatValue;

//        // 默认为字符串
//        return value;
//    }
//}