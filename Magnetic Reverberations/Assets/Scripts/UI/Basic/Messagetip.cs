using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;

public class Messagetip : MonoSingleton<Messagetip>
{
    [SerializeField] TMP_Text messageText;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] int maxMessages = 5;
    [SerializeField] float messageLifetime = 3f;

    class MessageData
    {
        public string name;
        public int count;
        public float expireTime;
        public float alpha = 1f;
        public Coroutine fadeCor;
        public LinkedListNode<MessageData> node; // 新增节点引用
    }

    LinkedList<MessageData> messages = new LinkedList<MessageData>();
    Dictionary<string, MessageData> msgDict = new Dictionary<string, MessageData>();
    StringBuilder sb = new StringBuilder(128);
    Coroutine updateCoroutine;

    public void ShowMessage(string itemName, int count = 1)
    {
        if (msgDict.TryGetValue(itemName, out var data))
        {
            UpdateExisting(data, count);
        }
        else
        {
            AddNewMessage(itemName, count);
        }
        StartUpdateCoroutine();
        UpdateDisplay();
    }

    void UpdateExisting(MessageData data, int count)
    {
        data.count += count;
        data.expireTime = Time.time + messageLifetime;
        ReorderMessage(data);
    }

    void AddNewMessage(string itemName, int count)
    {
        var node = messages.AddLast(new MessageData());
        node.Value = new MessageData
        {
            name = itemName,
            count = count,
            expireTime = Time.time + messageLifetime,
            node = node
        };
        msgDict.Add(itemName, node.Value);

        while (messages.Count > maxMessages)
        {
            RemoveMessage(messages.First.Value);
        }
    }

    void ReorderMessage(MessageData data)
    {
        messages.Remove(data.node);
        InsertSorted(data);
    }

    void InsertSorted(MessageData data)
    {
        var current = messages.First;
        while (current != null && current.Value.expireTime < data.expireTime)
        {
            current = current.Next;
        }
        data.node = current != null ?
            messages.AddBefore(current, data) :
            messages.AddLast(data);
    }

    IEnumerator AutoUpdate()
    {
        while (true)
        {
            yield return WaitForNextEvent();
            ProcessExpiredMessages();
            UpdateDisplay();
        }
    }

    WaitForSeconds WaitForNextEvent()
    {
        if (messages.Count == 0) return new WaitForSeconds(1f);

        float nearestExpire = messages.First.Value.expireTime;
        float waitTime = Mathf.Max(nearestExpire - Time.time, 0);
        return new WaitForSeconds(waitTime + 0.1f); // 增加缓冲时间
    }

    void ProcessExpiredMessages()
    {
        float now = Time.time;
        foreach (var msg in messages)
        {
            if (msg.expireTime <= now && msg.fadeCor == null)
            {
                msg.fadeCor = StartCoroutine(FadeOut(msg));
            }
        }
    }

    IEnumerator FadeOut(MessageData data)
    {
        float startTime = Time.time;
        while (Time.time < startTime + fadeDuration)
        {
            data.alpha = 1 - (Time.time - startTime) / fadeDuration;
            yield return null;
        }
        RemoveMessage(data);
    }

    void RemoveMessage(MessageData data)
    {
        if (data.fadeCor != null) StopCoroutine(data.fadeCor);
        messages.Remove(data.node);
        msgDict.Remove(data.name);
    }

    void StartUpdateCoroutine()
    {
        if (updateCoroutine == null)
        {
            updateCoroutine = StartCoroutine(AutoUpdate());
        }
    }

    void UpdateDisplay()
    {
        sb.Clear();
        foreach (var msg in messages)
        {
            sb.AppendLine($"<alpha=#{(byte)(msg.alpha * 255):X2}>{msg.name} x{msg.count}");
        }
        messageText.text = sb.ToString();
    }
}