using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    Transform[] waypoints; // 이 웨이포인트에서 사용하는 웨이포인트 지점들

    int index = 0;         // 현재 가고 있는 웨이포인트의 인덱스(번호)

    public Transform CurrentWaypoint => waypoints[index];   // 현재 향하고 있는 웨이포인트의 트랜스폼 확인용 프로퍼티

    private void Awake()
    {
        waypoints = new Transform[transform.childCount]; // 자식 갯수만큼 배열을 만들어라
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
    public Transform GetNextWaypoint() // 다음에 이동해야 할 웨이 포인트를 알려주는 함수
    {
        index++;                        // index 증가
        index %= waypoints.Length;      // index는 0~(waypoints.Length-1)까지만 되어야 한다.
        return waypoints[index];        // 해당 트랜스폼 리턴
    }
}
