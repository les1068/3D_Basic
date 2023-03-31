using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : WaypointUser
{
    private void FixedUpdate()
    {
        Move();
    }
    protected override void SetTarget(Transform target)
    {
        base.SetTarget(target);
        Vector3 lookPosition = target.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);  // 목적지 바라보기
    }
}
