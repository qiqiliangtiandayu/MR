using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour//用于拾取的目标显示
{
    public Transform target;
    private Camera mainCamera;
    private float surfaceOffset;

    private void Awake()
    {
        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }

     void OnEnable()
    {
        StartCoroutine(Updates());
    }

    void OnDisable()
    {
        StopCoroutine(Updates());
    }
    public void TargetChanged(Transform target, float surfaceOffset = 0.4f)
    {
        if (target == this.target) return;

        this.target = target;
        this.surfaceOffset = surfaceOffset;
        gameObject.SetActive(target!=null);
    }

    private IEnumerator Updates()
    {
        while (true)
        {
            if (target == null)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            var viewportPos = mainCamera.WorldToViewportPoint(target.position);
            bool isVisible = viewportPos.z > 0
                && viewportPos.x > 0 && viewportPos.x < 1
                && viewportPos.y > 0 && viewportPos.y < 1;

            if (isVisible)
            {
                var toCamera = mainCamera.transform.position - target.position;
                transform.position = target.position + toCamera.normalized * surfaceOffset;
                transform.LookAt(mainCamera.transform);
                transform.localScale = Vector3.one * Mathf.Clamp(toCamera.magnitude / 20f, 0.1f, 1f);
            }
            gameObject.SetActive(isVisible);
            yield return null;
        }
    }
}