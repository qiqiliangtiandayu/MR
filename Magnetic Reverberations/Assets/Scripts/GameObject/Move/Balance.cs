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
/// ����ƽ�������������ƶ���������
/// </summary>
public class Balance : MonoBehaviour
{
    [Header("�󶨽ڵ�")]
    [Tooltip("ͷ�ڵ�")]
    public GameObject head;
    [Tooltip("����")]
    public CharacterMove Character;
    [Header("���")]
    public CinemachineVirtualCamera thirdPersonCamera;
    public CinemachineVirtualCamera firstPersonCamera;
    [Header("InputSystem��")]
    [Tooltip("InputSystem��")]
    [SerializeField] private InputActionAsset playerControls;//����ϵͳ��Դ
    [SerializeField] private InputAction CameraChange;//�л��ӽ�����

    //����
    CinemachinePOV s1;//��һ�˳������POV���
    private float lastcurrentValue;//���ڻ���

    //����ʱ����
    private bool isFirstPerson = false;//�Ƿ�Ϊ��һ�˳�
    private bool _isMoving =false;//�Ƿ����ƶ�
    public Vector2 input;//����
    public Vector3 moveDirection;//Ӧ��Ҫ�ƶ��ķ���

    
    void Start()//��ʼ��
    {
        s1 = firstPersonCamera.GetComponentInChildren<CinemachinePOV>();
        InputActionMap playerMap = playerControls.FindActionMap("Player");
        CameraChange = playerMap.FindAction("CameraChange");
        CameraChange.performed +=OnChanged;
        StartCoroutine(RotateHead());
    }
    void OnDestroy()//����ʱ����
    {
        CameraChange.performed -= OnChanged;
        StopCoroutine(RotateHead());
    }

    void LateUpdate()//�����ӽǱ任��ÿ֡ʱ����
    {
        OnMove();
        if (isFirstPerson)firstBalance();
        else thirdBalance();
    }
    void OnChanged(InputAction.CallbackContext context)//����V�л��ӽ�
    {
        
            isFirstPerson = !isFirstPerson;
            thirdPersonCamera.Priority = isFirstPerson ? 0 : 10;
            firstPersonCamera.Priority = isFirstPerson ? 10 : 0;
            head.transform.rotation = new Quaternion    (0, 0, 0, 1);
    }
    void thirdBalance()//�����˳��ӽǹ淶
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
    void firstBalance()//��һ�˳��ӽǹ淶
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

    private void OnMove()//�ƶ�����Ӧ��
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
    IEnumerator RotateHead()//���ﳯ��ת��Э��
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