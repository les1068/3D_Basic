using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Earth : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Sun;

    private void Update()
    {
        //transform.LookAt(Sun);  // 특정 방향을 바라보게 만들기
        //transform.rotation = Quaternion.LookRotation(Sun.position - transform.position);
        
        // 특정 지점에 하나의 축을 세우고 그 축을 기준으로 회전시키기
        transform.RotateAround(Sun.position, Sun.up, Time.deltaTime * 90.0f);  // 나(Earth)를 Sun를 기준으로 sun.up방향으로 움직임
    }
}
