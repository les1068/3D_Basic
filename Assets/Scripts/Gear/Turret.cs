using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;   // 발사할 총알 프리팹
    public float fireInterval;        // 총알 발사 시간 간격

    protected Transform fireTransform;          // 총알이 발사되는 트랜스폼
    protected Transform BarrelbodyTransform;    // 총몸의 트랜스폼
    protected IEnumerator fireCoroutine;     // 총알을 계속 발사하는 코루틴
    WaitForSeconds waitFireInterval;         // 총알 발사 간격만큼 기다리는 WaitForSeconds

    protected virtual void Awake()
    {
        BarrelbodyTransform = transform.GetChild(2);          // Barrelbody
        fireTransform = BarrelbodyTransform.GetChild(1);      // fireTransform
        fireCoroutine = PeriodFire();
    }
    protected virtual void Start()
    {
        waitFireInterval = new WaitForSeconds(fireInterval);
    }
    IEnumerator PeriodFire()  // 주기적으로 총알을 발사하는 코루틴
    {
        while (true)
        {
            OnFire();
            yield return waitFireInterval;
        }
    }
    protected virtual void OnFire() // 총알을 한발 발사하는 함수
    {

    }
}
