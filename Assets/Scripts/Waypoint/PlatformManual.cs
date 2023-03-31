using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManual : Platform,IUseableObject
{
    bool isMoving = false;  // 플랫폼이 현재 움직임 여부를 설정하는 변수. true면 움직이고 false면 안움직인다.
    bool IsMoving           // private 프로퍼티. 내부에서 isMoving이 변경될 때 실행될 함수 적용
    {
        get => isMoving;
        set
        {
            isMoving = value;
            if(isMoving)
            {
                ActivatePlatform();
            }
            else
            {
                DeactivatePlatform();
            }
        }
    }

    public bool IsDirectUse => false;   // 직접 사용 금지

    private void FixedUpdate()
    {
        if (isMoving)     // isMoving이 true일 때만 실행
        {
            Move();
        }
    }
    void ActivatePlatform()   // 움직이기 시작할 때 실행될 함수
    {
        
    }
    void DeactivatePlatform()  // 정지 시킬 때 실행될 함수
    {
        
    }

    public void Used()  // 아이템을 사용하면 실핼되는 함수
    {
        IsMoving = !IsMoving;     // IsMoving만 반대로 변경
    }
}
