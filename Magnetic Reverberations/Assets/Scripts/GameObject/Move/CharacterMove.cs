using Common.Data;
using Managers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove/*此为用于人物移动的类*/ : MonoBehaviour, IRecordable
{
    public int ID;//角色ID
    public IMoveDefine config;//角色配置

    [Header("控制器")]
    public CharacterController controller;//控制器
    public float originalHeight;//原始高度
    public Vector3 originalCenter;//原始中心

    [Header("输入控制")]
    [SerializeField] private InputActionAsset playerControls;//输入控制
    private InputAction Run;//奔跑
    private InputAction Jump;//跳跃
    private InputAction Crouch;//蹲下

    [Header("移动控制")]
    public float verticalVelocity;//垂直速度
    public float currentSpeed;//当前速度
    public Vector3 moveDirection;//水平移动方向

    [Header("耐力控制")]
    public float recoverRate;//耐力恢复速度
    public float lastConsumeTime;//上一次消耗时间
    public float currentStamina;//当前耐力

    [Header("状态")]
    public bool isGrounded;//是否在地面
    public bool isSprinting;//是否奔跑
    public bool isCrouching;//是否蹲下
    public bool isJumping;//是否跳跃
    public bool isMove;//是否移动
    public float BufferTime;//缓冲时间
    public float LastJumpTime;//上一次跳跃时间
    void Awake()//初始化
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;
        Run = playerControls.FindAction("Run");
        Jump = playerControls.FindAction("Jump");
        Crouch = playerControls.FindAction("Crouch");
        Run.performed += RunPerformed;
        Jump.performed += JumpPerformed;
        Crouch.performed += CrouchPerformed;
        StartCoroutine(ApplyPhysic());
    }

    void Start()
    {
        
    }

    void OnDestroy()
    {
        
    }

    public void Initialize(IMoveDefine config, int id)//信息初始化
    {
        ID = id;
        this.config = config;
        currentStamina = config.Endurance;
        recoverRate = config.RecoverSpeed;
        controller.slopeLimit = 45f;
        controller.stepOffset = 0.3f;
        controller.skinWidth = 0.08f;
    }
    public void Update()
    {
        if (ReplayManager.Instance.IsInPlaybackMode())
        {
            // 回放模式：使用录制的输入数据进行移动
        }
        else
        {
            // 正常模式：使用真实输入数据进行移动
            HandleJump();
            UpdateStamina();
            ApplyMove(moveDirection);
            SpeedDecay();
        }
    }
    public void UpdateMove(Vector3 moveDirection)// 移动更新
    {
        isMove = moveDirection == Vector3.zero? false : true;
        if (isJumping||!isMove) return;
        BufferTime = (isMove ? 0.2f : 0.3f);
        this.moveDirection = moveDirection;
        UpdateMovement();
    }
    private void RunPerformed(InputAction.CallbackContext context)//奔跑
    {
        isSprinting=(currentStamina > 10)&& !isSprinting;
    }
    private void JumpPerformed(InputAction.CallbackContext context)//跳跃
    {
        if( !isGrounded|| currentStamina < 10) return;
        LastJumpTime = Time.time;
        ApplyPhysics();
        currentStamina += config.JumpDeplete;
    }
    private void CrouchPerformed(InputAction.CallbackContext context)// 蹲下
    {
        isCrouching = !isCrouching;
        if (isCrouching)
        {
            controller.height = config.SquatHeight;
            controller.center =new Vector3(originalCenter.x, config.SquatHeight / 2, originalCenter.z) ;
            recoverRate =config.RecoverSpeed + config.SquatRecoverSpeed;
        }
        else
        {
            controller.height = originalHeight;
            controller.center = originalCenter;
            recoverRate =config.RecoverSpeed;
        }
    }
    private void UpdateMovement()// 速度更新
    {
        float targetSpeed = isCrouching ? config.SquatSpeed :
                          isSprinting ? config.RunSpeed :
                          config.BaseSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 8f * Time.deltaTime);
    }
    public void SpeedDecay()//速度衰减
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0,(isJumping? 0.5f : 6f) * Time.deltaTime);
        if(!isGrounded)verticalVelocity = Mathf.Lerp(verticalVelocity,-10f,  Time.deltaTime);
    }
    private void HandleJump()// 跳跃处理
    {
        float t = Time.time - LastJumpTime;
        if (t < BufferTime)
        {
            float ss = Mathf.Clamp(t+0.1f, 0, BufferTime);
            verticalVelocity =(isMove ? config.JumpSpeed: config.JumpSpeed*1.3f) * (ss / BufferTime);
        }
        if (!isJumping &&t > BufferTime )
        {
            isJumping = true;
        }
        if (isJumping)
        {
            if (t < 0.4f&& Keyboard.current.spaceKey.isPressed)
            {
                verticalVelocity = (isMove ? config.JumpSpeed : config.JumpSpeed * 1.3f) ;
            }
            if (isGrounded)isJumping = false;
        }
    }
    private void ApplyMove(Vector3 moveDirection)// 应用最终移动
    {
        moveDirection.Set(moveDirection.x * currentSpeed,verticalVelocity,moveDirection.z * currentSpeed);
        moveDirection *= Time.deltaTime;
        controller.Move(moveDirection);
    }
    private void UpdateStamina()// 耐力更新
    {
        if (isSprinting&&isMove)// 奔跑状态下消耗耐力
        {
            if (currentStamina < 10) isSprinting = false;
            currentStamina += config.RunDeplete * Time.deltaTime;// 奔跑消耗耐力
        }
        currentStamina = Mathf.Clamp(
            currentStamina + recoverRate * Time.deltaTime,
            0,
            config.Endurance
        );
    }
    private void ApplyPhysics()// 物理应用
    {
            isGrounded = controller.isGrounded;
            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = -2f;
    }
    private IEnumerator ApplyPhysic()//物理应用
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            isGrounded = controller.isGrounded;
            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = -2f;
        }
    }





    [Header("存档标识")]
    [SerializeField] private string _recordID;
    public string GetID()
    {
#if UNITY_EDITOR
        // 在运行时检查，如果是空说明有问题，报错并生成临时ID保证运行
        if (string.IsNullOrEmpty(_recordID))
        {
            Debug.LogError($"TimeRecordID is missing on {gameObject.name}!", this);
            _recordID = "TEMPORARY_" + System.Guid.NewGuid().ToString();
        }
#endif
        return _recordID;
    }

#if UNITY_EDITOR
    private void Reset() // 当组件首次添加或重置时调用
    {
        if (string.IsNullOrEmpty(_recordID))
        {
            _recordID = System.Guid.NewGuid().ToString();
            // 保存到场景文件中
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public string GetDataType()
    {
        return "PlayerSaveData";
    }

    public object CaptureState()
    {
        return new PlayerSaveData()
        {
            pos = transform.position,
            rot = transform.rotation
        };
    }

    public void RestoreState(object state)
    {
        if (state is PlayerSaveData data)
        {
            controller.enabled = false;
            transform.position = data.pos;
            transform.rotation = data.rot;
            controller.enabled = true;
            moveDirection = Vector3.zero;
            currentSpeed = 0;
            isJumping = false;
            verticalVelocity = -2f;
        }
    }

    //Dictionary<string, object> lastData = new Dictionary<string, object>
    //{
    //    ["pos"] = Vector3.zero,
    //    ["rot"] = Quaternion.identity,
    //};

    //public Dictionary<string, object> CaptureState()
    //{
    //    Dictionary<string, object> currentData = new Dictionary<string, object>();
    //    // 和上一帧的值不一样时，才记录，并更新lastData
    //    if (transform.position != (Vector3)lastData["pos"])
    //    {
    //        currentData["pos"] = transform.position;
    //        lastData["pos"] = transform.position;
    //    }
    //    if (transform.rotation != (Quaternion)lastData["rot"])
    //    {
    //        currentData["rot"] = transform.rotation;
    //        lastData["rot"] = transform.rotation;
    //    }

    //    return currentData;
    //}

    //public void RestoreState(Dictionary<string, object> state)
    //{
    //    if (state.TryGetValue("pos", out object pos)) { transform.position = (Vector3)pos; }
    //    if (state.TryGetValue("rot", out object rot)) { transform.rotation = (Quaternion)rot; }
    //}

    public bool ShouldRecord() => true;
}

public class PlayerSaveData
{
    public Vector3 pos;
    public Quaternion rot;

    public static PlayerSaveData Lerp(PlayerSaveData data1, PlayerSaveData data2, float t)
    {
        return new PlayerSaveData
        {
            pos = Vector3.Lerp(data1.pos, data2.pos, t),
            rot = Quaternion.Lerp(data1.rot, data2.rot, t)
        };
    }
}