using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera), typeof(CinemachineInputProvider))]
public class CameraZoom /*此类用于鼠标中键滚动切换视角大小*/: MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField, Range(0f, 10f)] private float defaultDistance = 6f;
    [SerializeField, Range(0f, 10f)] private float minimumDistance = 1f;
    [SerializeField, Range(0f, 10f)] private float maximumDistance = 10f;
    [SerializeField, Range(0f, 10f)] private float smoothing = 4f;
    [SerializeField, Range(0f, 10f)] private float zoomSensitivity = 1f;

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider inputProvider;
    private float currentTargetDistance;
    private void Awake()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
        currentTargetDistance = defaultDistance;
    }
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        Zoom();
    }
    private void Zoom()
    {
        // 1. 获取输入并计算缩放值（支持反向操作）
        float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity; // 乘以-1使输入方向符合直觉
        currentTargetDistance = Mathf.Clamp(
            currentTargetDistance + zoomValue,
            minimumDistance,
            maximumDistance // 添加最大距离限制
        );

        // 2. 获取当前相机实际距离
        float currentDistance = framingTransposer.m_CameraDistance; // 修正拼写错误：原代码为m_CaneraDistance

        if (Vector3.Distance(
    new Vector3(currentDistance, 0),
    new Vector3(currentTargetDistance, 0)) < 0.005f) return;

        float lerpedZoomValue = Mathf.MoveTowards(
    currentDistance,
    currentTargetDistance,
    smoothing * Time.deltaTime * Mathf.Abs(currentTargetDistance - currentDistance)
);
        // 5. 应用新距离
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}