using Cinemachine;
using Spine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

/// <summary>
/// 用于平衡相机和人物的移动、朝向方向
/// </summary>
public class Balance : MonoBehaviour
{
    [Header("绑定节点")]
    [Tooltip("头节点")]
    public GameObject head;
    [Tooltip("人物")]
    public CharacterMove Character;
    [Header("相机")]
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    [Header("InputSystem绑定")]
    [Tooltip("InputSystem绑定")]
    [SerializeField] private InputActionAsset playerControls;//输入系统资源
    [SerializeField] private InputAction CameraChange;//切换视角输入

    //辅助
    CinemachinePOV s1;//第一人称相机的POV组件
    private float lastcurrentValue;//用于回正

    //运行时变量
    private bool isFirstPerson = false;//是否为第一人称
    private bool _isMoving =false;//是否在移动
    public Vector2 input;//输入
    public Vector3 moveDirection;//应该要移动的方向

    
    void Start()//初始化
    {
        s1 = firstPersonCamera.GetComponentInChildren<CinemachinePOV>();
        InputActionMap playerMap = playerControls.FindActionMap("Player");
        CameraChange = playerMap.FindAction("CameraChange");
        CameraChange.performed +=OnChanged;
        StartCoroutine(RotateHead());
    }
    void OnDestroy()//销毁时调用
    {
        CameraChange.performed -= OnChanged;
        StopCoroutine(RotateHead());
    }

    void LateUpdate()//用于视角变换，每帧时调用
    {
        OnMove();
        if (isFirstPerson)firstBalance();
        else thirdBalance();
    }
    void OnChanged(InputAction.CallbackContext context)//按下V切换视角
    {
        
            isFirstPerson = !isFirstPerson;
            thirdPersonCamera.Priority = isFirstPerson ? 0 : 10;
            firstPersonCamera.Priority = isFirstPerson ? 10 : 0;
            head.transform.rotation = new Quaternion    (0, 0, 0, 1);
    }
    void thirdBalance()//第三人称视角规范
    {

        float signedAngle = Vector3.SignedAngle(Character.transform.forward, Vector3.ProjectOnPlane(thirdPersonCamera.transform.forward, Vector3.up), Vector3.up);
        if((Mathf.Abs(signedAngle) > 150f)|| _isMoving)
        {
            head.transform.rotation = Quaternion.Slerp(
                head.transform.rotation,
                Character.transform.rotation,
                Time.deltaTime * 3f
            );
            return;
        }
        float targetAngle = Mathf.Clamp(signedAngle, -75f, 75f);
        head.transform.rotation = Quaternion.Slerp(
                head.transform.rotation,
                Quaternion.Euler(thirdPersonCamera.transform.eulerAngles.x,Character.transform.eulerAngles.y + targetAngle,thirdPersonCamera.transform.eulerAngles.z),
                Time.deltaTime * 3f
        );
    }
    void firstBalance()//第一人称视角规范
    {
            if (Vector3.Angle(Character.transform.forward, Vector3.ProjectOnPlane(firstPersonCamera.transform.forward, Vector3.up)) > (_isMoving ? 45:95))
            {
                s1.m_HorizontalAxis.Value = lastcurrentValue;
            }
            else
            {
                lastcurrentValue= s1.m_HorizontalAxis.Value;
            }
        head.transform.rotation = firstPersonCamera.transform.rotation;
    }

    private void OnMove()//移动方向应用
    {
        input = new Vector2(
            Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue(),
            Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue()
        ).normalized;
        _isMoving = input.sqrMagnitude >= 0.5f;
        if (_isMoving)
        {
            Transform t = Character.transform;
            moveDirection = isFirstPerson ?
                (t.forward * input.y + Vector3.ProjectOnPlane(firstPersonCamera.transform.right, Vector3.up) * input.x) :
                t.forward;
        }
        Character.UpdateMove(_isMoving ? moveDirection.normalized : Vector3.zero);
    }
    IEnumerator RotateHead()//人物朝向转动协程
    {
        while (true)
        {
            yield return null;
            if (!_isMoving) continue;
            Transform cam = isFirstPerson ? firstPersonCamera.transform : thirdPersonCamera.transform;
            var (x, z) = (cam.forward.x, cam.forward.z);
            Vector3 targetDir = isFirstPerson
                ? new Vector3(x, 0, z)
                : new Vector3(
                    input.y * x + input.x * z,
                    0,
                    input.y * z - input.x * x 
                ).normalized;
            Character.transform.rotation = Quaternion.Slerp(
                Character.transform.rotation,
                Quaternion.LookRotation(targetDir),
                5 * Time.deltaTime
            );
        }
    }
}