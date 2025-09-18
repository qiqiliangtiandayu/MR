using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;

public class InteractionManager : MonoSingleton<InteractionManager>
{
    [Header("UI")]
    [Tooltip("UI相关设置")]
    [SerializeField] private string cinema3d;//第三人称相机名称
    [SerializeField] private Transform uiRoot;//UI根节点
    [SerializeField] private Arrow arrow3d;//箭头3D
    [SerializeField] private GameObject optionPrefab;//选项预制体
    [SerializeField] private Color selectedColor = Color.cyan;//选中颜色
    [Header("交互")]
    [Tooltip("交互相关设置")]
    [SerializeField] private LayerMask interactableMask;//可交互层
    [SerializeField][Range(1f, 3f)] private float checkRadius = 3f;//检测范围
    
    //运行时数据
    private const int MaxCandidates = 8;//最大候选数量
    private readonly Collider[] hitColliders = new Collider[MaxCandidates];//碰撞体缓存
    private List<ItemPickupHandler> candidates = new List<ItemPickupHandler>(4);//候选列表
    private List<GameObject> uiPool = new List<GameObject>(4);//UI对象池
    public int currentIndex;//当前选中索引
    private LayerMask playerLayerMask;//玩家层
    private Camera mainCam;//主相机

    private void Start()//初始化
    {
        mainCam = Camera.main;
        playerLayerMask = ~(1 << LayerMask.NameToLayer("Player"));

        for (int i = 0; i < 4; i++)
            uiPool.Add(Instantiate(optionPrefab, uiRoot));

        StartCoroutine(DetectionRoutine());
    }

    private IEnumerator DetectionRoutine()//检测协程
    {
        while (true)
        {
            UpdateCandidates();
            RefreshUI();
            yield return new WaitForSeconds(InteractionController.Instance.IsHoldingItem ? 0.05f : 0.2f);
        }
    }

    private void UpdateCandidates()//更新候选
    {
        candidates.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, checkRadius, hitColliders, interactableMask);

        for (int i = 0; i < count; i++)
        {
            if (hitColliders[i].TryGetComponent(out ItemPickupHandler item))
            {
                // 检查是否有遮挡（从摄像机到物体的射线检测）
                Vector3 cameraPos = mainCam.transform.position;
                Vector3 itemPos = item.transform.position;
                Vector3 direction = itemPos - cameraPos;
                float distance = direction.magnitude;
                direction.Normalize();
                // 使用 LayerMask 过滤遮挡检测的层级（例如忽略玩家自身）
                LayerMask occlusionMask = ~LayerMask.GetMask("Player", "IgnoreRaycast");
                bool isOccluded = Physics.Raycast(
                    cameraPos,
                    direction,
                    out RaycastHit hit,
                    distance,
                    occlusionMask
                );

                // 如果射线未命中任何物体，或命中的是物体自身，则视为无遮挡
                if (!isOccluded || hit.collider.gameObject == item.gameObject)
                {
                    candidates.Add(item);
                }
            }
        }
        //排序
        candidates.Sort((a, b) =>
            Vector3.Distance(a.transform.position, transform.position)
            .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
        currentIndex = Mathf.Clamp(currentIndex, 0, candidates.Count - 1);
        if (candidates.Count==0) currentIndex = -1;
    }

    private void RefreshUI()//刷新UI
    {
        for (int i = 0; i < uiPool.Count; i++)
        {
            bool active = i < candidates.Count;
            uiPool[i].SetActive(active);

            if (active)
            {
                var text = uiPool[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text = candidates[i].showText;
                text.color = (i == currentIndex) ? selectedColor : Color.white;
            }
        }
        ItemPickupHandler current = (currentIndex >= 0 && currentIndex < candidates.Count) ? candidates[currentIndex] : null;
        if (current != null)
            arrow3d.TargetChanged(current.transform, current.surfaceOffset);
        else
            arrow3d.TargetChanged(null);
    }

    private void Update()//鼠标滚轮更新，之后可更改为InputSystem
    {
        if (Input.mouseScrollDelta.y != 0 && candidates.Count > 0)
        {
            currentIndex = (currentIndex + (int)Mathf.Sign(Input.mouseScrollDelta.y) + candidates.Count) % candidates.Count;
            RefreshUI();
        }
    }

    public void ConfirmInteraction(ItemPickupHandler item = null)//确认交互
    {
        var controller = InteractionController.Instance;
        if (item != null)
        {
            item.DropDown(GetDropPosition());
            controller.DropItem();
        }
        else if (currentIndex>=0&& currentIndex < candidates.Count)
        {
            if (candidates[currentIndex].PickUp(controller.HandTransform))
            {
                controller.NotifyItemEquipped(candidates[currentIndex]);
                Messagetip.Instance.ShowMessage(candidates[currentIndex].showText);
            }
            
        }
        UpdateCandidates();
    }

    private Vector3 GetDropPosition()//获取放置位置,需要修改，兼容动画之后，不能这么生硬的指定位置，或者指定前方地面的某一处
    {
        var ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        CinemachineBrain brain = mainCam.GetComponent<CinemachineBrain>();
        ICinemachineCamera virtualCam = brain.ActiveVirtualCamera;
        if (virtualCam.Name == cinema3d)
        {
            return Physics.Raycast(ray, out RaycastHit hit, 15f, playerLayerMask)
            ? hit.point + Vector3.up * 0.5f : ray.GetPoint(6f);
        }
        else
        {
            return Physics.Raycast(ray, out RaycastHit hit, 3f,playerLayerMask)
            ? hit.point+Vector3.up*0.5f : ray.GetPoint(2f);
        }
        
    }
}