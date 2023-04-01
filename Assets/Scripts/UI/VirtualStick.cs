using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler
{
    RectTransform containerRect;
    RectTransform handleRect;

    float stickRange;
    void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;
        //handleRect = child.GetComponent<RectTransform>();
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        // containerRect의 피봇에서 얼마만큼 이동했는지가 position에 들어감
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect, eventData.position,eventData.pressEventCamera, out Vector2 position);
        position = Vector2.ClampMagnitude(position, stickRange);

        handleRect.anchoredPosition = position;
    }
}

