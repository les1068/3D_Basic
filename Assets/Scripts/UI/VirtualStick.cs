using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Action<Vector2> onMoveInput;  // 가상스틱의 입력을 알리는 델리게이트
    RectTransform containerRect;         // 전체 영역의 rect
    RectTransform handleRect;            // 핸들의 rect

    float stickRange;                    // 핸들이 움직일 수 있는 최대 거리

    void Start()
    {

    }
    void Awake()
    {
        containerRect = transform as RectTransform;
        Transform child = transform.GetChild(0);
        handleRect = child as RectTransform;
        //handleRect = child.GetComponent<RectTransform>();
        stickRange = (containerRect.rect.width - handleRect.rect.width) * 0.5f;   // 최대 거리 구하기
    }
    public void OnDrag(PointerEventData eventData)
    {
        // containerRect의 피봇에서 얼마만큼 이동했는지가 position에 들어감
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            containerRect, eventData.position,eventData.pressEventCamera, out Vector2 position);
        position = Vector2.ClampMagnitude(position, stickRange);  // 움직임이 stickRange를 넘지 않게 클램프

        InputUpdate(position);   // position만큼 핸들 움직이고 신호보내기
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputUpdate(Vector2.zero);  // 마우스를 땠을 때 핸들 중립 위치로 초기화
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //OnPointerUp 때문에 넣은 것. 없으면 OnPointerUp이 실행이 되지 않는다.
    }
    private void InputUpdate(Vector2 pos)  // 스틱의 움직임을 입력으로 변경해서 핸들을 움직이고 신호를 보내는 함수 / pos : 움직인 양
    {
        handleRect.anchoredPosition = pos;      // 앵커에서 pos만큼 이동
        onMoveInput?.Invoke(pos / stickRange);  // 입력 신호 보내기(범위는 -1~1사이)
    }

}

