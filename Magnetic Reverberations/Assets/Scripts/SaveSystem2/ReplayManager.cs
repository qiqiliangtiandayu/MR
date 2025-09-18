using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Playables;

/// <summary>
/// 回放系统管理器，负责录制、播放和管理所有回放相关功能
/// </summary>
public class ReplayManager : MonoSingleton<ReplayManager>
{
    public enum ReplayMode
    {
        Recording,  // 录制模式
        Playback    // 回溯模式
    }

    public bool isRecording;

    public ReplayMode CurrentMode { get; private set; } = ReplayMode.Recording;

    private ReplayData currentRecording;    // 当前正在录制的数据
    private float recordingStartTime;       // 录制开始时间
    private int recordingCurrentFrame;      // 录制当前帧

    private ReplayData loadedReplay;        // 已加载的回放数据
    private int currentPlaybackFrame;       // 当前回放帧索引
    private FrameData lastFrameData;
    private FrameData nextFrameData;
    private List<IRecordable> allRecordables = new List<IRecordable>(); // 所有可记录对象列表

    [SerializeField] private float targetFrameTime = 0.05f;
    private float accumulatedTime = 0f;

    void Start()
    {
        if (isRecording) StartRecording();   // 测试录制
        else LoadAndStartReplay(); // 测试回溯
    }

    private void Update()
    {
        accumulatedTime += Time.deltaTime;
        if (accumulatedTime >= targetFrameTime)
        {
            accumulatedTime -= targetFrameTime;
            if (CurrentMode == ReplayMode.Recording) RecordFrame();           // 录制模式下记录当前帧
            else if (CurrentMode == ReplayMode.Playback) PlaybackFrame();     // 回放模式下跳转到下一待播放帧
        }
        if (CurrentMode == ReplayMode.Playback) PlaybackLerpFrame(accumulatedTime / targetFrameTime);   // 通过插值回放场景

        if (Input.GetKeyUp(KeyCode.P))
        {
            // 运行中测试录制
            LoadAndStartReplay();
        }
    }



    public void StartRecording()
    {
        CurrentMode = ReplayMode.Recording;
        currentRecording = new ReplayData();
        recordingStartTime = Time.time;
        recordingCurrentFrame = 0;
        FindAllRecordables();
    }

    public void SaveRecordData(string filename = "replay.json")
    {
        if (CurrentMode != ReplayMode.Recording) return;

        currentRecording.totalTime = Time.time - recordingStartTime;
        currentRecording.totalFrames = recordingCurrentFrame;
        SavaSystem.SavaJson(filename, currentRecording);

        // 保存成功后，重新开始录制
        StartRecording();
    }

    public void LoadAndStartReplay(string filename = "replay.json")
    {
        CurrentMode = ReplayMode.Playback;

        loadedReplay = SavaSystem.LoadJson<ReplayData>(filename);
        currentPlaybackFrame = 0;

        FindAllRecordables();

        PlaybackFrame();
    }

    /// <summary>
    /// 查找场景中所有实现了IRecordable接口的对象
    /// </summary>
    private void FindAllRecordables()
    {
        allRecordables.Clear();
        var allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (var obj in allObjects)
        {
            if (obj is IRecordable recordable && recordable.ShouldRecord())
            {
                allRecordables.Add(recordable);
            }
        }
        Debug.Log($"查找到 {allRecordables.Count} 个所有实现了IRecordable接口的对象");
    }

    /// <summary>
    /// 记录当前帧的所有可记录对象状态
    /// </summary>
    private void RecordFrame()
    {
        // 创建新的帧数据
        var frameData = new FrameData(
            recordingCurrentFrame,
            Time.time - recordingStartTime
        );

        // 超出时间，记录到下一个ReplayData
        if (frameData.time >= 20)
        {
            SaveRecordData();
            frameData.time -= 20;
        }

        // 遍历所有可记录对象并捕获状态
        foreach (var recordable in allRecordables)
        {
            frameData.AddState(recordable.GetID(), recordable.CaptureState());
        }

        currentRecording.frames.Add(frameData);
        recordingCurrentFrame++;
        if (frameData.time < 20) Debug.Log($"添加了新的帧数据，共{frameData.objectStates.Count}个对象信息\n共有{currentRecording.frames.Count}个帧数据，time:{Time.time}");
    }

    /// <summary>
    /// 跳转到下一帧
    /// </summary>
    private void PlaybackFrame()
    {
        if (currentPlaybackFrame >= loadedReplay.frames.Count - 1)
        {
            foreach (var recordable in allRecordables)
            {
                recordable.RestoreState(loadedReplay.frames[currentPlaybackFrame].GetState(recordable.GetID(), recordable.GetDataType()));
            }
            StartRecording();
            return;
        }
        if (currentPlaybackFrame == 0)
        {
            foreach (var recordable in allRecordables)
            {
                recordable.RestoreState(loadedReplay.frames[1].GetState(recordable.GetID(), recordable.GetDataType()));
            }
        }

        lastFrameData = loadedReplay.frames[currentPlaybackFrame];
        nextFrameData = loadedReplay.frames[currentPlaybackFrame + 1];

        currentPlaybackFrame++;
    }

    /// <summary>
    /// 播放当前回放帧
    /// </summary>
    private void PlaybackLerpFrame(float t)
    {
        // 这里优化，不要每次都获取对象数据
        foreach (var recordable in allRecordables)
        {
            string recordId = recordable.GetID();
            string typeName = recordable.GetDataType();

            object lastobj = lastFrameData.GetState(recordId, typeName);
            object nextobj = nextFrameData.GetState(recordId, typeName);

            switch (typeName)
            {
                case "PlayerSaveData": { recordable.RestoreState(PlayerSaveData.Lerp((PlayerSaveData)lastobj, (PlayerSaveData)nextobj, t)); return; }
            }
        }

        currentPlaybackFrame++;
    }

    public bool IsInPlaybackMode()
    {
        return CurrentMode == ReplayMode.Playback;
    }
}