using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float initialSpeed = 20.0f;    // 시작 이동 속도

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = initialSpeed * transform.forward;   // 초기 운동량 결정
        Destroy(gameObject, 10.0f);  // 시작하고 10초 뒤 삭제
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 2.0f);   // 부딪치면 2초 뒤 삭제
    }
}
