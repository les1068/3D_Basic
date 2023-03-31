using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaypointUser : MonoBehaviour
{
    public Waypoints targetWaypoints;  // 이 오브젝트가 움직일 웨이포인트
    public float moveSpeed = 5.0f;     // 이동 속도

    public Action<Vector3> onMove;  // 이동량을 알려주는 델리게이트
    
    protected Vector3 moveDelta = Vector3.zero;  // 이번 프레임에 이동한 정도

    Transform target;            // 목적지 웨이포인트
    Vector3 moveDir;

    private void Start()
    {
        SetTarget(targetWaypoints.CurrentWaypoint);  // 첫번째 웨이포인트 지점 설정
    }
    protected void Move() // 이동처리용 함수 fixedupdate에서 호출 할것
    {
        moveDelta = Time.fixedDeltaTime * moveSpeed * transform.forward;
        transform.Translate(moveDelta,Space.World);  // 이동

        //(거리 < 0.1), (거리의 제곱 <0.1의 제곱) 둘의 결과는 같다 
        if ( (target.position - transform.position).sqrMagnitude < 0.01f)  // 거리가 0.1보다 작을 때
        { // 도착
            SetTarget(targetWaypoints.GetNextWaypoint());  // 도착했으면 다음 웨이포인트 지점 가져와서 설정하기
            moveDelta = Vector3.zero;
        }
        onMove?.Invoke(moveDelta);
    }
 
    protected virtual void SetTarget(Transform target)  // 다음 웨이포인트 지정하기
    {
        this.target = target;           // 목적지 설정하고
        moveDir = (this.transform.position - target.position).normalized;
    }
}
