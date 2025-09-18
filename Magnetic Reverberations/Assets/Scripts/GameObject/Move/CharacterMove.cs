using Common.Data;
using Managers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove/*��Ϊ���������ƶ�����*/ : MonoBehaviour, IRecordable
{
    public int ID;//��ɫID
    public IMoveDefine config;//��ɫ����

    [Header("������")]
    public CharacterController controller;//������
    public float originalHeight;//ԭʼ�߶�
    public Vector3 originalCenter;//ԭʼ����

    [Header("�������")]
    [SerializeField] private InputActionAsset playerControls;//�������
    private InputAction Run;//����
    private InputAction Jump;//��Ծ
    private InputAction Crouch;//����

    [Header("�ƶ�����")]
    public float verticalVelocity;//��ֱ�ٶ�
    public float currentSpeed;//��ǰ�ٶ�
    public Vector3 moveDirection;//ˮƽ�ƶ�����

    [Header("��������")]
    public float recoverRate;//�����ָ��ٶ�
    public float lastConsumeTime;//��һ������ʱ��
    public float currentStamina;//��ǰ����

    [Header("״̬")]
    public bool isGrounded;//�Ƿ��ڵ���
    public bool isSprinting;//�Ƿ���
    public bool isCrouching;//�Ƿ����
    public bool isJumping;//�Ƿ���Ծ
    public bool isMove;//�Ƿ��ƶ�
    public float BufferTime;//����ʱ��
    public float LastJumpTime;//��һ����Ծʱ��
    void Awake()//��ʼ��
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

    public void Initialize(IMoveDefine config, int id)//��Ϣ��ʼ��
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
            // �ط�ģʽ��ʹ��¼�Ƶ��������ݽ����ƶ�
        }
        else
        {
            // ����ģʽ��ʹ����ʵ�������ݽ����ƶ�
            HandleJump();
            UpdateStamina();
            ApplyMove(moveDirection);
            SpeedDecay();
        }
    }
    public void UpdateMove(Vector3 moveDirection)// �ƶ�����
    {
        isMove = moveDirection == Vector3.zero? false : true;
        if (isJumping||!isMove) return;
        BufferTime = (isMove ? 0.2f : 0.3f);
        this.moveDirection = moveDirection;
        UpdateMovement();
    }
    private void RunPerformed(InputAction.CallbackContext context)//����
    {
        isSprinting=(currentStamina > 10)&& !isSprinting;
    }
    private void JumpPerformed(InputAction.CallbackContext context)//��Ծ
    {
        if( !isGrounded|| currentStamina < 10) return;
        LastJumpTime = Time.time;
        ApplyPhysics();
        currentStamina += config.JumpDeplete;
    }
    private void CrouchPerformed(InputAction.CallbackContext context)// ����
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
    private void UpdateMovement()// �ٶȸ���
    {
        float targetSpeed = isCrouching ? config.SquatSpeed :
                          isSprinting ? config.RunSpeed :
                          config.BaseSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, 8f * Time.deltaTime);
    }
    public void SpeedDecay()//�ٶ�˥��
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0,(isJumping? 0.5f : 6f) * Time.deltaTime);
        if(!isGrounded)verticalVelocity = Mathf.Lerp(verticalVelocity,-10f,  Time.deltaTime);
    }
    private void HandleJump()// ��Ծ����
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
    private void ApplyMove(Vector3 moveDirection)// Ӧ�������ƶ�
    {
        moveDirection.Set(moveDirection.x * currentSpeed,verticalVelocity,moveDirection.z * currentSpeed);
        moveDirection *= Time.deltaTime;
        controller.Move(moveDirection);
    }
    private void UpdateStamina()// ��������
    {
        if (isSprinting&&isMove)// ����״̬����������
        {
            if (currentStamina < 10) isSprinting = false;
            currentStamina += config.RunDeplete * Time.deltaTime;// ������������
        }
        currentStamina = Mathf.Clamp(
            currentStamina + recoverRate * Time.deltaTime,
            0,
            config.Endurance
        );
    }
    private void ApplyPhysics()// ����Ӧ��
    {
            isGrounded = controller.isGrounded;
            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = -2f;
    }
    private IEnumerator ApplyPhysic()//����Ӧ��
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            isGrounded = controller.isGrounded;
            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = -2f;
        }
    }





    [Header("�浵��ʶ")]
    [SerializeField] private string _recordID;
    public string GetID()
    {
#if UNITY_EDITOR
        // ������ʱ��飬����ǿ�˵�������⣬����������ʱID��֤����
        if (string.IsNullOrEmpty(_recordID))
        {
            Debug.LogError($"TimeRecordID is missing on {gameObject.name}!", this);
            _recordID = "TEMPORARY_" + System.Guid.NewGuid().ToString();
        }
#endif
        return _recordID;
    }

#if UNITY_EDITOR
    private void Reset() // ������״���ӻ�����ʱ����
    {
        if (string.IsNullOrEmpty(_recordID))
        {
            _recordID = System.Guid.NewGuid().ToString();
            // ���浽�����ļ���
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
    //    // ����һ֡��ֵ��һ��ʱ���ż�¼��������lastData
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