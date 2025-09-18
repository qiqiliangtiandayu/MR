using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Developer : MonoBehaviour
{
    [UnityEditor.MenuItem("Developer/DeleteTimeData")]
    public static void DeleteTimeData()
    {
        SavaSystem.DeleteSaveFile("TimeData.json");
        Debug.Log("时间数据已删除");
    }
}
