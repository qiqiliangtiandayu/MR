//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem.LowLevel;
//using UnityEngine.Playables;

///// <summary>
///// 回放系统管理器，负责录制、播放和管理所有回放相关功能
///// </summary>
//public class ReplayManager : MonoSingleton<ReplayManager>
//{
//    public enum ReplayMode
//    {
//        Recording,  // 录制模式
//        Playback    // 回溯模式
//    }

//    public bool isRecording;

//    public ReplayMode CurrentMode { get; private set; } = ReplayMode.Recording;

//    private ReplayData currentRecording;    // 当前正在录制的数据
//    private float recordingStartTime;       // 录制开始时间
//    private int recordingCurrentFrame;      // 录制开始帧

//    private ReplayData loadedReplay;        // 已加载的回放数据
//    private float playbackStartTime;       // 录制开始时间
//    private int currentPlaybackFrame;       // 当前回放帧索引
//    private List<IRecordable> allRecordables = new List<IRecordable>(); // 所有可记录对象列表

//    void Start()
//    {
//        if (isRecording) StartRecording();   // 测试录制
//        else LoadAndStartReplay(); // 测试回溯
//    }

//    private void Update()
//    {
//        if (Input.GetKeyUp(KeyCode.P))
//        {
//            // 运行中测试录制
//            LoadAndStartReplay();
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (CurrentMode == ReplayMode.Recording) RecordFrame();           // 录制模式下记录当前帧
//        else if (CurrentMode == ReplayMode.Playback) PlaybackFrame();     // 回放模式下跳转到下一待播放帧
//    }


//    public void StartRecording()
//    {
//        CurrentMode = ReplayMode.Recording;
//        currentRecording = new ReplayData();
//        recordingStartTime = Time.time;
//        recordingCurrentFrame = 1;
//        FindAllRecordables();
//    }

//    public void SaveRecordData(string filename = "TimeData.json")
//    {
//        if (CurrentMode != ReplayMode.Recording) return;

//        currentRecording.totalFrames = recordingCurrentFrame;
//        currentRecording.totalTime = Time.time - recordingStartTime;
//        SavaSystem.SavaJson(filename, currentRecording);

//        recordingCurrentFrame = 1;

//        // 保存成功后，重新开始录制
//        StartRecording();
//    }

//    public void LoadAndStartReplay(string filename = "TimeData.json")
//    {
//        CurrentMode = ReplayMode.Playback;

//        loadedReplay = SavaSystem.LoadJson<ReplayData>(filename);
//        currentPlaybackFrame = 1;
//        playbackStartTime = Time.time;

//        FindAllRecordables();
//    }

//    /// <summary>
//    /// 查找场景中所有实现了IRecordable接口的对象
//    /// </summary>
//    private void FindAllRecordables()
//    {
//        allRecordables.Clear();
//        var allObjects = FindObjectsOfType<MonoBehaviour>();
//        foreach (var obj in allObjects)
//        {
//            if (obj is IRecordable recordable && recordable.ShouldRecord())
//            {
//                allRecordables.Add(recordable);
//            }
//        }
//        Debug.Log($"查找到 {allRecordables.Count} 个所有实现了IRecordable接口的对象");
//    }

//    /// <summary>
//    /// 记录当前帧的所有可记录对象状态
//    /// </summary>
//    private void RecordFrame()
//    {
//        // 创建新的帧数据
//        var frameData = new FrameData(
//            recordingCurrentFrame,
//            Time.time - recordingStartTime
//        );

//        // 超出时间，记录到下一个ReplayData
//        if (frameData.time >= 20)
//        {
//            SaveRecordData();
//            frameData.time -= 20;
//        }

//        // 遍历所有可记录对象并捕获状态
//        foreach (var recordable in allRecordables)
//        {
//            Dictionary<string, object> state = recordable.CaptureState();
//            if (state.Count == 0) continue;  //如果对象信息没有发生变化，不记录
//            frameData.AddState(recordable.GetID(), state);
//        }

//        if (frameData.objectStates.Count == 0) return;

//        currentRecording.AddFrame(frameData);
//        recordingCurrentFrame++;
//        Debug.Log($"添加了新的帧数据，共{frameData.objectStates.Count}个对象信息\n共有{currentRecording.frames.Count}个帧数据，time:{Time.time}");
//    }

//    /// <summary>
//    /// 播放当前回放帧
//    /// </summary>
//    private void PlaybackFrame()
//    {
//        if (currentPlaybackFrame >= loadedReplay.frames.Count)
//        {
//            StartRecording();
//            return;
//        }

//        // 时间没到，暂时不播放
//        if (loadedReplay.frames[currentPlaybackFrame].time < Time.time - playbackStartTime) return;

//        FrameData frame = loadedReplay.GetState(currentPlaybackFrame);

//        foreach (var recordable in allRecordables)
//        {
//            recordable.RestoreState(frame.GetState(recordable.GetID()));
//        }

//        currentPlaybackFrame++;
//    }

//    public bool IsInPlaybackMode()
//    {
//        return CurrentMode == ReplayMode.Playback;
//    }
//}