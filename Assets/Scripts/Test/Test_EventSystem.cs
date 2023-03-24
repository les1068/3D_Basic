using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test_EventSystem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, 
    IDragHandler, IBeginDragHandler, IEndDragHandler  // DargHandler를 사용하려면 IDragHandler가 있어야한다.
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("DargStart");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("DargEnd");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Darg");
    }

    public void OnPointerClick(PointerEventData eventData)  // 오브젝트를 마우스로 눌렀을때 실행
    {
        Debug.Log("Click");
    }

    public void OnPointerEnter(PointerEventData eventData)  // 오브젝트에 마우스가 들어갔을 때 실행
    {
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)  // 오브젝트에 마우스가 나갔을 때 실행
    {
        Debug.Log("Exit");
    }
}
