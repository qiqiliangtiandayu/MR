//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem.LowLevel;
//using UnityEngine.Playables;

///// <summary>
///// �ط�ϵͳ������������¼�ơ����ź͹������лط���ع���
///// </summary>
//public class ReplayManager : MonoSingleton<ReplayManager>
//{
//    public enum ReplayMode
//    {
//        Recording,  // ¼��ģʽ
//        Playback    // ����ģʽ
//    }

//    public bool isRecording;

//    public ReplayMode CurrentMode { get; private set; } = ReplayMode.Recording;

//    private ReplayData currentRecording;    // ��ǰ����¼�Ƶ�����
//    private float recordingStartTime;       // ¼�ƿ�ʼʱ��
//    private int recordingCurrentFrame;      // ¼�ƿ�ʼ֡

//    private ReplayData loadedReplay;        // �Ѽ��صĻط�����
//    private float playbackStartTime;       // ¼�ƿ�ʼʱ��
//    private int currentPlaybackFrame;       // ��ǰ�ط�֡����
//    private List<IRecordable> allRecordables = new List<IRecordable>(); // ���пɼ�¼�����б�

//    void Start()
//    {
//        if (isRecording) StartRecording();   // ����¼��
//        else LoadAndStartReplay(); // ���Ի���
//    }

//    private void Update()
//    {
//        if (Input.GetKeyUp(KeyCode.P))
//        {
//            // �����в���¼��
//            LoadAndStartReplay();
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (CurrentMode == ReplayMode.Recording) RecordFrame();           // ¼��ģʽ�¼�¼��ǰ֡
//        else if (CurrentMode == ReplayMode.Playback) PlaybackFrame();     // �ط�ģʽ����ת����һ������֡
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

//        // ����ɹ������¿�ʼ¼��
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
//    /// ���ҳ���������ʵ����IRecordable�ӿڵĶ���
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
//        Debug.Log($"���ҵ� {allRecordables.Count} ������ʵ����IRecordable�ӿڵĶ���");
//    }

//    /// <summary>
//    /// ��¼��ǰ֡�����пɼ�¼����״̬
//    /// </summary>
//    private void RecordFrame()
//    {
//        // �����µ�֡����
//        var frameData = new FrameData(
//            recordingCurrentFrame,
//            Time.time - recordingStartTime
//        );

//        // ����ʱ�䣬��¼����һ��ReplayData
//        if (frameData.time >= 20)
//        {
//            SaveRecordData();
//            frameData.time -= 20;
//        }

//        // �������пɼ�¼���󲢲���״̬
//        foreach (var recordable in allRecordables)
//        {
//            Dictionary<string, object> state = recordable.CaptureState();
//            if (state.Count == 0) continue;  //���������Ϣû�з����仯������¼
//            frameData.AddState(recordable.GetID(), state);
//        }

//        if (frameData.objectStates.Count == 0) return;

//        currentRecording.AddFrame(frameData);
//        recordingCurrentFrame++;
//        Debug.Log($"������µ�֡���ݣ���{frameData.objectStates.Count}��������Ϣ\n����{currentRecording.frames.Count}��֡���ݣ�time:{Time.time}");
//    }

//    /// <summary>
//    /// ���ŵ�ǰ�ط�֡
//    /// </summary>
//    private void PlaybackFrame()
//    {
//        if (currentPlaybackFrame >= loadedReplay.frames.Count)
//        {
//            StartRecording();
//            return;
//        }

//        // ʱ��û������ʱ������
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