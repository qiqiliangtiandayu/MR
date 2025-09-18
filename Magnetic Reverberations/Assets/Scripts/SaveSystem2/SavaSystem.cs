using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SavaSystem
{
    public static void SavaJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        Debug.Log(json);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"����ɹ���path:{path}");
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"����ʧ�ܣ�path:{path}\nexception:{exception}");
        }
    }

    public static T LoadJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            Debug.Log($"���سɹ���path:{path}");
            return data;
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"����ʧ�ܣ�path:{path}\nexception:{exception}");
            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"ɾ��ʧ�ܣ�path:{path}\nexception:{exception}");
        }
    }
}
