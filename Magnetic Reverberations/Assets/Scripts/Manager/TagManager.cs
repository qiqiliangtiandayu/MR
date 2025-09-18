using Common.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TagManager : MonoSingleton<TagManager>
{
    private Dictionary<string, List<GameObject>> _tagIndex = new();

    // 注册标签
    public void RegisterTag(GameObject obj, string tag)
    {
        if (!_tagIndex.ContainsKey(tag))
            _tagIndex.Add(tag, new List<GameObject>());
        _tagIndex[tag].Add(obj);
    }

    // 查询带标签的对象
    public List<GameObject> FindWithTag(string tag)
    {
        return _tagIndex.TryGetValue(tag, out var list) ? list : new();
    }

    // 分层查询（如"状态.*"）
    public List<GameObject> FindWithHierarchyTag(string parentTag)
    {
        return _tagIndex.Where(kv => kv.Key.StartsWith(parentTag))
                       .SelectMany(kv => kv.Value).ToList();
    }
}