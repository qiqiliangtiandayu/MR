using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class pos : MonoBehaviour, IScrollHandler
{

    public ScrollRect scrollRect;
    public float scrollSpeed = 0.5f;

    private void Update()
    {
        if (scrollRect == null) return;
        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScroll != 0)
        {
            float offsetX = -mouseScroll * scrollSpeed * Time.deltaTime * 1000;
            Vector2 newPos = scrollRect.content.anchoredPosition + new Vector2(offsetX, 0);
            scrollRect.content.anchoredPosition = newPos;
        }
    }
    public void OnScroll(PointerEventData eventData)
    {
    }
}